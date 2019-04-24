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
using System.IO;
using System.Reflection;
using AlexanderOnTest.NetCoreWebDriverFactory.DependencyInjection;
using AlexanderOnTest.NetCoreWebDriverFactory.DriverManager;
using AlexanderOnTest.NetCoreWebDriverFactory.UnitTests.DependencyInjection;
using AlexanderOnTest.NetCoreWebDriverFactory.UnitTests.DriverManager.Tests.Dependencies;
using AlexanderOnTest.NetCoreWebDriverFactory.WebDriverFactory;
using FluentAssertions;
using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NUnit.Framework;
using OpenQA.Selenium;

namespace AlexanderOnTest.NetCoreWebDriverFactory.UnitTests.DriverManager.Tests
{
    [Category("CI")]
    public class WebDriverManagerTests
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly bool IsDebugEnabled = Logger.IsDebugEnabled;

        private IWebDriver DriverOne { get; set; }

        private IWebDriver DriverTwo { get; set; }

        private IWebDriverManager WebDriverManager { get; set; }

        [OneTimeSetUp]
        public void Prepare()
        {
            ILoggerRepository logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo(Path.Combine(TestContext.CurrentContext.TestDirectory, "log4net.config")));

            // Use the in-built IoC functionality
            IServiceCollection services = ServiceCollectionFactory.GetDefaultServiceCollection((IWebDriverConfiguration) null);
            services.Replace(ServiceDescriptor.Singleton<IWebDriverFactory, FakeWebDriverFactory>());
            IServiceProvider provider = services.BuildServiceProvider();

            // Or use Scrutor
            //IServiceProvider provider = DependencyInjector.GetScannedServiceProvider();

            // or use the original IoC container code
            //IServiceProvider provider = DependencyInjector.GetDefinedServiceProvider();

            WebDriverManager = provider.GetService<IWebDriverManager>();
        }

        /// <summary>
        /// Get() returns a new driver.
        /// </summary>
        [Test]
        public void GetCreatesNewDriver()
        {
            if (IsDebugEnabled)
            {
                Logger.Debug($"Starting {TestContext.CurrentContext.Test.Name}");
            }
            DriverOne = WebDriverManager.Get();
            DriverOne.Should().NotBeNull();
        }

        /// <summary>
        /// A second call to Get() returns the same instance.
        /// </summary>
        [Test]
        public void GetReturnsTheSameDriver()
        {
            if (IsDebugEnabled)
            {
                Logger.Debug($"Starting {TestContext.CurrentContext.Test.Name}");
            }
            DriverOne = WebDriverManager.Get();
            DriverTwo = WebDriverManager.Get();
            DriverTwo.Should().BeSameAs(DriverOne);
        }

        /// <summary>
        /// A call to GetAdditionalWebDriver() returns a different instance.
        /// </summary>
        [Test]
        public void GetAdditionalWebDriverReturnsAnotherInstance()
        {
            if (IsDebugEnabled)
            {
                Logger.Debug($"Starting {TestContext.CurrentContext.Test.Name}");
            }
            DriverOne = WebDriverManager.Get();
            DriverTwo = WebDriverManager.GetAdditionalWebDriver();
            DriverTwo.Should().NotBeSameAs(DriverOne);
        }

        /// <summary>
        /// Calling GetAdditionalWebDriver() first also returns different instances
        /// </summary>
        [Test]
        public void GetAdditionalWebDriverDoesNotCreateSingleton()
        {
            if (IsDebugEnabled)
            {
                Logger.Debug($"Starting {TestContext.CurrentContext.Test.Name}");
            }
            // Reverse of the above, check that get additional first does not create a singleton instance
            DriverTwo = WebDriverManager.GetAdditionalWebDriver();
            DriverOne = WebDriverManager.Get();
            DriverTwo.Should().NotBeSameAs(DriverOne);
        }

        /// <summary>
        /// Calling Quit() has quit the WebDriver instance
        /// </summary>
        [Test]
        public void QuitHasClosedTheWebDriver()
        {
            if (IsDebugEnabled)
            {
                Logger.Debug($"Starting {TestContext.CurrentContext.Test.Name}");
            }
            DriverOne = WebDriverManager.Get();
            WebDriverManager.Quit();
            Action act = () => DriverOne.Manage().Window.FullScreen();
            act.Should().ThrowExactly<WebDriverException>();
        }

        /// <summary>
        /// Calling Get() after Quit() returns a new instance
        /// </summary>
        [Test]
        public void AfterQuittingANewDriverIsCreated()
        {
            if (IsDebugEnabled)
            {
                Logger.Debug($"Starting {TestContext.CurrentContext.Test.Name}");
            }
            DriverOne = WebDriverManager.Get();
            WebDriverManager.Quit();
            DriverTwo = WebDriverManager.Get();
            DriverTwo.Should().NotBeSameAs(DriverOne);
        }

        /// <summary>
        /// Calling Quit() quits only the singleton driver.
        /// </summary>
        [Test]
        public void QuitOnlyQuitsTheSingletonWebDriver()
        {
            if (IsDebugEnabled)
            {
                Logger.Debug($"Starting {TestContext.CurrentContext.Test.Name}");
            }
            DriverOne = WebDriverManager.Get();
            DriverTwo = WebDriverManager.GetAdditionalWebDriver();
            
            WebDriverManager.Quit();

            Action actOne = () => DriverOne.Manage().Window.FullScreen();
            actOne.Should().ThrowExactly<WebDriverException>();

            Action actTwo = () => DriverTwo.Manage().Window.FullScreen();
            actTwo.Should().NotThrow<WebDriverException>();
        }

        [TearDown]
        public void Teardown()
        {
            DriverOne = WebDriverManager.Quit();
            DriverTwo?.Quit();
        }
    }
}
