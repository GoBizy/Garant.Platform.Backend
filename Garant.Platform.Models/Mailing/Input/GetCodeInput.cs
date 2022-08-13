namespace Garant.Platform.Models.Mailing.Input
{
    /// <summary>
    /// Класс входной модели проверки полученного кода сброса пароля.
    /// </summary>
    public class GetCodeInput
    {
        /// <summary>
        /// Почта.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Код сброса пароля.
        /// </summary>
        public string Code { get; set; }
    }
}
