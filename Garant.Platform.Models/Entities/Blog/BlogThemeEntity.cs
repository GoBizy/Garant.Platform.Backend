﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garant.Platform.Models.Entities.Blog
{
    /// <summary>
    /// Класс сопоставляется с таблицей блогов Info.BlogThemes. 
    /// </summary>
    [Table("BlogThemes", Schema = "Info")]
    public class BlogThemeEntity
    {
        /// <summary>
        /// PK.
        /// </summary>
        [Key]
        public long BlogThemeId { get; set; }

        /// <summary>
        /// Заголовок.
        /// </summary>
        [Column("Title", TypeName = "varchar(200)")]
        public string Title { get; set; }

    }
}