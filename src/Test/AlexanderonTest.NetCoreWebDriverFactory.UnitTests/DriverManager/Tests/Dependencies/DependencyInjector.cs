// <copyright>
// Copyright 2018 Alexander Dunn
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
using AlexanderonTest.NetCoreWebDriverFactory.UnitTests.Settings;
using AlexanderOnTest.NetCoreWebDriverFactory;
using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium;

namespace AlexanderonTest.NetCoreWebDriverFactory.UnitTests.DriverManager.Tests.Dependencies
{
    internal static class DependencyInjector
    {
        public static IServiceProvider GetServiceProvider()
        {
            IWebDriverConfiguration configuration = new WebDriverConfiguration()
                {
                    Browser = Browser.Firefox,
                    GridUri = new Uri("http://192.168.0.200:4444/wd/hub"),
                    IsLocal = true,
                    PlatformType = PlatformType.Windows,
                    WindowSize = WindowSize.Hd,
                    Headless = false
                };

            DriverPath driverPath = new DriverPath(Assembly.GetExecutingAssembly());
            ServiceCollection services = new ServiceCollection();
            services.AddSingleton(configuration);
            services.AddSingleton(driverPath);

            Type webDriverType = TestSettings.UseRealWebDriver
                ? typeof(DefaultWebDriverFactory)
                : typeof(FakeWebDriverFactory);

            services.AddTransient(typeof(IWebDriverFactory), webDriverType);

            services.AddTransient(typeof(IWebDriverManager), typeof(WebDriverManager));
            
            return services.BuildServiceProvider();
        }
    }
}
