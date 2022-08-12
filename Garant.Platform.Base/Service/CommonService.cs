using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.DataBase;
using Garant.Platform.Base.Abstraction;
using Garant.Platform.Base.Exceptions;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Exceptions;
using Garant.Platform.Core.Logger;
using Garant.Platform.Core.Utils;
using Garant.Platform.Mailings.Abstraction;
using Garant.Platform.Models.Entities.User;
using Garant.Platform.Models.Mailing.Output;
using Garant.Platform.Models.User.Output;
using Microsoft.EntityFrameworkCore;

namespace Garant.Platform.Base.Service
{
    /// <summary>
    /// Сервис общих методов.
    /// </summary>
    public sealed class CommonService : ICommonService
    {
        private readonly IMailingService _mailigSmsService;
        private readonly PostgreDbContext _postgreDbContext;

        public CommonService(IMailingService mailigSmsService)
        {
            var dbContext = AutoFac.Resolve<IDataBaseConfig>();
            _postgreDbContext = dbContext.GetDbContext();
            _mailigSmsService = mailigSmsService;
        }

        /// <summary>
        /// Метод создает код. Кол-во цифр зависит от переданного типа.
        /// </summary>
        /// <param name="data">Телефон или почта.</param>
        /// <returns>Флаг успеха.</returns>
        public async Task<MailngOutput> GenerateAcceptCodeAsync(string data)
        {
            try
            {
                var random = new Random();
                string type = null;

                if (string.IsNullOrEmpty(data))
                {
                    throw new EmptyTypeMailingException();
                }

                // Создаст код подтверждения из 5 цифр.
                var code = random.Next(10000, 99999).ToString("D4");

                // Если передали почту.
                if (data.Contains("@"))
                {
                    type = "mail";
                }

                // Если не почту, тогда по sms.
                else if (!data.Contains("@"))
                {
                    type = "sms";
                }

                if (type.Equals("sms"))
                {
                    // Запишет код подтверждения в базу или обновит его.
                    await SaveCodeAsync(data, code);

                    // Отправит код подтверждения по смс.
                    await _mailigSmsService.SendAcceptCodeSmsAsync(data, code);
                }

                else if (type.Equals("mail"))
                {
                    // Запишет код подтверждения в базу или обновит его.
                    await SaveCodeAsync(data, code);

                    await _mailigSmsService.SendAcceptCodeMailAsync(code, data);
                }

                var result = new MailngOutput
                {
                    IsSuccessMailing = true,
                    TypeMailing = type
                };

                return result;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);

                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogCritical();
                throw;
            }
        }

        /// <summary>
        /// Метод создаст код сброса пароля, запишет его в базу данных и отправит пользователю.
        /// </summary>
        /// <param name="email"> Почта. </param>
        /// <returns> Флаг успеха. </returns>
        public async Task<MailngOutput> GenerateResetCodeAsync(string email)
        {
            try
            {
                var random = new Random();
                string type = null;

                if (string.IsNullOrEmpty(email))
                {
                    throw new EmptyTypeMailingException();
                }

                // Создаст код сброса пароля из 5 цифр.
                var code = random.Next(10000, 99999).ToString("D4");

                type = "mail";

                // Запишет код сброса в базу или обновит его.
                await SaveCodeAsync(email, code);

                await _mailigSmsService.SendAcceptCodeMailAsync(code, email);

                var result = new MailngOutput
                {
                    IsSuccessMailing = true,
                    TypeMailing = type
                };

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogCritical();
                throw;
            }
        }

        /// <summary>
        /// Метод проверит соответствие полученного кода с кодом в базе данных и почтой.
        /// </summary>
        /// <param name="email"> Почта. </param>
        /// <param name="code"> Полученный код. </param>
        /// <returns> Флаг успеха. </returns>
        public async Task<bool> CheckCodeAsync(string email, string code)
        {
            try
            {
                if (string.IsNullOrEmpty(code))
                {
                    throw new EmptyTypeMailingException();
                }

                var findedUser = await _postgreDbContext.Users
                    .Where(b => b.Code.Equals(code))
                    .Select(b => b)
                    .FirstOrDefaultAsync();

                return findedUser.Email == email;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogCritical();
                throw;
            }
        }

