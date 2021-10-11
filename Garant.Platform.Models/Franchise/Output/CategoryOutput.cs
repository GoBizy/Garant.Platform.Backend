﻿using System;

namespace Garant.Platform.Models.Franchise.Output
{
    /// <summary>
    /// Класс выходной модели категорий бизнеса.
    /// </summary>
    public class CategoryOutput
    {
        /// <summary>
        /// Guid код категории бизнеса.
        /// </summary>
        public Guid CategoryCode { get; set; }

        /// <summary>
        /// Вид бизнеса.
        /// </summary>
        public string CategoryName { get; set; }
    }
}
