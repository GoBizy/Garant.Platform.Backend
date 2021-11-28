﻿using System;

namespace Garant.Platform.Models.Request.Output
{
    /// <summary>
    /// Класс выходной модели создания заявки франшизы.
    /// </summary>
    public class RequestFranchiseOutput
    {
        /// <summary>
        /// PK.
        /// </summary>
        public long RequestId { get; set; }

        /// <summary>
        /// Id пользователя, который оставил заявку.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Номер телефона.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Город.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Дата создания заявки.
        /// </summary>
        public DateTime DateCreate { get; set; }

        /// <summary>
        /// Id франшизы, по которой оставлена заявка.
        /// </summary>
        public long FranchiseId { get; set; }
    }
}
