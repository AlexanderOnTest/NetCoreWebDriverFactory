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
    /// You are STRONGLY advised to use the source code for inspiration rather than using these method directly.</para>
    /// </summary>
    [QuickStart]
    public static class ServiceCollectionFactory
    {
        /// <summary>
        /// Get a ServiceCollection referencing default implementations using implicit path.
        /// Use for .NET Framework projects and projects where the driver executables are on the System Path.
        /// <para>WARNING: This should not be considered stable for use in production projects.
        /// You are STRONGLY advised to use the source code for inspiration rather than using this method directly.</para>
        /// </summary>
        /// <returns></returns>
        [QuickStart]
        public static IServiceCollection GetDefaultServiceCollection()
        {
            return GetDefaultServiceCollection(null, (IWebDriverConfiguration) null);
        }

        /// <summary>
        /// Get a ServiceCollection referencing default implementations using implicit path.
        /// Use for .NET Framework projects and projects where the driver executables are on the System Path.
        /// <para>WARNING: This should not be considered stable for use in production projects.
        /// You are STRONGLY advised to use the source code for inspiration rather than using this method directly.</para>
        /// </summary>
        /// <param name="driverConfig"></param>
        /// <returns></returns>
        [QuickStart]
        public static IServiceCollection GetDefaultServiceCollection(
            IWebDriverConfiguration driverConfig)
        {
            return GetDefaultServiceCollection(null, driverConfig);
        }

        /// <summary>
        /// Get a ServiceCollection referencing default implementations using implicit path.
        /// Use for .NET Framework projects and projects where the driver executables are on the System Path.
        /// <para>WARNING: This should not be considered stable for use in production projects.
        /// You are STRONGLY advised to use the source code for inspiration rather than using this method directly.</para>
        /// </summary>
        /// <param name="gridUri"></param>
        /// <returns></returns>
        [QuickStart]
        public static IServiceCollection GetDefaultServiceCollection(
            Uri gridUri)
        {
            return GetDefaultServiceCollection(null, GetDefaultWebDriverConfigurationFromUri(gridUri));
        }

        /// <summary>
        /// Experimental - Get a ServiceCollection referencing default implementations.
        /// Use true for .NET Core projects with driver executables from nuget packages / false for .NET Framework using nuget or driver executables on the System Path.
        /// <para>WARNING: This should not be considered stable for use in production projects.
        /// You are STRONGLY advised to use the source code for inspiration rather than using this method directly.</para>
        /// </summary>
        /// <param name="useDefaultDotNetCoreDriverPath"> </param>
        /// <returns></returns>
        [QuickStart]
        [Experimental("The calling assembly must have the required driver nuget packages installed.")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static IServiceCollection GetDefaultServiceCollection(
            bool useDefaultDotNetCoreDriverPath)
        {
            return useDefaultDotNetCoreDriverPath? 
                GetDefaultServiceCollection(new DriverPath(Assembly.GetCallingAssembly()), (IWebDriverConfiguration) null) :
                GetDefaultServiceCollection(null, (IWebDriverConfiguration) null);
        }

        /// <summary>
        /// Experimental - Get a ServiceCollection referencing default implementations.
        /// Use true for .NET Core projects with driver executables from nuget packages / false for .NET Framework using nuget or driver executables on the System Path.
        /// WARNING: This should not be considered stable for use in production projects.
        /// You are STRONGLY advised to use the source code for inspiration rather than using this method directly.
        /// </summary>
        /// <param name="useDefaultDotNetCoreDriverPath"></param>
        /// <param name="gridUri"></param>
        /// <returns></returns>
        [QuickStart]
        [Experimental("The calling assembly must have the required driver nuget packages installed.")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static IServiceCollection GetDefaultServiceCollection(
            bool useDefaultDotNetCoreDriverPath,
            Uri gridUri)
        {
            return useDefaultDotNetCoreDriverPath ? 
                GetDefaultServiceCollection(new DriverPath(Assembly.GetCallingAssembly()), GetDefaultWebDriverConfigurationFromUri(gridUri)) :
                GetDefaultServiceCollection(null, GetDefaultWebDriverConfigurationFromUri(gridUri));
        }

        /// <summary>
        /// Experimental - Get a ServiceCollection referencing default implementations.
        /// Use true for .NET Core projects with driver executables from nuget packages / false for .NET Framework using nuget or driver executables on the System Path.
        /// <para>WARNING: This should not be considered stable for use in production projects.
        /// You are STRONGLY advised to use the source code for inspiration rather than using this method directly.</para>
        /// </summary>
        /// <param name="useDefaultDotNetCoreDriverPath"></param>
        /// <param name="driverConfig"></param>
        /// <returns></returns>
        [QuickStart]
        [Experimental("The calling assembly must have the required driver nuget packages installed.")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static IServiceCollection GetDefaultServiceCollection(
            bool useDefaultDotNetCoreDriverPath,
            IWebDriverConfiguration driverConfig)
        {
            return useDefaultDotNetCoreDriverPath ? 
                GetDefaultServiceCollection(new DriverPath(Assembly.GetCallingAssembly()), driverConfig) :
                GetDefaultServiceCollection(null, driverConfig);
        }

        /// <summary>
        /// Get a ServiceCollection referencing default implementations with a defined driver path.
        /// Use for .NET Core projects with driver executables from nuget packages.
        /// <para>WARNING: This should not be considered stable for use in production projects.
        /// You are STRONGLY advised to use the source code for inspiration rather than using this method directly.</para>
        /// </summary>
        /// <param name="driverPath"></param>
        /// <returns></returns>
        [QuickStart]
        public static IServiceCollection GetDefaultServiceCollection(
            DriverPath driverPath)
        {
            return GetDefaultServiceCollection(driverPath, (IWebDriverConfiguration) null);
        }

        /// <summary>
        /// Get a ServiceCollection referencing default implementations with a defined driver path.
        /// Use for .NET Core projects with driver executables from nuget packages.
        /// <para>WARNING: This should not be considered stable for use in production projects.
        /// You are STRONGLY advised to use the source code for inspiration rather than using this method directly.</para>
        /// </summary>
        /// <param name="driverPath"></param>
        /// <param name="gridUri"></param>
        /// <returns></returns>
        [QuickStart]
        public static IServiceCollection GetDefaultServiceCollection(
            DriverPath driverPath,
            Uri gridUri)
        {
            return GetDefaultServiceCollection(driverPath, GetDefaultWebDriverConfigurationFromUri(gridUri));
        }

        /// <summary>
        /// Get a ServiceCollection referencing all Default implementations. 
        /// <para>WARNING: This should not be considered stable for use in production projects.
        /// You are STRONGLY advised to use the source code for inspiration rather than using this method directly.</para>
        /// </summary>
        /// <param name="driverPath"></param>
        /// <param name="driverConfig"></param>
        /// <returns></returns>
        [QuickStart]
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
