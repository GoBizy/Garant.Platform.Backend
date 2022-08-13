namespace Garant.Platform.Models.Mailing.Input
{
    /// <summary>
    /// Класс входной модели отправки кода сброса пароля.
    /// </summary>
    public class ForgotPasswordInput
    {
        /// <summary>
        /// Почта.
        /// </summary>
        public string Email { get; set; }
    }
}
