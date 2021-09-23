﻿//using Autofac;
//using Garant.Platform.Mailings.AutofacModules;

//namespace Garant.Platform.Base.Utils
//{
//    public static class AutofacExtensions
//    {
//        public static void InitModules(ContainerBuilder builder)
//        {
//            builder.RegisterModule(new MailingModule());
//            //builder.RegisterModule(new CommonServicesModule());
//        }

//        //private static ContainerBuilder _builder;
//        //private static IContainer _container;

//        ///// <summary>
//        ///// Получить сервис
//        ///// </summary>
//        ///// <typeparam name="TService">Тип сервиса</typeparam>
//        ///// <param name="notException">Не выдавать исключение если не удалось получить объект По умолчанию false</param> 
//        ///// <returns>Экземпляр запрашиваемого сервиса</returns>
//        //public static TService Resolve<TService>() where TService : class
//        //{
//        //    if (_container == null)
//        //    {
//        //        _builder = new ContainerBuilder();
//        //        _container = _builder.Build();
//        //    }

//        //    if (!_container.IsRegistered<TService>())
//        //    {
//        //        return null;
//        //    }

//        //    var service = _container.Resolve<TService>();

//        //    return service;
//        //}

//        //public static ILifetimeScope CreateLifetimeScope()
//        //{
//        //    if (_container == null)
//        //    {
//        //        _builder = new ContainerBuilder();
//        //        _container = _builder.Build();
//        //    }

//        //    return _container.BeginLifetimeScope();
//        //}

//        ///// <summary>
//        ///// Получить сервис по уникальному имени
//        ///// </summary>
//        ///// <typeparam name="TService">Экземпляр запрашиваемого сервиса</typeparam>
//        ///// <param name="serviceName">Уникальное имя запрашиваемого типа</param>
//        ///// <param name="notException">Не выдавать исключение если не удалось получить объект По умолчанию false</param>
//        ///// <returns></returns>
//        //public static TService ResolveNamedScoped<TService>(this ILifetimeScope scope, string serviceName) where TService : class
//        //{
//        //    if (!_container.IsRegisteredWithName<TService>(serviceName))
//        //    {
//        //        return null;
//        //    }

//        //    var service = _container.ResolveNamed<TService>(serviceName);

//        //    return service;
//        //}
//    }
//}
