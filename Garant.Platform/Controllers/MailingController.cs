﻿using System.Threading.Tasks;
using Garant.Platform.Core.Abstraction;
using Garant.Platform.Models.Mailing.Input;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Garant.Platform.Controllers
{
    /// <summary>
    /// Контроллер работы с рассылками.
    /// </summary>
    [ApiController, Route("mailing")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MailingController : BaseController
    {
        private readonly ICommonService _commonService;

        public MailingController(ICommonService commonService)
        {
            _commonService = commonService;
        }

        /// <summary>
        /// Метод отправит код подтверждения. Также запишет этот код в базу.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost, Route("send-sms-confirm-code")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public async Task<IActionResult> SendMailAcceptCodeSmsAsync([FromBody] SendAcceptCodeInput sendAcceptCodeInput)
        {
            await _commonService.GenerateAcceptCodeAsync(sendAcceptCodeInput.Data);

            return Ok();
        }
    }
}
