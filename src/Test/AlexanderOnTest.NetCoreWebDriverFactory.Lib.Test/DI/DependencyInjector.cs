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
using AlexanderOnTest.NetCoreWebDriverFactory.DriverManager;
using AlexanderOnTest.NetCoreWebDriverFactory.DriverOptionsFactory;
using AlexanderOnTest.NetCoreWebDriverFactory.Utils;
using AlexanderOnTest.NetCoreWebDriverFactory.WebDriverFactory;
using AlexanderOnTest.WebDriverFactoryNunitConfig.TestSettings;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace AlexanderOnTest.NetCoreWebDriverFactory.Lib.Test.DI
{
    public static class DependencyInjector
    {
        public static IServiceProvider GetServiceProvider()
        {
            ServiceCollection services = new ServiceCollection();
            services.AddSingleton(typeof(IWebDriverConfiguration), WebDriverSettings.WebDriverConfiguration);
            services.AddSingleton(new DriverPath(Assembly.GetExecutingAssembly()));

            services.AddSingleton(typeof(IDriverOptionsFactory), typeof(DefaultDriverOptionsFactory));
            services.AddSingleton(typeof(IWebDriverReSizer), typeof(WebDriverReSizer));
            services.AddSingleton(typeof(ILocalWebDriverFactory), typeof(DefaultLocalWebDriverFactory));
            services.AddSingleton(typeof(IRemoteWebDriverFactory), typeof(DefaultRemoteWebDriverFactory));
            services.AddSingleton(typeof(IWebDriverFactory), typeof(DefaultWebDriverFactory));
            services.AddSingleton(typeof(IWebDriverManager), typeof(WebDriverManager));

            return services.BuildServiceProvider();
        }

        public static IServiceProvider GetScannedServiceProvider()
        {
            ServiceCollection services = new ServiceCollection();
            services.AddSingleton(typeof(IWebDriverConfiguration), WebDriverSettings.WebDriverConfiguration);
            services.AddSingleton(new DriverPath(Assembly.GetExecutingAssembly()));

            // Scrutor guide -https://andrewlock.net/using-scrutor-to-automatically-register-your-services-with-the-asp-net-core-di-container/
            // Select default implementations for interfaces where there are multiples
            services.Scan(scan => scan
                .FromAssemblyOf<WebDriverConfiguration>()
                .AddClasses(classes => classes.Where(type => type.Name.StartsWith("Default")))
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsImplementedInterfaces()
                .WithSingletonLifetime());

            // Select first (only?) implementation for other interfaces
            services.Scan(scan => scan
                .FromAssemblyOf<WebDriverConfiguration>()
                .AddClasses()
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsMatchingInterface()
                .WithSingletonLifetime());

            return services.BuildServiceProvider();
        }
    }
}
