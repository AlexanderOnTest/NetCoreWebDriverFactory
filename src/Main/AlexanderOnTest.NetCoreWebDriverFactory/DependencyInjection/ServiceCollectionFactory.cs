using System;
using AlexanderOnTest.NetCoreWebDriverFactory.DriverManager;
using AlexanderOnTest.NetCoreWebDriverFactory.DriverOptionsFactory;
using AlexanderOnTest.NetCoreWebDriverFactory.Utils;
using AlexanderOnTest.NetCoreWebDriverFactory.Utils.Builders;
using AlexanderOnTest.NetCoreWebDriverFactory.WebDriverFactory;
using Microsoft.Extensions.DependencyInjection;

namespace AlexanderOnTest.NetCoreWebDriverFactory.DependencyInjection
{
    /// <summary>
    /// Convenient DI Container factory
    /// </summary>
    public static class ServiceCollectionFactory
    {
        /// <summary>
        /// Get a ServiceCollection referencing default implementations using implicit path.
        /// Use for .NET Framework projects or projects where the driver executables are on the System Path.
        /// </summary>
        /// <param name="driverConfig"></param>
        /// <returns></returns>
        public static IServiceCollection GetDefaultServiceCollection(
            IWebDriverConfiguration driverConfig)
        {
            return GetDefaultServiceCollection(null, driverConfig);
        }

        /// <summary>
        /// Get a ServiceCollection referencing default implementations using implicit path.
        /// Use for .NET Framework projects or projects where the driver executables are on the System Path.
        /// </summary>
        /// <param name="gridUri"></param>
        /// <returns></returns>
        public static IServiceCollection GetDefaultServiceCollection(
            Uri gridUri)
        {
            return GetDefaultServiceCollection(null, GetDefaultWebDriverConfigurationFromUri(gridUri));
        }

        /// <summary>
        /// Get a ServiceCollection referencing default implementations with a defined driver path.
        /// Use for .NET Core projects with driver executables from nuget packages.
        /// </summary>
        /// <param name="driverPath"></param>
        /// <returns></returns>
        public static IServiceCollection GetDefaultServiceCollection(
            DriverPath driverPath)
        {
            return GetDefaultServiceCollection(driverPath, (IWebDriverConfiguration) null);
        }

        /// <summary>
        /// Get a ServiceCollection referencing default implementations with a defined driver path.
        /// Use for .NET Core projects with driver executables from nuget packages.
        /// </summary>
        /// <param name="driverPath"></param>
        /// <param name="gridUri"></param>
        /// <returns></returns>
        public static IServiceCollection GetDefaultServiceCollection(
            DriverPath driverPath,
            Uri gridUri)
        {
            return GetDefaultServiceCollection(driverPath, GetDefaultWebDriverConfigurationFromUri(gridUri));
        }

        /// <summary>
        /// Get a ServiceCollection referencing all Default implementations. 
        /// </summary>
        /// <param name="driverPath"></param>
        /// <param name="driverConfig"></param>
        /// <returns></returns>
        public static IServiceCollection GetDefaultServiceCollection(
            DriverPath driverPath,
            IWebDriverConfiguration driverConfig)
        {
            IServiceCollection services = new ServiceCollection();
            
            services.AddSingleton<IDriverOptionsFactory, DefaultDriverOptionsFactory>();
            services.AddSingleton<IWebDriverReSizer, WebDriverReSizer>();

            if (driverPath != null)
            {
                services.AddSingleton<DriverPath>(driverPath);
                services.AddSingleton<ILocalWebDriverFactory, DefaultLocalWebDriverFactory>();
            }
            else
            {
                services.AddSingleton<ILocalWebDriverFactory, FrameworkLocalWebDriverFactory>();
            }

            services.AddSingleton(driverConfig ?? WebDriverConfigurationBuilder.Start().Build());

            services.AddSingleton<IRemoteWebDriverFactory, DefaultRemoteWebDriverFactory>();

            services.AddSingleton<IWebDriverFactory, DefaultWebDriverFactory>();
            services.AddSingleton<IWebDriverManager, WebDriverManager>();
            
            return services;
        }
        
        private static IWebDriverConfiguration GetDefaultWebDriverConfigurationFromUri(Uri gridUri)
        {
            return WebDriverConfigurationBuilder.Start().WithGridUri(gridUri).Build();
        }
    }
}
