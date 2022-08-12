namespace Garant.Platform.Models.Mailing.Input
{
    /// <summary>
    /// Класс входной модели сброса пароля.
    /// </summary>
    public class ResetPasswordInput
    {
        /// <summary>
        /// Почта.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Новый пароль.
        /// </summary>
        public string Password { get; set; }
    }
}
