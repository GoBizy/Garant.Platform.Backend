﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Garant.Platform.Models.Blog.Input;
using Garant.Platform.Models.Blog.Output;

namespace Garant.Platform.Abstractions.Blog
{
    /// <summary>
    /// Абстракция сервиса блогов и новостей.
    /// </summary>
    public interface IBlogService
    {
        /// <summary>
        /// Метод получит список блогов, у которых стоит флаг IsPaid. Т.е те, которые проплачены за размещение на главной.
        /// </summary>
        /// <returns>Список объявлений.</returns>
        Task<IEnumerable<BlogOutput>> GetBlogsListMainPageAsync();

        /// <summary>
        /// Метод получит список новостей, у которых стоит флаг IsPaid. Т.е те, которые проплачены за размещение на главной.
        /// </summary>
        /// <returns>Список новостей.</returns>
        Task<IEnumerable<NewsOutput>> GetTopNewsMainPageAsync();

        /// <summary>
        /// Метод получит список тем блогов.
        /// </summary>
        /// <returns>Список тем блогов.</returns>
        Task<IEnumerable<BlogThemesOutput>> GetBlogThemesAsync();

        /// <summary>
        /// Метод получит список блогов.
        /// </summary>
        /// <returns>Список блогов.</returns>
        Task<IEnumerable<BlogOutput>> GetBlogsListAsync();

        /// <summary>
        /// Метод создаст блог.
        /// </summary>
        /// <param name="blogInput">Входная модель блога.</param>
        /// <returns>Созданный блог.</returns>
        Task<BlogOutput> CreateBlogAsync(CreateBlogInput blogInput);

        /// <summary>
        /// Метод обновит существующий блог.
        /// </summary>
        /// <param name="blogInput">Входная модель блога.</param>
        /// <returns>Обновлённый блог.</returns>
        Task<BlogOutput> UpdateBlogAsync(UpdateBlogInput blogInput);
    }
}
