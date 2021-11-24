﻿namespace Garant.Platform.Models.Transition.Input
{
    /// <summary>
    /// Класс входной модели перехода.
    /// </summary>
    public class TransitionInput
    {
        /// <summary>
        /// Id пользователя, который совершил переход.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Тип перехода.
        /// </summary>
        public string TransitionType { get; set; }

        /// <summary>
        /// Id франшизы или готового бизнеса.
        /// </summary>
        public long ReferenceId { get; set; }

        /// <summary>
        /// Id другого пользователя.
        /// </summary>
        public string OtherId { get; set; }

        /// <summary>
        /// Тип обсуждения.
        /// </summary>
        public string TypeItem { get; set; }
    }
}
