using Garant.Platform.Models.Configurator.Output;
using Garant.Platform.Models.Entities.Business;
using System.Collections.Generic;

namespace Garant.Platform.Models.Business.Output
{
    /// <summary>
    /// Класс выходной модели бизнеса.
    /// </summary>
    public class BusinessOutput : BusinessEntity
    {
        /// <summary>
        /// Кол-во дней.
        /// </summary>
        public int CountDays { get; set; }

        /// <summary>
        /// Склонение дней.
        /// </summary>
        public string DayDeclination { get; set; }

        /// <summary>
        /// Полная строка текста для вставки в одном поле.
        /// </summary>
        public string FullText { get; set; }

        /// <summary>
        /// Режим просмотр или редактирование бизнеса.
        /// </summary>
        public string Mode { get; set; }

        /// <summary>
        /// Сумма инвестиций.
        /// </summary>
        public string TotalInvest { get; set; }

        public new string Price { get; set; }

        /// <summary>
        /// Полное ФИО создавшего бизнес.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Путь к изображению.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Сконвертированное поле InvestPrice.
        /// </summary>
        public List<ConvertInvestPriceIncludeOutput> InvestPriceOutput { get; set; }

    }
}
