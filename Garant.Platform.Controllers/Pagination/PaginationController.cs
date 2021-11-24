﻿using System.Threading.Tasks;
using Garant.Platform.Abstractions.Pagination;
using Garant.Platform.Models.Pagination.Input;
using Garant.Platform.Models.Pagination.Output;
using Microsoft.AspNetCore.Mvc;

namespace Garant.Platform.Controllers.Pagination
{
    /// <summary>
    /// Контроллер пагинации.
    /// </summary>
    [ApiController, Route("pagination")]
    public class PaginationController : BaseController
    {
        private readonly IPaginationService _paginationService;

        public PaginationController(IPaginationService paginationService)
        {
            _paginationService = paginationService;
        }

        /// <summary>
        /// Метод пагинации на ините каталога франшиз.
        /// </summary>
        /// <param name="pageIdx">Номер страницы. По дефолту 1.</param>
        /// <returns>Данные пагинации.</returns>
        [HttpPost, Route("init-catalog-franchise")]
        [ProducesResponseType(200, Type = typeof(IndexOutput))]
        public async Task<IActionResult> GetInitPaginationCatalogFranchiseAsync([FromBody] PaginationInput paginationInput)
        {
            var paginationData = await _paginationService.GetInitPaginationCatalogFranchiseAsync(paginationInput.PageNumber);

            return Ok(paginationData);
        }

        /// <summary>
        /// Метод получения пагинации франшиз в каталоге.
        /// </summary>
        /// <param name="pageIdx">Номер страницы. По дефолту 1.</param>
        /// <returns>Данные пагинации.</returns>
        [HttpPost, Route("catalog-franchise")]
        [ProducesResponseType(200, Type = typeof(IndexOutput))]
        public async Task<IActionResult> GetPaginationCatalogFranchiseAsync([FromBody] PaginationInput paginationInput)
        {
            var paginationData = await _paginationService.GetPaginationCatalogFranchiseAsync(paginationInput.PageNumber, paginationInput.CountRows);

            return Ok(paginationData);
        }

        /// <summary>
        /// Метод пагинации на ините каталога бизнеса.
        /// </summary>
        /// <param name="pageIdx">Номер страницы. По дефолту 1.</param>
        /// <returns>Данные пагинации.</returns>
        [HttpPost, Route("init-catalog-business")]
        [ProducesResponseType(200, Type = typeof(IndexOutput))]
        public async Task<IActionResult> GetInitPaginationCatalogBusinessAsync([FromBody] PaginationInput paginationInput)
        {
            var paginationData = await _paginationService.GetInitPaginationCatalogBusinessAsync(paginationInput.PageNumber);

            return Ok(paginationData);
        }

        /// <summary>
        /// Метод получения пагинации франшиз в каталоге.
        /// </summary>
        /// <param name="pageIdx">Номер страницы. По дефолту 1.</param>
        /// <returns>Данные пагинации.</returns>
        [HttpPost, Route("catalog-business")]
        [ProducesResponseType(200, Type = typeof(IndexOutput))]
        public async Task<IActionResult> GetPaginationCatalogBusinessAsync([FromBody] PaginationInput paginationInput)
        {
            var paginationData = await _paginationService.GetPaginationCatalogBusinessAsync(paginationInput.PageNumber, paginationInput.CountRows);

            return Ok(paginationData);
        }
    }
}
