using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.Business;
using Garant.Platform.Abstractions.DataBase;
using Garant.Platform.Abstractions.User;
using Garant.Platform.Base.Abstraction;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Exceptions;
using Garant.Platform.Core.Logger;
using Garant.Platform.Core.Utils;
using Garant.Platform.Models.Business.Input;
using Garant.Platform.Models.Business.Output;
using Garant.Platform.Models.Configurator.Output;
using Garant.Platform.Models.Entities.Business;
using Garant.Platform.Models.Request.Output;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Garant.Platform.Services.Service.Business
{
    /// <summary>
    /// Репозиторий готового бизнеса.
    /// </summary>
    public sealed class BusinessRepository : IBusinessRepository
    {
        private readonly PostgreDbContext _postgreDbContext;
        private readonly IUserRepository _userRepository;
        private readonly ICommonService _commonService;

        public BusinessRepository(IUserRepository userRepository, ICommonService commonService)
        {
            var dbContext = AutoFac.Resolve<IDataBaseConfig>();
            _postgreDbContext = dbContext.GetDbContext();
            _userRepository = userRepository;
            _commonService = commonService;
        }

        /// <summary>
        /// Метод создаст новую или обновит существующий бизнес.
        /// </summary>
        /// <param name="files">Входные файлы.</param>
        /// <param name="businessInput">Входная модель.</param>
        /// <param name="urlsBusiness">Пути к доп.изображениям.</param>
        /// <param name="account">Логин.</param>
        /// <returns>Данные франшизы.</returns>
        public async Task<CreateUpdateBusinessOutput> CreateUpdateBusinessAsync(CreateUpdateBusinessInput businessInput, string[] urlsBusiness, IFormFileCollection files, string account)
        {
            try
            {
                CreateUpdateBusinessOutput result = null;

                if (businessInput != null)
                {
                    var userId = string.Empty;

                    var findUser = await _userRepository.FindUserByEmailOrPhoneNumberAsync(account);

                    // Если такого пользователя не найдено, значит поищет по коду.
                    if (findUser == null)
                    {
                        var findUserIdByCode = await _userRepository.FindUserByCodeAsync(account);

                        if (!string.IsNullOrEmpty(findUserIdByCode))
                        {
                            userId = findUserIdByCode;
                        }
                    }

                    else
                    {
                        userId = findUser.UserId;
                    }

                    // Найдет бизнес с таким названием.
                    var findBusiness = await GetBusinessAsync(businessInput.BusinessName);
                    var urls = await _commonService.JoinArrayWithDelimeterAsync(urlsBusiness);
                    var activityPhotoName = string.Empty;
                    var assetsPhotoName = string.Empty;
                    var nameFinModelFile = string.Empty;
                    var reasonsSalePhotoName = string.Empty;

                    if (files.Any(c => c.Name.Equals("filesTextBusiness")))
                    {
                        activityPhotoName = files.Where(c => c.Name.Equals("filesTextBusiness")).ToArray()[0].FileName;
                    }

                    if (files.Any(c => c.Name.Equals("filesAssets")))
                    {
                        assetsPhotoName = files.Where(c => c.Name.Equals("filesAssets")).ToArray()[0].FileName;
                    }
                    
                    if (files.Any(c => c.Name.Equals("finModelFile")))
                    {
                        nameFinModelFile = files.Where(c => c.Name.Equals("finModelFile")).ToArray()[0].FileName;
                    }
                    
                    if (files.Any(c => c.Name.Equals("filesReasonsSale")))
                    {
                        reasonsSalePhotoName = files.Where(c => c.Name.Equals("filesReasonsSale")).ToArray()[0].FileName;
                    }

                    // Создаст новый бизнес.
                    if (businessInput.IsNew && findBusiness == null)
                    {
                        await _postgreDbContext.Businesses.AddAsync(new BusinessEntity
                        {
                            ActivityDetail = businessInput.ActivityDetail,
                            ActivityPhotoName = activityPhotoName,
                            Address = businessInput.Address,
                            Assets = businessInput.Assets,
                            AssetsPhotoName = assetsPhotoName,
                            BusinessAge = businessInput.BusinessAge,
                            BusinessName = businessInput.BusinessName,
                            EmployeeCountYear = businessInput.EmployeeCountYear,
                            Form = businessInput.Form,
                            Status = businessInput.Status,
                            Price = businessInput.Price,
                            UrlsBusiness = urls,
                            TurnPrice = businessInput.TurnPrice,
                            ProfitPrice = businessInput.ProfitPrice,
                            Payback = businessInput.Payback,
                            Profitability = businessInput.Profitability,
                            InvestPrice = businessInput.InvestPrice,
                            Text = businessInput.Text,
                            Share = businessInput.Share,
                            Site = businessInput.Site,
                            Peculiarity = businessInput.Peculiarity,
                            NameFinModelFile = nameFinModelFile,
                            ReasonsSale = businessInput.ReasonsSale,
                            ReasonsSalePhotoName = reasonsSalePhotoName,
                            UrlVideo = businessInput.UrlVideo,
                            IsGarant = businessInput.IsGarant,
                            UserId = userId,
                            DateCreate = DateTime.Now,
                            TextDoPrice = "Стоимость:",
                            Category = businessInput.Category,
                            SubCategory = businessInput.SubCategory,
                            BusinessCity = businessInput.BusinessCity
                        });
                    }

                    // Обновит бизнес.
                    else if (!businessInput.IsNew && findBusiness != null)
                    {
                        findBusiness.ActivityDetail = businessInput.ActivityDetail;
                        findBusiness.ActivityPhotoName = activityPhotoName;
                            findBusiness.Address = businessInput.Address;
                        findBusiness.Assets = businessInput.Assets;
                        findBusiness.AssetsPhotoName = assetsPhotoName;
                            findBusiness.BusinessAge = businessInput.BusinessAge;
                        findBusiness.BusinessName = businessInput.BusinessName;
                        findBusiness.EmployeeCountYear = businessInput.EmployeeCountYear;
                        findBusiness.Form = businessInput.Form;
                        findBusiness.Status = businessInput.Status;
                        findBusiness.Price = businessInput.Price;
                        findBusiness.UrlsBusiness = urls;
                        findBusiness.TurnPrice = businessInput.TurnPrice;
                        findBusiness.ProfitPrice = businessInput.ProfitPrice;
                        findBusiness.Payback = businessInput.Payback;
                        findBusiness.Profitability = businessInput.Profitability;
                        findBusiness.InvestPrice = businessInput.InvestPrice;
                        findBusiness.Text = businessInput.Text;
                        findBusiness.Share = businessInput.Share;
                        findBusiness.Site = businessInput.Site;
                        findBusiness.Peculiarity = businessInput.Peculiarity;
                        findBusiness.NameFinModelFile = nameFinModelFile;
                        findBusiness.ReasonsSalePhotoName = reasonsSalePhotoName;
                        findBusiness.UrlVideo = businessInput.UrlVideo;
                        findBusiness.IsGarant = businessInput.IsGarant;
                        findBusiness.DateCreate = DateTime.Now;
                        findBusiness.TextDoPrice = "Стоимость:";
                        findBusiness.Category = businessInput.Category;
                        findBusiness.SubCategory = businessInput.SubCategory;
                        findBusiness.BusinessCity = businessInput.BusinessCity;

                        _postgreDbContext.Update(findBusiness);
                    }

                    await _postgreDbContext.SaveChangesAsync();

                    var lastBusinessId = await _postgreDbContext.Businesses.MaxAsync(b => b.BusinessId);

                    result = new CreateUpdateBusinessOutput
                    {
                        ActivityDetail = businessInput.ActivityDetail,
                        ActivityPhotoName = string.IsNullOrEmpty(activityPhotoName) ? string.Empty : "../../../assets/images/" + files.Where(c => c.Name.Equals("filesTextBusiness")).ToArray()[0].FileName,
                        Address = businessInput.Address,
                        Assets = businessInput.Assets,
                        AssetsPhotoName = string.IsNullOrEmpty(assetsPhotoName) ? string.Empty : "../../../assets/images/" + files.Where(c => c.Name.Equals("filesAssets")).ToArray()[0].FileName,
                        BusinessAge = businessInput.BusinessAge,
                        BusinessId = lastBusinessId,
                        BusinessName = businessInput.BusinessName,
                        EmployeeCountYear = businessInput.EmployeeCountYear,
                        Form = businessInput.Form,
                        Status = businessInput.Status,
                        Price = businessInput.Price,
                        UrlsBusiness = urls,
                        TurnPrice = businessInput.TurnPrice,
                        ProfitPrice = businessInput.ProfitPrice,
                        Payback = businessInput.Payback,
                        Profitability = businessInput.Profitability,
                        InvestPrice = businessInput.InvestPrice,
                        Text = businessInput.Text,
                        Share = businessInput.Share,
                        Site = businessInput.Site,
                        Peculiarity = businessInput.Peculiarity,
                        NameFinModelFile = string.IsNullOrEmpty(nameFinModelFile) ? string.Empty : "../../../assets/images/" + files.Where(c => c.Name.Equals("finModelFile")).ToArray()[0].FileName,
                        ReasonsSale = businessInput.ReasonsSale,
                        ReasonsSalePhotoName = string.IsNullOrEmpty(reasonsSalePhotoName) ? string.Empty : "../../../assets/images/" + files.Where(c => c.Name.Equals("filesReasonsSale")).ToArray()[0].FileName,
                        UrlVideo = businessInput.UrlVideo,
                        IsGarant = businessInput.IsGarant,
                        DateCreate = DateTime.Now,
                        TextDoPrice = "Стоимость:",
                        BusinessCity = businessInput.BusinessCity
                    };
                }

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
        /// Метод найдет карточку готового бизнеса.
        /// </summary>
        /// <param name="title">Название карточки готового бизнеса.</param>
        /// <returns>Данные карточки готового бизнеса.</returns>
        public async Task<BusinessEntity> GetBusinessAsync(string title)
        {
            try
            {
                var result = await _postgreDbContext.Businesses
                    .Where(b => b.BusinessName.Equals(title))
                    .Select(b => new BusinessEntity
                    {
                        ActivityDetail = b.ActivityDetail,
                        ActivityPhotoName = b.ActivityPhotoName,
                        Address = b.Address,
                        Assets = b.Assets,
                        AssetsPhotoName = b.AssetsPhotoName,
                        BusinessAge = b.BusinessAge,
                        BusinessId = b.BusinessId,
                        BusinessName = b.BusinessName,
                        EmployeeCountYear = b.EmployeeCountYear,
                        Form = b.Form,
                        Status = b.Status,
                        Price = b.Price,
                        UrlsBusiness = b.UrlsBusiness,
                        TurnPrice = b.TurnPrice,
                        ProfitPrice = b.ProfitPrice,
                        Payback = b.Payback,
                        Profitability = b.Profitability,
                        InvestPrice = b.InvestPrice,
                        Text = b.Text,
                        Share = b.Share,
                        Site = b.Site,
                        Peculiarity = b.Peculiarity,
                        NameFinModelFile = b.NameFinModelFile,
                        ReasonsSale = b.ReasonsSale,
                        ReasonsSalePhotoName = b.ReasonsSalePhotoName,
                        UrlVideo = b.UrlVideo,
                        IsGarant = b.IsGarant,
                        UserId = b.UserId,
                        DateCreate = b.DateCreate,
                        Category = b.Category,
                        SubCategory = b.SubCategory
                    })
                    .FirstOrDefaultAsync();

                return result;
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
        /// Метод получит бизнес по Id пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Данные бизнеса.</returns>
        public async Task<BusinessEntity> GetBusinessByUserIdAsync(string userId)
        {
            try
            {
                var result = await _postgreDbContext.Businesses
                    .Where(b => b.UserId.Equals(userId))
                    .Select(b => new BusinessEntity
                    {
                        ActivityDetail = b.ActivityDetail,
                        ActivityPhotoName = b.ActivityPhotoName,
                        Address = b.Address,
                        Assets = b.Assets,
                        AssetsPhotoName = b.AssetsPhotoName,
                        BusinessAge = b.BusinessAge,
                        BusinessId = b.BusinessId,
                        BusinessName = b.BusinessName,
                        EmployeeCountYear = b.EmployeeCountYear,
                        Form = b.Form,
                        Status = b.Status,
                        Price = b.Price,
                        UrlsBusiness = b.UrlsBusiness,
                        TurnPrice = b.TurnPrice,
                        ProfitPrice = b.ProfitPrice,
                        Payback = b.Payback,
                        Profitability = b.Profitability,
                        InvestPrice = b.InvestPrice,
                        Text = b.Text,
                        Share = b.Share,
                        Site = b.Site,
                        Peculiarity = b.Peculiarity,
                        NameFinModelFile = b.NameFinModelFile,
                        ReasonsSale = b.ReasonsSale,
                        ReasonsSalePhotoName = b.ReasonsSalePhotoName,
                        UrlVideo = b.UrlVideo,
                        IsGarant = b.IsGarant,
                        UserId = b.UserId,
                        DateCreate = b.DateCreate,
                        Category = b.Category,
                        SubCategory = b.SubCategory
                    })
                    .FirstOrDefaultAsync();

                return result;
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
        /// Метод отправит файл в папку и временно запишет в БД.
        /// </summary>
        /// <param name="files">Файлы.</param>
        /// <returns>Список названий файлов.</returns>
        public async Task<IEnumerable<string>> AddTempFilesBeforeCreateBusinessAsync(IFormFileCollection files, string account)
        {
            try
            {
                var results = new List<string>();
                var userId = string.Empty;

                // Найдет такого пользователя.
                var findUser = await _userRepository.FindUserByEmailOrPhoneNumberAsync(account);

                // Если такого пользователя не найдено, значит поищет по коду.
                if (findUser == null)
                {
                    var findUserIdByCode = await _userRepository.FindUserByCodeAsync(account);

                    if (!string.IsNullOrEmpty(findUserIdByCode))
                    {
                        userId = findUserIdByCode;
                    }
                }

                else
                {
                    userId = findUser.UserId;
                }

                if (!string.IsNullOrEmpty(userId))
                {
                    // Запишет во временную таблицу какие названия файлов, которые добавили но еще не сохранили.
                    foreach (var item in files)
                    {
                        await _postgreDbContext.TempBusinesses.AddAsync(new TempBusinessEntity
                        {
                            FileName = item.FileName,
                            Id = userId
                        });

                        results.Add("../../../assets/images/" + item.FileName);
                    }

                    await _postgreDbContext.SaveChangesAsync();
                }

                return results;
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
        /// Метод получит бизнес для просмотра или изменения.
        /// </summary>
        /// <param name="businessId">Id бизнеса.</param>
        /// <param name="mode">Режим (Edit или View).</param>
        /// <returns>Данные бизнеса.</returns>
        public async Task<BusinessOutput> GetBusinessAsync(long businessId, string mode)
        {
            try
            {
                var result = await (from b in _postgreDbContext.Businesses
                                    where b.BusinessId == businessId
                                    select new BusinessOutput
                                    {
                                        ActivityDetail = b.ActivityDetail,
                                        ActivityPhotoName = b.ActivityPhotoName,
                                        Address = b.Address,
                                        Assets = b.Assets,
                                        AssetsPhotoName = b.AssetsPhotoName,
                                        BusinessAge = b.BusinessAge,
                                        BusinessId = b.BusinessId,
                                        BusinessName = b.BusinessName,
                                        EmployeeCountYear = b.EmployeeCountYear,
                                        Form = b.Form,
                                        Status = b.Status,
                                        Price = string.Format("{0:0,0}", b.Price),
                                        UrlsBusiness = b.UrlsBusiness,
                                        Url = b.UrlsBusiness,
                                        TurnPrice = b.TurnPrice,
                                        ProfitPrice = b.ProfitPrice,
                                        Payback = b.Payback,
                                        Profitability = b.Profitability,
                                        InvestPrice = b.InvestPrice,
                                        InvestPriceOutput = JsonConvert.DeserializeObject<List<ConvertInvestPriceIncludeOutput>>(b.InvestPrice),
                                        Text = b.Text,
                                        Share = b.Share,
                                        Site = b.Site,
                                        Peculiarity = b.Peculiarity,
                                        NameFinModelFile = b.NameFinModelFile,
                                        ReasonsSale = b.ReasonsSale,
                                        ReasonsSalePhotoName = b.ReasonsSalePhotoName,
                                        UrlVideo = b.UrlVideo,
                                        IsGarant = b.IsGarant,
                                        UserId = b.UserId,
                                        DateCreate = b.DateCreate,
                                        Category = b.Category,
                                        SubCategory = b.SubCategory
                                    })
                    .FirstOrDefaultAsync();

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
        /// Метод получит список категорий бизнеса.
        /// </summary>
        /// <returns>Список категорий.</returns>
        public async Task<IEnumerable<GetBusinessCategoryOutput>> GetBusinessCategoriesAsync()
        {
            try
            {
                var result = await _postgreDbContext.BusinessCategories
                    .Select(fc => new GetBusinessCategoryOutput
                    {
                        CategoryCode = fc.BusinessCategoryCode,
                        CategoryName = fc.BusinessCategoryName
                    })
                    .ToListAsync();

                return result;
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
        /// Метод получит список подкатегорий бизнеса.
        /// </summary>
        /// <returns>Список подкатегорий.</returns>
        public async Task<IEnumerable<BusinessSubCategoryOutput>> GetSubBusinessCategoryListAsync()
        {
            try
            {
                var result = await _postgreDbContext.BusinessSubCategories
                    .Select(fc => new BusinessSubCategoryOutput
                    {
                        SubCategoryCode = fc.BusinessSubCategoryCode,
                        SubCategoryName = fc.BusinessSubCategoryName
                    })
                    .ToListAsync();

                return result;
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
        /// Метод получит список городов.
        /// </summary>
        /// <returns>Список городов.</returns>
        public async Task<IEnumerable<BusinessCitiesOutput>> GetCitiesListAsync()
        {
            try
            {
                var result = await _postgreDbContext.BusinessCities
                    .Select(fc => new BusinessCitiesOutput
                    {
                        BusinessCityCode = fc.BusinessCityCode,
                        BusinessCityName = fc.BusinessCityName
                    })
                    .ToListAsync();

                return result;
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
        /// Метод получит заголовок бизнеса по Id пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Заголовок бизнеса.</returns>
        public async Task<string> GetBusinessTitleAsync(string userId)
        {
            try
            {
                var title = await _postgreDbContext.Businesses
                    .Where(f => f.UserId.Equals(userId))
                    .Select(f => f.BusinessName)
                    .FirstOrDefaultAsync();

                return title;
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
        /// Метод получит список популярного бизнеса.
        /// </summary>
        /// <returns>Список бизнеса.</returns>
        public async Task<IEnumerable<PopularBusinessOutput>> GetPopularBusinessAsync()
        {
            try
            {
                var result = await _postgreDbContext.Businesses
                    .Select(b => new PopularBusinessOutput
                    {
                        DateCreate = b.DateCreate,
                        Price = string.Format("{0:0,0}", b.Price),
                        CountDays = DateTime.Now.Subtract(b.DateCreate).Days,
                        DayDeclination = "дня",
                        Text = b.Text,
                        TextDoPrice = b.TextDoPrice,
                        Title = b.BusinessName,
                        Url = b.UrlsBusiness,
                        TotalInvest = string.Format("{0:0,0}", b.InvestPrice),
                        BusinessId = b.BusinessId
                    })
                    .Take(4)
                    .ToListAsync();

                foreach (var item in result)
                {
                    item.DayDeclination = await _commonService.GetCorrectDayDeclinationAsync(item.CountDays);
                }

                return result;
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
        /// Метод получит список бизнеса.
        /// </summary>
        /// <returns>Список бизнеса.</returns>
        public async Task<IEnumerable<BusinessOutput>> GetBusinessListAsync()
        {
            try
            {
                var result = await _postgreDbContext.Businesses
                    .Where(b => b.IsAccepted == true)
                    .Select(b => new BusinessOutput
                    {
                        DateCreate = b.DateCreate,
                        Price = string.Format("{0:0,0}", b.Price),
                        CountDays = DateTime.Now.Subtract(b.DateCreate).Days,
                        DayDeclination = "дня",
                        Text = b.Text,
                        TextDoPrice = b.TextDoPrice,
                        BusinessName = b.BusinessName,
                        Url = b.UrlsBusiness,
                        TotalInvest = string.Format("{0:0,0}", b.InvestPrice),
                        BusinessId = b.BusinessId
                    })
                    .ToListAsync();

                foreach (var item in result)
                {
                    item.DayDeclination = await _commonService.GetCorrectDayDeclinationAsync(item.CountDays);
                }

                return result;
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
        /// Метод фильтрует список бизнесов по параметрам.
        /// </summary>
        /// <param name="typeSortPrice">Тип сортировки цены (убыванию, возрастанию).</param>
        /// <param name="profitMinPrice">Цена от.</param>
        /// <param name="profitMaxPrice">Цена до.</param>
        /// <param name="categoryCode">Город.</param>
        /// <param name="viewCode">Код вида бизнеса.</param>
        /// <param name="minPriceInvest">Сумма общих инвестиций от.</param>
        /// <param name="maxPriceInvest">Сумма общих инвестиций до.</param>
        /// <param name="isGarant">Флаг гаранта.</param>
        /// <returns>Список бизнесов после фильтрации.</returns>
        public async Task<List<BusinessOutput>> FilterBusinessesAsync(string typeSortPrice, double profitMinPrice, double profitMaxPrice, string viewCode, string categoryCode, double minPriceInvest, double maxPriceInvest, bool isGarant = false)
        {
            try
            {
                List<BusinessOutput> items = null;
                IQueryable<BusinessOutput> query = null;

                //TODO: какая-то путаница с параметрами, сортировка по Id происходит
                // Сортировать на возрастанию цены.
                if (typeSortPrice.Equals("Asc"))
                {
                    query = (from f in _postgreDbContext.Businesses
                             where f.Category.Equals(categoryCode)
                                   && (f.Price <= profitMaxPrice && f.Price >= profitMinPrice)
                                   && (f.ProfitPrice >= minPriceInvest && f.ProfitPrice <= maxPriceInvest)
                                   && f.IsGarant == isGarant
                             orderby f.BusinessId
                             select new BusinessOutput
                             {
                                 DateCreate = f.DateCreate,
                                 Price = string.Format("{0:0,0}", f.Price),
                                 CountDays = DateTime.Now.Subtract(f.DateCreate).Days,
                                 DayDeclination = "дня",
                                 Text = f.Text,
                                 TextDoPrice = f.TextDoPrice,
                                 BusinessName = f.BusinessName,
                                 Url = f.UrlsBusiness,
                                 IsGarant = f.IsGarant,
                                 ProfitPrice = f.ProfitPrice,
                                 TotalInvest = string.Format("{0:0,0}", f.ProfitPrice),
                                 BusinessId = f.BusinessId
                             })
                        .AsQueryable();
                }

                // Сортировать на убыванию цены.
                else if (typeSortPrice.Equals("Desc"))
                {
                    query = (from f in _postgreDbContext.Businesses
                             where f.Category.Equals(categoryCode)
                                   && (f.Price <= profitMaxPrice && f.Price >= profitMinPrice)
                                   && (f.ProfitPrice >= minPriceInvest && f.ProfitPrice <= maxPriceInvest)
                                   && f.IsGarant == isGarant
                             orderby f.BusinessId descending
                             select new BusinessOutput
                             {
                                 DateCreate = f.DateCreate,
                                 Price = string.Format("{0:0,0}", f.Price),
                                 CountDays = DateTime.Now.Subtract(f.DateCreate).Days,
                                 DayDeclination = "дня",
                                 Text = f.Text,
                                 TextDoPrice = f.TextDoPrice,
                                 BusinessName = f.BusinessName,
                                 Url = f.UrlsBusiness,
                                 IsGarant = f.IsGarant,
                                 ProfitPrice = f.ProfitPrice,
                                 TotalInvest = string.Format("{0:0,0}", f.ProfitPrice),
                                 BusinessId = f.BusinessId
                             })
                        .AsQueryable();
                }

                if (query != null)
                {
                    // Нужно ли дополнить запрос для сортировки по прибыли.
                    if (profitMinPrice > 0 && profitMaxPrice > 0)
                    {
                        query = query.Where(c => c.ProfitPrice <= Convert.ToDouble(profitMaxPrice)
                                                 && c.ProfitPrice >= Convert.ToDouble(profitMinPrice));
                    }

                    items = await query.ToListAsync();

                    foreach (var item in items)
                    {
                        item.DayDeclination = await _commonService.GetCorrectDayDeclinationAsync(item.CountDays);
                    }
                }

                return items;
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
        /// Метод получит новый бизнес, который был создан в текущем месяце.
        /// </summary>
        /// <returns>Список бизнеса.</returns>
        public async Task<List<BusinessOutput>> GetNewBusinesseListAsync()
        {
            try
            {
                var month = DateTime.Now.Month;

                var items = await (from f in _postgreDbContext.Businesses
                                   where f.DateCreate.Month == month
                                   select new BusinessOutput
                                   {
                                       DateCreate = f.DateCreate,
                                       Price = string.Format("{0:0,0}", f.Price),
                                       CountDays = DateTime.Now.Subtract(f.DateCreate).Days,
                                       DayDeclination = "дня",
                                       Text = f.Text,
                                       TextDoPrice = f.TextDoPrice,
                                       BusinessName = f.BusinessName,
                                       Url = f.UrlsBusiness,
                                       IsGarant = f.IsGarant,
                                       ProfitPrice = f.ProfitPrice,
                                       TotalInvest = string.Format("{0:0,0}", f.Price),
                                       BusinessId = f.BusinessId
                                   })
                    .Take(4)
                    .ToListAsync();

                foreach (var item in items)
                {
                    item.DayDeclination = await _commonService.GetCorrectDayDeclinationAsync(item.CountDays);
                }

                return items;
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
        /// Метод найдет среди бизнеса по запросу.
        /// </summary>
        /// <param name="searchText">Текст поиска.</param>
        /// <returns>Список с результатами.</returns>
        public async Task<IEnumerable<BusinessOutput>> SearchByBusinessesAsync(string searchText)
        {
            try
            {
                var result = await _postgreDbContext.Businesses
                    .Where(f => f.BusinessName.ToLower().Contains(searchText.ToLower())
                                || f.ActivityDetail.ToLower().Contains(searchText.ToLower()))
                    .Select(f => new BusinessOutput
                    {
                        DateCreate = f.DateCreate,
                        Price = string.Format("{0:0,0}", f.Price),
                        CountDays = DateTime.Now.Subtract(f.DateCreate).Days,
                        DayDeclination = "дня",
                        Text = f.Text,
                        TextDoPrice = f.TextDoPrice,
                        BusinessName = f.BusinessName,
                        Url = f.UrlsBusiness,
                        IsGarant = f.IsGarant,
                        ProfitPrice = f.ProfitPrice,
                        TotalInvest = string.Format("{0:0,0}", f.Price),
                        BusinessId = f.BusinessId
                    })
                    .ToListAsync();

                foreach (var item in result)
                {
                    item.DayDeclination = await _commonService.GetCorrectDayDeclinationAsync(item.CountDays);
                }

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
        /// Метод создаст заявку бизнеса.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        /// <param name="phone">Телефон.</param>
        /// <param name="account">Аккаунт пользователя.</param>
        /// <param name="businessId">Id бизнеса, по которому оставлена заявка.</param>
        /// <returns>Данные заявки.</returns>
        public async Task<RequestBusinessOutput> CreateRequestBusinessAsync(string userName, string phone, string account, long businessId)
        {
            try
            {
                var userId = await _userRepository.FindUserIdUniverseAsync(account);
                var now = DateTime.Now;

                var addRequestData = new RequestBusinessEntity
                {
                    UserId = userId,
                    DateCreate = now,
                    Phone = phone,
                    UserName = userName,
                    BusinessId = businessId
                };

                await _postgreDbContext.RequestsBusinesses.AddAsync(addRequestData);
                await _postgreDbContext.SaveChangesAsync();

                var result = new RequestBusinessOutput
                {
                    UserId = userId,
                    DateCreate = now,
                    Phone = phone,
                    UserName = userName,
                    BusinessId = businessId
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
        /// Метод получит список заявок для вкладки профиля "Уведомления".
        /// <param name="account">Аккаунт.</param>
        /// </summary>
        /// <returns>Список заявок.</returns>
        public async Task<IEnumerable<RequestBusinessEntity>> GetBusinessRequestsAsync(string account)
        {
            try
            {
                var userId = await _userRepository.FindUserIdUniverseAsync(account);

                if (string.IsNullOrEmpty(userId))
                {
                    throw new NotFoundUserIdException(account);
                }

                // Получит список заявок пользовтеля.
                var result = await _postgreDbContext.RequestsBusinesses.Where(r => r.UserId.Equals(userId)).ToListAsync();

                return result;
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
        /// Метод применяет фильтры независимо друг от друга.
        /// </summary>
        /// <param name="typeSortPrice">Тип сортировки цены (убыванию, возрастанию).</param>
        /// <param name="minPrice">Цена от.</param>
        /// <param name="maxPrice">Цена до.</param>
        /// <param name="city">Город.</param>
        /// <param name="categoryCode">Код категории.</param>        
        /// <param name="minProfit">Прибыль в месяц от.</param>
        /// <param name="maxProfit">Прибыль в месяц до.</param>
        /// <param name="isGarant">Флаг гаранта.</param>
        /// <returns>Список бизнесов после фильтрации.</returns>
        public async Task<List<BusinessOutput>> FilterBusinessesIndependentlyAsync(string typeSortPrice, double minPrice, double maxPrice,
                                                                                   string city, string categoryCode, double minProfit,
                                                                                   double maxProfit, bool isGarant = true)
        {
            try
            {
                List<BusinessOutput> items = null;
                IQueryable<BusinessEntity> query = _postgreDbContext.Businesses;

                //Применяем фильтры, если они указаны                
                if (minProfit > 0)
                {
                    query = query.Where(q => q.ProfitPrice >= Convert.ToDouble(minProfit)).AsQueryable();
                }
                if (maxProfit > 0)
                {
                    query = query.Where(q => q.ProfitPrice <= Convert.ToDouble(maxProfit)).AsQueryable();
                }
                if (minPrice > 0)
                {
                    query = query.Where(q => q.Price >= Convert.ToDouble(minPrice)).AsQueryable();
                }
                if (maxPrice > 0)
                {
                    query = query.Where(q => q.Price <= Convert.ToDouble(maxPrice)).AsQueryable();
                }
                if (!string.IsNullOrEmpty(city))
                {
                    query = query.Where(q => q.BusinessCity.Equals(city)).AsQueryable();
                }
                if (!string.IsNullOrEmpty(categoryCode))
                {
                    query = query.Where(q => q.Category.Equals(categoryCode)).AsQueryable();
                }
                query = query.Where(q => q.IsGarant.Equals(isGarant)).AsQueryable();

                if (typeSortPrice is not null)
                {
                    if (typeSortPrice.Equals("Asc"))
                    {
                        query = query.OrderBy(u => u.Price);
                    }

                    if (typeSortPrice.Equals("Desc"))
                    {
                        query = query.OrderByDescending(u => u.Price);
                    }
                                       
                }

                if (query is not null)
                {
                    items = await query.Select(f => new BusinessOutput
                    {
                        DateCreate = f.DateCreate,
                        Price = string.Format("{0:0,0}", f.Price),
                        CountDays = DateTime.Now.Subtract(f.DateCreate).Days,
                        DayDeclination = "дня",
                        Text = f.Text,
                        TextDoPrice = f.TextDoPrice,
                        BusinessName = f.BusinessName,
                        Url = f.UrlsBusiness,
                        IsGarant = f.IsGarant,
                        ProfitPrice = f.ProfitPrice,
                        TotalInvest = string.Format("{0:0,0}", f.ProfitPrice),
                        BusinessId = f.BusinessId
                    }).ToListAsync();

                    foreach (var item in items)
                    {
                        item.DayDeclination = await _commonService.GetCorrectDayDeclinationAsync(item.CountDays);
                    }
                }                

                return items;
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
        /// Метод обновит поле одобрения карточки бизнеса.
        /// </summary>
        /// <param name="businessId">Id бизнеса.</param>
        /// <returns>Статус одобрения.</returns>
        public async Task<bool> UpdateAcceptedBusinessAsync(long businessId)
        {
            try
            {
                var result = await _postgreDbContext.Businesses
                    .Where(b => b.BusinessId == businessId)
                    .FirstOrDefaultAsync();

                if (result != null)
                {
                    result.IsAccepted = true;
                    await _postgreDbContext.SaveChangesAsync();
                    
                    return true;
                }

                return false;
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
        /// Метод обновит поле отклонения карточки бизнеса
        /// </summary>
        /// <param name="businessId">Id бизнеса.</param>
        /// <param name="comment">Комментарий отклонения.</param>
        /// <returns>Статус отклонения.</returns>
        public async Task<bool> UpdateRejectedBusinessAsync(long businessId, string comment)
        {
            try
            {
                var result = await _postgreDbContext.Businesses
                    .Where(b => b.BusinessId == businessId)
                    .FirstOrDefaultAsync();

                if (result != null)
                {
                    result.IsRejected = true;
                    result.IsAccepted = false;
                    result.CommentRejection = comment;
                    await _postgreDbContext.SaveChangesAsync();

                    return true;
                }

                return false;
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
        /// Метод получит список бизнесов, которые ожидают согласования.
        /// </summary>
        /// <returns>Список бизнесов.</returns>
        public async Task<IEnumerable<BusinessOutput>> GetNotAcceptedBusinessesAsync()
        {
            try
            {
                var items = await (from p in _postgreDbContext.Businesses
                        where p.IsAccepted == false && p.IsRejected == false
                        select new BusinessOutput
                        {
                            DateCreate = p.DateCreate,
                            Price = string.Format("{0:0,0}", p.Price),
                            CountDays = DateTime.Now.Subtract(p.DateCreate).Days,
                            DayDeclination = "дня",
                            Text = p.Text,
                            TextDoPrice = p.TextDoPrice,
                            BusinessName = p.BusinessName,
                            Url = p.UrlsBusiness,
                            IsGarant = p.IsGarant,
                            ProfitPrice = p.ProfitPrice,
                            TotalInvest = string.Format("{0:0,0}", p.InvestPrice),
                            BusinessId = p.BusinessId
                        })
                    .ToListAsync();

                return items;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogError();
                throw;
            }
        }
    }
}
