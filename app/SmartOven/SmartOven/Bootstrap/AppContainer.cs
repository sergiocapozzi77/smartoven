// <copyright file="AppContainer.cs" company="Racing Solutions Ltd.">
// Copyright (c) Racing Solutions Ltd.</copyright>

using Autofac;
using SmartOvenV2.Services;
using SmartOvenV2.ViewModels;
using System;
using SmartOvenV2.Managers;
using SmartOvenV2.Models;

namespace PizzaTime.Bootstrap
{
    public class AppContainer
    {
        private static IContainer container;

        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<BleConnector>().As<IBleConnector>().SingleInstance();
#if DEBUG
            builder.RegisterType<StatusPoller>().As<IStatusPoller>().SingleInstance();
#else
            builder.RegisterType<StatusPoller>().As<IStatusPoller>().SingleInstance();
#endif
            builder.RegisterType<RecipesRemoteService>().As<IRecipesService>().SingleInstance();
            builder.RegisterType<RecipesManager>().As<IRecipesManager>().SingleInstance();
            builder.RegisterType<AppStatusManager>().As<IAppStatusManager>().SingleInstance();
            builder.RegisterType<OtaService>().As<IOtaService>().SingleInstance();
            builder.RegisterType<NotificationService>().As<INotificationService>().SingleInstance();
            builder.RegisterType<RecipeStepFactory>().As<IRecipeStepFactory>().SingleInstance();
            builder.RegisterType<SeatableDataService>().As<ISeatableDataService>().SingleInstance();

            builder.RegisterType<OvenViewModel>();
            builder.RegisterType<InfoViewModel>();
            builder.RegisterType<RecipeViewModel>();
            builder.RegisterType<IngredientsViewModel>();

            container = builder.Build();
        }

        public static object Resolve(Type typeName)
        {
            return container.Resolve(typeName);
        }

        public static T Resolve<T>()
        {
            return container.Resolve<T>();
        }
    }
}