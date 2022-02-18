// <copyright>
// Copyright 2019 Alexander Dunn
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>

using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using AlexanderOnTest.NetCoreWebDriverFactory.Config;
using AlexanderOnTest.NetCoreWebDriverFactory.DriverManager;
using AlexanderOnTest.NetCoreWebDriverFactory.DriverOptionsFactory;
using AlexanderOnTest.NetCoreWebDriverFactory.Utils;
using AlexanderOnTest.NetCoreWebDriverFactory.Utils.Builders;
using AlexanderOnTest.NetCoreWebDriverFactory.WebDriverFactory;
using Microsoft.Extensions.DependencyInjection;

namespace AlexanderOnTest.NetCoreWebDriverFactory.DependencyInjection
{
    /// <summary>
    /// Example DI Container factory for quick prototyping.
    /// <para>WARNING: This should not be considered stable for use in production projects.
    /// You are STRONGLY advised to use the source code for inspiration rather than using these methods directly.</para>
    /// </summary>
    [QuickStart]
    public static class ServiceCollectionFactory
    {
        /// <summary>
        /// Get a ServiceCollection referencing default implementations using implicit path.
        /// <para>WARNING: This should not be considered stable for use in production projects.
        /// You are STRONGLY advised to use the source code for inspiration rather than using this method directly.</para>
        /// </summary>
        /// <returns></returns>
        [QuickStart]
        public static IServiceCollection GetDefaultServiceCollection()
        {
            return GetDefaultServiceCollection((IWebDriverConfiguration) null, null);
        }

        /// <summary>
        /// Get a ServiceCollection referencing default implementations using implicit path.
        /// <para>WARNING: This should not be considered stable for use in production projects.
        /// You are STRONGLY advised to use the source code for inspiration rather than using this method directly.</para>
        /// </summary>
        /// <param name="driverConfig"></param>
        /// <returns></returns>
        [QuickStart]
        public static IServiceCollection GetDefaultServiceCollection(
            IWebDriverConfiguration driverConfig)
        {
            return GetDefaultServiceCollection(driverConfig, null);
        }

        /// <summary>
        /// Get a ServiceCollection referencing default implementations using implicit path.
        /// <para>WARNING: This should not be considered stable for use in production projects.
        /// You are STRONGLY advised to use the source code for inspiration rather than using this method directly.</para>
        /// </summary>
        /// <param name="gridUri"></param>
        /// <returns></returns>
        [QuickStart]
        public static IServiceCollection GetDefaultServiceCollection(
            Uri gridUri)
        {
            return GetDefaultServiceCollection(GetDefaultWebDriverConfigurationFromUri(gridUri), null);
        }

        /// <summary>
        /// Get a ServiceCollection referencing default implementations with a defined driver path.
        /// <para>WARNING: This should not be considered stable for use in production projects.
        /// You are STRONGLY advised to use the source code for inspiration rather than using this method directly.</para>
        /// </summary>
        /// <param name="driverPath"></param>
        /// <returns></returns>
        [QuickStart]
        public static IServiceCollection GetDefaultServiceCollection(
            DriverPath driverPath)
        {
            return GetDefaultServiceCollection((IWebDriverConfiguration) null, driverPath);
        }

        /// <summary>
        /// Get a ServiceCollection referencing default implementations with a defined driver path.
        /// <para>WARNING: This should not be considered stable for use in production projects.
        /// You are STRONGLY advised to use the source code for inspiration rather than using this method directly.</para>
        /// </summary>
        /// <param name="gridUri"></param>
        /// <param name="driverPath"></param>
        /// <returns></returns>
        [QuickStart]
        public static IServiceCollection GetDefaultServiceCollection(Uri gridUri, DriverPath driverPath)
        {
            return GetDefaultServiceCollection(GetDefaultWebDriverConfigurationFromUri(gridUri), driverPath);
        }

        /// <summary>
        /// Get a ServiceCollection referencing all Default implementations. 
        /// <para>WARNING: This should not be considered stable for use in production projects.
        /// You are STRONGLY advised to use the source code for inspiration rather than using this method directly.</para>
        /// </summary>
        /// <param name="driverConfig"></param>
        /// <param name="driverPath"></param>
        /// <returns></returns>
        [QuickStart]
        public static IServiceCollection GetDefaultServiceCollection(
            IWebDriverConfiguration driverConfig,
            DriverPath driverPath)
        {
            IServiceCollection services = new ServiceCollection();
            
            services.AddSingleton<IDriverOptionsFactory, DefaultDriverOptionsFactory>();
            services.AddSingleton<IWebDriverReSizer, WebDriverReSizer>();

            if (driverPath != null)
            {
                services.AddSingleton<DriverPath>(driverPath);
            }

            services.AddSingleton(driverConfig ?? WebDriverConfigurationBuilder.Start().Build());
            services.AddSingleton<ILocalWebDriverFactory, DefaultLocalWebDriverFactory>();
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
