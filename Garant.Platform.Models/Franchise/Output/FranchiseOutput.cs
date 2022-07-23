using Garant.Platform.Models.Configurator.Output;
using Garant.Platform.Models.Entities.Franchise;
using System.Collections.Generic;

namespace Garant.Platform.Models.Franchise.Output
{
    public class FranchiseOutput : FranchiseEntity
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
        /// Режим просмотр или редактирование франшизы.
        /// </summary>
        public string Mode { get; set; }

        /// <summary>
        /// Сумма инвестиций.
        /// </summary>
        public string TotalInvest { get; set; }

        public new string  Price { get; set; }

        /// <summary>
        /// Полное ФИО создавшего франшизу.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Сконвертированное поле InvestInclude.
        /// </summary>
        public List<ConvertInvestPriceIncludeOutput> InvestIncludeOutput { get; set; }
    }
}