        /// <summary>
        /// Метод изменит пароль пользователя.
        /// </summary>
        /// <param name="email"> Почта. </param>
        /// <param name="password"> Новый пароль. </param>
        /// <returns> Флаг успеха. </returns>
        public async Task<bool> ChangePasswordAsync(string email, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    throw new EmptyTypeMailingException();
                }

                var passwordHash = await HashPasswordAsync(password);

                var getUser = await (from u in _postgreDbContext.Users
                                     where u.Email.Equals(email)
                                     select u)
                            .FirstOrDefaultAsync();

                var oldPassword = getUser.UserPassword;

                getUser.UserPassword = password;
                getUser.PasswordHash = passwordHash;

                await _postgreDbContext.SaveChangesAsync();

                return oldPassword != getUser.UserPassword;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogCritical();
                throw;
            }
        }

        /// <summary>
        /// Метод запишет код подтвержедния в БД.
        /// </summary>
        /// <param name="number">Данные.</param>
        /// <param name="code">Код подтверждения.</param>
        private async Task SaveCodeAsync(string data, string code)
        {
            try
            {
                UserEntity findNumber = null;
                var isEmail = data.Contains("@");

                if (!isEmail)
                {
                    // Если найдет такой номер телеофна в БД, то перезапишет код.
                    findNumber = await (from u in _postgreDbContext.Users
                                        where u.PhoneNumber.Equals(data)
                                        select u)
                        .FirstOrDefaultAsync();
                }

                else
                {
                    // Если найдет такой username в БД, то перезапишет код.
                    findNumber = await (from u in _postgreDbContext.Users
                                        where u.UserName.Equals(data)
                                        select u)
                        .FirstOrDefaultAsync();
                }

                // Если такой номер был, то обновит код пользователю.
                if (findNumber != null)
                {
                    findNumber.Code = code;
                    await _postgreDbContext.SaveChangesAsync();

                    return;
                }

                var insertData = new UserEntity
                {
                    Code = code,
                    DateRegister = DateTime.Now,
                    RememberMe = false,
                    EmailConfirmed = false,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0
                };

                if (isEmail)
                {
                    insertData.UserName = data;
                    insertData.Email = data;
                }

                else
                {
                    insertData.PhoneNumber = data;
                }

                // Такого номера не было, добавит пользователя.
                await _postgreDbContext.Users.AddAsync(insertData);
                await _postgreDbContext.SaveChangesAsync();

                await SaveUserBaseAsync(data);
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogCritical();
                throw;
            }
        }

        /// <summary>
        /// Метод хэширует пароль аналогично как это делает Identity.
        /// </summary>
        /// <param name="password">Исходный пароль без хэша.</param>
        /// <returns>Хэш пароля.</returns>
        public async Task<string> HashPasswordAsync(string password)
        {
            byte[] salt;
            byte[] buffer2;

            using (var bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }

            var dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);

            return await Task.FromResult(Convert.ToBase64String(dst));
        }

        /// <summary>
        /// Метод вычислит кол-во месяцев между датами.
        /// </summary>
        /// <param name="startDate">Дата начала.</param>
        /// <param name="endDate">Текущая дата.</param>
        /// <returns>Кол-во месяцев округленное до целого.</returns>
        public async Task<double> GetSubtractMonthAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var dt1 = new DateTime(startDate.Year, startDate.Month, startDate.Day);
                var dt2 = new DateTime(endDate.Year, endDate.Month, endDate.Day);

                if (dt1 > dt2 || dt1 == dt2)
                {
                    return 0;
                }

                double days = (dt2 - dt1).TotalDays;
                double mnt = 0;

                while (days != 0)
                {
                    var inMnt = DateTime.DaysInMonth(dt1.Year, dt1.Month);

                    if (days >= inMnt)
                    {
                        days -= inMnt;
                        ++mnt;
                        dt1 = dt1.AddMonths(1);
                    }

                    else
                    {
                        mnt += days / inMnt;
                        days = 0;
                    }
                }

                return await Task.FromResult(Math.Round(mnt));
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogError();
                throw;
            }
        }

        /// <summary>
        /// Метод соединит в строку из массива строк разделяя запятой.
        /// </summary>
        /// <param name="arr">Исходный массив строк.</param>
        /// <returns>Строка со значениями массива.</returns>
        public async Task<string> JoinArrayWithDelimeterAsync(string[] arr)
        {
            try
            {
                var builder = new StringBuilder();
                var delimiter = string.Empty;
                var urls = string.Empty;

                if (arr == null)
                {
                    return string.Empty;
                }

                if (arr.Any())
                {
                    foreach (var url in arr)
                    {
                        builder.Append(delimiter);
                        builder.Append(url);
                        delimiter = ",";
                    }

                    urls = builder.ToString();
                }

                return await Task.FromResult(urls);
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogError();
                throw;
            }
        }

        /// <summary>
        /// Метод преобразует рубли в копейки.
        /// </summary>
        /// <param name="amount">Сумма в руб.</param>
        /// <returns>Сумма в копейках.</returns>
        public async Task<double> ConvertRubToPennyAsync(double amount)
        {
            try
            {
                if (amount <= 0)
                {
                    throw new EmptySumException("Сумма была меньше или равна 0.");
                }

                return await Task.FromResult(amount * 100);
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogCritical();
                throw;
            }
        }

        /// <summary>
        /// Метод хэширует строку по SHA-256.
        /// </summary>
        /// <param name="str">Исходная строка с параметрами, которые конкантенированы.</param>
        /// <returns>Измененная строку по SHA-256.</returns>
        public async Task<string> HashSha256Async(Dictionary<string, object> hashValues)
        {
            try
            {
                var sortedDict = new SortedDictionary<string, object>(hashValues);
                var sb = new StringBuilder();

                foreach (var item in sortedDict)
                {
                    sb.Append(item.Value);
                }

                using var sha256Hash = SHA256.Create();
                var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(sb.ToString()));
                var builder = new StringBuilder();

                foreach (var t in bytes)
                {
                    builder.Append(t.ToString("x2"));
                }

                return builder.ToString();
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogCritical();
                throw;
            }
        }

        /// <summary>
        /// Метод вернёт корректное склонение для слова "день" в зависимости от количества дней.
        /// </summary>
        /// <param name="days">Количество дней.</param>
        /// <returns>Корректное склонение слова "день".</returns>
        public async Task<string> GetCorrectDayDeclinationAsync(int days)
        {
            try
            {
                if (days < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(days), $"Количество дней {nameof(days)} не может быть отрицательным");
                }

                if (days % 100 >= 10 && days % 100 <= 20)
                {
                    return await Task.FromResult("дней");
                }

                if (days % 10 == 1)
                {
                    return await Task.FromResult("день");

                }
                if (days % 10 >= 2 && days % 10 <= 4)
                {
                    return await Task.FromResult("дня");

                }
                else
                {
                    return await Task.FromResult("дней");
                }

            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogCritical();
                throw;
            }
        }

        /// <summary>
        /// Метод добавит пользователя в таблицу Identity.
        /// </summary>
        /// <param name="data"></param>
        private async Task SaveUserBaseAsync(string data)
        {
            var user = await _postgreDbContext.Users.FirstOrDefaultAsync(u => u.Email.Equals(data) && u.UserName.Equals(data));

            var baseUser = new BaseUserEntity
            {
                UserName = user.UserName,
                Email = user.Email,
                DateRegister = user.DateRegister,
                NormalizedUserName = user.UserName.ToUpper(),
                NormalizedEmail = user.Email.ToUpper(),
                SecurityStamp = user.SecurityStamp,
                Code = user.Code
            };

            await _postgreDbContext.BaseUsers.AddAsync(baseUser);
            await _postgreDbContext.SaveChangesAsync();
        }
    }
}
