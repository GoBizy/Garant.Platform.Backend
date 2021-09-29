﻿using Autofac;
using Garant.Platform.Core.Abstraction;
using Garant.Platform.Core.Attributes;
using Garant.Platform.Service.Service.Common;
using Garant.Platform.Service.Service.User;

namespace Garant.Platform.Service.AutofacModules
{
    /// <summary>
    /// Класс регистрации сервисов автофака.
    /// </summary>
    [CommonModule]
    public sealed class CommonServicesModule : Module
    {
        public static void InitModules(ContainerBuilder builder)
        {
            // Сервис пользователя.
            builder.RegisterType<UserService>().Named<IUserService>("UserService");
            builder.RegisterType<UserService>().As<IUserService>();

            // Общий сервис.
            builder.RegisterType<CommonService>().Named<ICommonService>("CommonService");
            builder.RegisterType<CommonService>().As<ICommonService>();
        }
    }
}
