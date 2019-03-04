using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using MVCWeb.Cores.Entities;
using MVCWeb.Cores.IRepositories;
using MVCWeb.Cores.IServices;
using Unity.Mvc5;

namespace MVCWeb
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // Database context, one per request, ensure it is disposed
            container.BindInRequestScope<IDbAppContext, DbAppContext>();

            //Bind the various domain model services and repositories that e.g. our controllers require         
            //container.BindInRequestScope<IUnitOfWorkManager, UnitOfWorkManager>();
            RegisterRepositories(container);
            RegisterServices(container);
            //container.BindInRequestScope<ISessionHelper, SessionHelper>();

            return container;
        }

        private static void RegisterRepositories(IUnityContainer container)
        {
            var respositories =
                AllClasses.FromLoadedAssemblies()
                    .Where(type => typeof(IWebAppRepository).IsAssignableFrom(type))
                    .ToList();

            container.RegisterTypes(
                respositories,
                FromAllInterfacesIgnore,
                WithName.Default,
                WithLifetime.Transient, null, true);
        }

        private static void RegisterServices(IUnityContainer container)
        {
            var services = AllClasses.FromLoadedAssemblies()
                .Where(type => typeof(IWebAppService).IsAssignableFrom(type))
                .ToList();

            container.RegisterTypes(
                services,
                FromAllInterfacesIgnore,
                WithName.Default,
                WithLifetime.Transient, null, true);
        }

        private static IEnumerable<Type> FromAllInterfacesIgnore(Type type)
        {
            var intefaces = WithMappings.FromAllInterfaces(type)
                .Where(o => o != typeof(IWebAppRepository))
                .Where(o => o != typeof(IWebAppService))
                ;
            return intefaces;
        }
    }
    public static class IocExtensions
    {
        public static void BindInRequestScope<T1, T2>(this IUnityContainer container) where T2 : T1
        {
            container.RegisterType<T1, T2>(new HierarchicalLifetimeManager());
        }

        public static void BindInSingletonScope<T1, T2>(this IUnityContainer container) where T2 : T1
        {
            container.RegisterType<T1, T2>(new ContainerControlledLifetimeManager());
        }
    }
}