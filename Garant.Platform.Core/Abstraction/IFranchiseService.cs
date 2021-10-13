﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Garant.Platform.Models.Franchise.Output;

namespace Garant.Platform.Core.Abstraction
{
    /// <summary>
    /// Абстракция сервиса франшиз.
    /// </summary>
    public interface IFranchiseService
    {
        /// <summary>
        /// Метод получит список франшиз.
        /// </summary>
        /// <returns>Список франшиз.</returns>
        Task<IEnumerable<FranchiseOutput>> GetFranchisesListAsync();

        /// <summary>
        /// Метод получит список популярных франшиз для главной страницы.
        /// </summary>
        /// <returns>Список франшиз.</returns>
        Task<IEnumerable<PopularFranchiseOutput>> GetMainPopularFranchises();

        /// <summary>
        /// Метод получит 4 франшизы для выгрузки в блок с быстрым поиском.
        /// </summary>
        /// <returns>Список франшиз.</returns>
        Task<IEnumerable<FranchiseOutput>> GetFranchiseQuickSearchAsync();

        /// <summary>
        /// Метод получит список городов франшиз.
        /// </summary>
        /// <returns>Список городов.</returns>
        Task<IEnumerable<FranchiseCityOutput>> GetFranchisesCitiesListAsync();

        /// <summary>
        /// Метод получит список категорий бизнеса.
        /// </summary>
        /// <returns>Список категорий.</returns>
        Task<IEnumerable<CategoryOutput>> GetFranchisesCategoriesListAsync();

        /// <summary>
        /// Метод получит список видов бизнеса.
        /// </summary>
        /// <returns>Список бизнеса.</returns>
        Task<IEnumerable<ViewBusinessOutput>> GetFranchisesViewBusinessListAsync();

        /// <summary>
        /// Метод фильтрации франшиз по разным атрибутам.
        /// </summary>
        /// <param name="typeSort">Тип фильтрации цены (по возрастанию или убыванию).</param>
        /// <param name="isGarant">Покупка через гарант.</param>
        /// <param name="minPrice">Прибыль от.</param>
        /// <param name="maxPrice">Прибыль до.</param>
        /// <returns>Список франшиз после фильтрации.</returns>
        Task<IEnumerable<FranchiseOutput>> FilterFranchisesAsync(string typeSort, string minPrice, string maxPrice, bool isGarant = false);
    }
}
