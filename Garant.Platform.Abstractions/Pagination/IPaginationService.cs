﻿using System.Threading.Tasks;
using Garant.Platform.Models.Pagination.Output;

namespace Garant.Platform.Abstractions.Pagination
{
    /// <summary>
    /// Абстракция сервиса пагинации.
    /// </summary>
    public interface IPaginationService
    {
        /// <summary>
        /// Метод пагинации на ините каталога франшиз.
        /// </summary>
        /// <param name="pageIdx">Номер страницы.</param>
        /// <returns>Данные пагинации.</returns>
        Task<IndexOutput> GetInitPaginationCatalogFranchiseAsync(int pageIdx);

        /// <summary>
        /// Метод получения пагинации франшиз в каталоге.
        /// </summary>
        /// <param name="pageNumber">Номер страницы.</param>
        /// <param name="countRows">Кол-во строк.</param>
        /// <returns>Данные пагинации.</returns>
        Task<IndexOutput> GetPaginationCatalogFranchiseAsync(int pageNumber, int countRows);

        /// <summary>
        /// Метод пагинации на ините каталога франшиз.
        /// </summary>
        /// <param name="pageIdx">Номер страницы.</param>
        /// <returns>Данные пагинации.</returns>
        Task<IndexOutput> GetInitPaginationCatalogBusinessAsync(int pageIdx);

        /// <summary>
        /// Метод получения пагинации бизнеса в каталоге.
        /// </summary>
        /// <param name="pageNumber">Номер страницы.</param>
        /// <param name="countRows">Кол-во строк.</param>
        /// <returns>Данные пагинации.</returns>
        Task<IndexOutput> GetPaginationCatalogBusinessAsync(int pageNumber, int countRows);
    }
}
