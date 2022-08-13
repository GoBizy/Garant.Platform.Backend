using System.Threading.Tasks;
using Garant.Platform.Base;
using Garant.Platform.Base.Abstraction;
using Garant.Platform.Models.Mailing.Input;
using Garant.Platform.Models.Mailing.Output;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Garant.Platform.Controllers.Mailing
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
        [HttpPost, Route("send-confirm-code")]
        [ProducesResponseType(200, Type = typeof(MailngOutput))]
        public async Task<IActionResult> SendMailAcceptCodeSmsAsync([FromBody] SendAcceptCodeInput sendAcceptCodeInput)
        {
            var result = await _commonService.GenerateAcceptCodeAsync(sendAcceptCodeInput.Data);

            return Ok(result);
        }

        /// <summary>
        /// Метод отправит код сброса пароля. Также запишет этот код в базу данных.
        /// </summary>
        /// <param name="input"> Входная модель. </param>
        /// <returns> Успешно ли отправлен код. </returns>
        [AllowAnonymous]
        [HttpPost, Route("send-reset-code")]
        [ProducesResponseType(200, Type = typeof(MailngOutput))]
        public async Task<IActionResult> ForgotPasswordAsync([FromBody] ForgotPasswordInput input)
        {
            var result = await _commonService.GenerateResetCodeAsync(input.Email);

            return Ok(result);
        }

        /// <summary>
        /// Метод проверит есть ли такой код в базе данных и соответствует ли он email.
        /// </summary>
        /// <param name="input"> Входная модель. </param>
        /// <returns> Флаг успеха. </returns>
        [AllowAnonymous]
        [HttpPost, Route("check-reset-code")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public async Task<IActionResult> GetAndCheckCodeAsync([FromBody] GetCodeInput input)
        {
            var result = await _commonService.CheckCodeAsync(input.Email, input.Code);

            return Ok(result);
        }

        /// <summary>
        /// Метод изменяет старый парол на новый. Также делает хэш нового пароля.
        /// </summary>
        /// <param name="input"> Входная модель. </param>
        /// <returns> Флаг успеха. </returns>
        [AllowAnonymous]
        [HttpPost, Route("change-password")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] ResetPasswordInput input)
        {
            var result = await _commonService.ChangePasswordAsync(input.Email, input.Password);

            return Ok(result);
        }
    }
}
