﻿using Garant.Platform.Core.Data;

namespace Garant.Platform.Abstractions.DataBase
{
    /// <summary>
    /// Абстракция сервиса настроек БД.
    /// </summary>
    public interface IDataBaseConfig
    {
        /// <summary>
        /// Метод вернет датаконтекст.
        /// </summary>
        /// <returns>Датаконтекст</returns>
        PostgreDbContext GetDbContext();

        /// <summary>
        /// Метод вернет датаконтекст.
        /// </summary>
        /// <returns>Датаконтекст</returns>
        IdentityDbContext GetIdentityDbContext();
    }
}