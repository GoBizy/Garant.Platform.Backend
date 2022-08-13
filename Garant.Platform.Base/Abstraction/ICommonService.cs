using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Garant.Platform.Models.Mailing.Output;

namespace Garant.Platform.Base.Abstraction
{
    /// <summary>
    /// Абстракция общего сервиса.
    /// </summary>
    public interface ICommonService
    {
        /// <summary>
        /// Метод создает код. Кол-во цифр зависит от переданного типа.
        /// </summary>
        /// <param name="data">Телефон или почта.</param>
        /// <returns>Флаг успеха.</returns>
        Task<MailngOutput> GenerateAcceptCodeAsync(string data);

        /// <summary>
        /// Метод хэширует пароль аналогично как это делает Identity.
        /// </summary>
        /// <param name="password">Исходный пароль без хэша.</param>
        /// <returns>Хэш пароля.</returns>
        Task<string> HashPasswordAsync(string password);

        /// <summary>
        /// Метод вычислит кол-во месяцев между датами.
        /// </summary>
        /// <param name="start">Дата начала.</param>
        /// <param name="end">Текущая дата.</param>
        /// <returns>Кол-во месяцев округленное до целого.</returns>
        Task<double> GetSubtractMonthAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Метод соединит в строку из массива строк разделяя запятой.
        /// </summary>
        /// <param name="arr">Исходный массив строк.</param>
        /// <returns>Строка со значениями массива.</returns>
        Task<string> JoinArrayWithDelimeterAsync(string[] arr);

        /// <summary>
        /// Метод преобразует рубли в копейки.
        /// </summary>
        /// <param name="amount">Сумма в руб.</param>
        /// <returns>Сумма в копейках.</returns>
        Task<double> ConvertRubToPennyAsync(double amount);

        /// <summary>
        /// Метод вернёт корректное склонение для слова "день" в зависимости от количества дней.
        /// </summary>
        /// <param name="days">Количество дней.</param>
        /// <returns>Корректное склонение слова "день".</returns>
        Task<string> GetCorrectDayDeclinationAsync(int days);

        /// <summary>
        /// Метод хэширует строку по SHA-256.
        /// </summary>
        /// <param name="str">Исходная строка с параметрами, которые конкантенированы.</param>
        /// <returns>Измененная строку по SHA-256.</returns>
        Task<string> HashSha256Async(Dictionary<string, object> hashValues);

        /// <summary>
        /// Метод создаст код сброса пароля, запишет его в базу данных и отправит пользователю.
        /// </summary>
        /// <param name="email"> Почта. </param>
        /// <returns> Флаг успеха. </returns>
        Task<MailngOutput> GenerateResetCodeAsync(string email);

        /// <summary>
        /// Метод проверит соответствие полученного кода с кодом в базе данных и почтой.
        /// </summary>
        /// <param name="email"> Почта. </param>
        /// <param name="code"> Полученный код. </param>
        /// <returns> Флаг успеха. </returns>
        Task<bool> CheckCodeAsync(string email, string code);

        /// <summary>
        /// Метод изменит пароль пользователя.
        /// </summary>
        /// <param name="email"> Почта. </param>
        /// <param name="password"> Новый пароль. </param>
        /// <returns> Флаг успеха. </returns>
        Task<bool> ChangePasswordAsync(string email, string password);
    }
}
