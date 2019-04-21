using System;
using AlexanderOnTest.NetCoreWebDriverFactory.DriverManager;
using AlexanderOnTest.NetCoreWebDriverFactory.DriverOptionsFactory;
using AlexanderOnTest.NetCoreWebDriverFactory.Utils;
using AlexanderOnTest.NetCoreWebDriverFactory.WebDriverFactory;
using Microsoft.Extensions.DependencyInjection;

namespace AlexanderOnTest.NetCoreWebDriverFactory.DependencyInjection
{
    /// <summary>
    /// Convenient DI Containers
    /// </summary>
    public static class IoCContainerFactory
    {
        /// <summary>
        /// Get an IServiceProvider containing all Default implementations. 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IServiceProvider GetDefaultProviderForLocalTesting(DriverPath path)
        {

            ServiceCollection services = new ServiceCollection();
            services.AddSingleton(typeof(DriverPath), path);
            
            AddCommonDependencies(ref services);

            return services.BuildServiceProvider();
        }

        /// <summary>
        /// Get an IServiceProvider containing all Default implementations. 
        /// </summary>
        /// <param name="gridUri"></param>
        /// <returns></returns>
        public static IServiceProvider GetDefaultProviderForGridTesting(Uri gridUri)
        {

            ServiceCollection services = new ServiceCollection();
            services.AddSingleton(typeof(Uri), gridUri);
            
            AddCommonDependencies(ref services);

            return services.BuildServiceProvider();
        }

        /// <summary>
        /// Get an IServiceProvider containing all Default implementations. 
        /// </summary>
        /// <param name="webDriverConfiguration"></param>
        /// <returns></returns>
        public static IServiceProvider GetDefaultProviderForGridTesting(IWebDriverConfiguration webDriverConfiguration)
        {

            ServiceCollection services = new ServiceCollection();
            services.AddSingleton(typeof(IWebDriverConfiguration), webDriverConfiguration);

            AddCommonDependencies(ref services);

            return services.BuildServiceProvider();
        }

        /// <summary>
        /// Get an IServiceProvider containing all Default implementations. 
        /// </summary>
        /// <param name="gridUri"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IServiceProvider GetDefaultProviderForLocalAndGridTesting(Uri gridUri, DriverPath path)
        {

            ServiceCollection services = new ServiceCollection();
            services.AddSingleton(typeof(DriverPath), path);
            services.AddSingleton(typeof(Uri), gridUri);

            AddCommonDependencies(ref services);

            return services.BuildServiceProvider();
        }

        /// <summary>
        /// Get an IServiceProvider containing all Default implementations. 
        /// </summary>
        /// <param name="webDriverConfiguration"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IServiceProvider GetDefaultProviderForLocalAndGridTesting(IWebDriverConfiguration webDriverConfiguration, DriverPath path)
        {

            ServiceCollection services = new ServiceCollection();
            services.AddSingleton(typeof(DriverPath), path);
            services.AddSingleton(typeof(IWebDriverConfiguration), webDriverConfiguration);

            AddCommonDependencies(ref services);

            return services.BuildServiceProvider();
        }

        private static void AddCommonDependencies(ref ServiceCollection services)
        {
            services.AddSingleton(typeof(IDriverOptionsFactory), typeof(DefaultDriverOptionsFactory));
            services.AddSingleton(typeof(IWebDriverReSizer), typeof(WebDriverReSizer));

            services.AddSingleton(typeof(ILocalWebDriverFactory), typeof(DefaultLocalWebDriverFactory));
            services.AddSingleton(typeof(IRemoteWebDriverFactory), typeof(DefaultRemoteWebDriverFactory));

            services.AddSingleton(typeof(IWebDriverFactory), typeof(DefaultWebDriverFactory));
            services.AddSingleton(typeof(IWebDriverManager), typeof(WebDriverManager));
        }
    }
}
