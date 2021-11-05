﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Garant.Platform.Abstractions.Business;
using Garant.Platform.Models.Business.Output;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Garant.Platform.Controllers.ReadyBusiness
{
    /// <summary>
    /// Контроллер готового бизнеса.
    /// </summary>
    [ApiController]
    [Route("business")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BusinessController : BaseController
    {
        private readonly IBusinessService _businessService;

        public BusinessController(IBusinessService businessService)
        {
            _businessService = businessService;
        }

        /// <summary>
        /// Метод создаст или обновит карточку бизнеса.
        /// </summary>
        /// <param name="businessFilesInput">Файлы карточки.</param>
        /// <param name="businessDataInput">Входная модель.</param>
        /// <returns>Данные карточки бизнеса.</returns>
        [HttpPost]
        [Route("create-update-business")]
        [ProducesResponseType(200, Type = typeof(CreateUpdateBusinessOutput))]
        public async Task<IActionResult> CreateUpdateBusinessAsync([FromForm] IFormCollection businessFilesInput, [FromForm] string businessDataInput)
        {
            var result = await _businessService.CreateUpdateBusinessAsync(businessFilesInput, businessDataInput, GetUserName());

            return Ok(result);
        }

        /// <summary>
        /// Метод отправит файл в папку и запишет в БД.
        /// </summary>
        /// <param name="files">Файлы.</param>
        [HttpPost]
        [Route("temp-file")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<string>))]
        public async Task<IActionResult> AddTempFilesBeforeCreateBusinessAsync([FromForm] IFormCollection files)
        {
            var result = await _businessService.AddTempFilesBeforeCreateBusinessAsync(files, GetUserName());

            return Ok(result);
        }
    }
}
