﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Garant.Platform.Models.Business.Output;
using Microsoft.AspNetCore.Http;

namespace Garant.Platform.Abstractions.Business
{
    /// <summary>
    /// Абстракция сервиса готового бизнеса.
    /// </summary>
    public interface IBusinessService
    {
        /// <summary>
        /// Метод создаст или обновит карточку бизнеса.
        /// </summary>
        /// <param name="businessFilesInput">Файлы карточки.</param>
        /// <param name="businessDataInput">Входная модель.</param>
        /// <param name="account">Логин.</param>
        /// <returns>Данные карточки бизнеса.</returns>
        Task<CreateUpdateBusinessOutput> CreateUpdateBusinessAsync(IFormCollection businessFilesInput, string businessDataInput, string account);

        /// <summary>
        /// Метод отправит файл в папку и временно запишет в БД.
        /// </summary>
        /// <param name="form">Файлы.</param>
        /// <returns>Список названий файлов.</returns>
        Task<IEnumerable<string>> AddTempFilesBeforeCreateBusinessAsync(IFormCollection form, string account);

        /// <summary>
        /// Метод получит бизнес для просмотра или изменения.
        /// </summary>
        /// <param name="businessId">Id бизнеса.</param>
        /// <param name="mode">Режим (Edit или View).</param>
        /// <returns>Данные бизнеса.</returns>
        Task<BusinessOutput> GetBusinessAsync(long businessId, string mode = null);

        /// <summary>
        /// Метод получит список категорий бизнеса.
        /// </summary>
        /// <returns>Список категорий.</returns>
        Task<IEnumerable<GetBusinessCategoryOutput>> GetBusinessCategoriesAsync();

        /// <summary>
        /// Метод получит список подкатегорий бизнеса.
        /// </summary>
        /// <returns>Список подкатегорий.</returns>
        Task<IEnumerable<BusinessSubCategoryOutput>> GetSubBusinessCategoryListAsync();

        /// <summary>
        /// Метод получит список городов.
        /// </summary>
        /// <returns>Список городов.</returns>
        Task<IEnumerable<BusinessCitiesOutput>> GetCitiesListAsync();

        /// <summary>
        /// Метод получит список популярного бизнеса.
        /// </summary>
        /// <returns>Список бизнеса.</returns>
        Task<IEnumerable<PopularBusinessOutput>> GetPopularBusinessAsync();

        /// <summary>
        /// Метод получит список бизнеса.
        /// </summary>
        /// <returns>Список бизнеса.</returns>
        Task<IEnumerable<PopularBusinessOutput>> GetBusinessListAsync();

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
        Task<IEnumerable<BusinessOutput>> FilterBusinessesAsync(string typeSortPrice, double profitMinPrice, double profitMaxPrice, string viewCode, string categoryCode, double minPriceInvest, double maxPriceInvest, bool isGarant = false);

        /// <summary>
        /// Метод получит новый бизнес, который был создан в текущем месяце.
        /// </summary>
        /// <returns>Список бизнеса.</returns>
        Task<IEnumerable<BusinessOutput>> GetNewBusinesseListAsync();
    }
}
