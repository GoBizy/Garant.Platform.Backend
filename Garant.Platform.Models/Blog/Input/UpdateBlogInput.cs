﻿namespace Garant.Platform.Models.Blog.Input
{
    /// <summary>
    /// Класс входной модели блога.
    /// </summary>
    public class UpdateBlogInput
    {
        /// <summary>
        /// PK.
        /// </summary>
        public long BlogId { get; set; }

        /// <summary>
        /// Заголовок.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Оплачено ли размещение блога на главной.
        /// </summary>
        public bool IsPaid { get; set; }

        /// <summary>
        /// Позиция при размещении.
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Код темы блога.
        /// </summary>
        public string ThemeCategoryCode { get; set; }
    }
}
