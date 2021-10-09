﻿using Garant.Platform.Models.Entities.Actions;
using Garant.Platform.Models.Entities.Category;
using Garant.Platform.Models.Entities.Footer;
using Garant.Platform.Models.Entities.Header;
using Garant.Platform.Models.Entities.LastBuy;
using Garant.Platform.Models.Entities.Logger;
using Garant.Platform.Models.Entities.News;
using Garant.Platform.Models.Entities.Suggestion;
using Garant.Platform.Models.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace Garant.Platform.Core.Data
{
    public class PostgreDbContext : DbContext
    {
        private readonly DbContextOptions<PostgreDbContext> _options;

        public PostgreDbContext(DbContextOptions<PostgreDbContext> options) : base(options)
        {
            _options = options;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) { }

        /// <summary>
        /// Таблица пользователей.
        /// </summary>
        public DbSet<UserEntity> Users { get; set; }

        /// <summary>
        /// Таблица логов.
        /// </summary>
        public DbSet<LoggerEntity> Logs { get; set; }

        /// <summary>
        /// Таблица хидера.
        /// </summary>
        public DbSet<HeaderEntity> Headers { get; set; }

        /// <summary>
        /// Таблица футера.
        /// </summary>
        public DbSet<FooterEntity> Footers { get; set; }

        /// <summary>
        /// Таблица информации пользователя dbo.UsersInformation.
        /// </summary>
        public DbSet<UserInformationEntity> UsersInformation { get; set; }

        /// <summary>
        /// Таблица новостей dbo.News.
        /// </summary>
        public DbSet<NewsEntity> News { get; set; }

        /// <summary>
        /// Таблица категорий dbo.BusinessCategories..
        /// </summary>
        public DbSet<BusinessCategoryEntity> BusinessCategories { get; set; }

        /// <summary>
        /// Таблица последних покупок dbo.LastBuy.
        /// </summary>
        public DbSet<LastBuyEntity> LastBuys { get; set; }

        /// <summary>
        /// Таблица событий на главной странице dbo.MainPageActions.
        /// </summary>
        public DbSet<MainPageActionEntity> MainPageActions { get; set; }

        /// <summary>
        /// Таблица предложений dbo.Suggestions.
        /// </summary>
        public DbSet<SuggestionEntity> Suggestions { get; set; }
    }
}
