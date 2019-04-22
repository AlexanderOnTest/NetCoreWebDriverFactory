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
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using AlexanderOnTest.NetCoreWebDriverFactory.DependencyInjection;
using AlexanderOnTest.NetCoreWebDriverFactory.Lib.Test.DI;
using AlexanderOnTest.NetCoreWebDriverFactory.Utils.Builders;
using AlexanderOnTest.NetCoreWebDriverFactory.WebDriverFactory;
using AlexanderOnTest.WebDriverFactoryNunitConfig.TestSettings;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using OpenQA.Selenium;

namespace AlexanderOnTest.NetCoreWebDriverFactory.Lib.Test
{
    [TestFixture]
    public abstract class ConfigurationBasedTestsBase
    {
        private readonly OSPlatform thisPlatform;
        private readonly string driverPath;
        private readonly Uri gridUrl;

        protected ConfigurationBasedTestsBase(OSPlatform thisPlatform, string driverPath)
        {
            this.thisPlatform = thisPlatform;
            this.driverPath = driverPath;
            this.gridUrl = WebDriverSettings.GridUri;
        }

        private IWebDriver Driver { get; set; }
        private IWebDriverFactory WebDriverFactory { get; set; }

        [OneTimeSetUp]
        public void SetUp()
        {
            Assume.That(() => RuntimeInformation.IsOSPlatform(thisPlatform));

            IServiceProvider provider = ServiceCollectionFactory
                .GetDefaultServiceCollection(new DriverPath(Assembly.GetExecutingAssembly()), WebDriverSettings.GridUri)
                .BuildServiceProvider();
            WebDriverFactory = provider.GetRequiredService<IWebDriverFactory>();
        }
        
        public void LocalWebDriverFactoryWorks(Browser browser, BrowserVisibility browserVisibility)
        {
            IWebDriverConfiguration configuration = WebDriverConfigurationBuilder.Start()
                .WithBrowser(browser)
                .WithHeadless(browserVisibility == BrowserVisibility.Headless)
                .Build();
            Driver = WebDriverFactory.GetWebDriver(configuration);
            Assertions.AssertThatPageCanBeLoaded(Driver);
        }
        
        public void RemoteWebDriverFactoryWorks(PlatformType platformType, Browser browser)
        {
            IWebDriverConfiguration configuration = WebDriverConfigurationBuilder.Start()
                .WithBrowser(browser)
                .RunRemotelyOn(gridUrl)
                .WithPlatformType(platformType)
                .Build();
            Driver = WebDriverFactory.GetWebDriver(configuration);
            Assertions.AssertThatPageCanBeLoaded(Driver);
        }
        
        public void BrowserIsOfRequestedSize(WindowSize windowSize, int expectedWidth, int expectedHeight)
        {
            IWebDriverConfiguration configuration = WebDriverConfigurationBuilder.Start()
                .WithBrowser(Browser.Firefox)
                .RunHeadless()
                .WithWindowSize(windowSize)
                .Build();
            Driver = WebDriverFactory.GetWebDriver(configuration);
            Assertions.AssertThatBrowserWindowSizeIsCorrect(Driver, expectedWidth, expectedHeight);
        }
        
        public void CustomSizeBrowserIsOfRequestedSize(WindowSize windowSize, int expectedWidth, int expectedHeight)
        {
            WebDriverConfiguration configuration = WebDriverConfigurationBuilder.Start()
                .WithBrowser(Browser.Firefox)
                .RunHeadless()
                .WithCustomSize(new Size(expectedWidth, expectedHeight))
                .Build();
            Driver = WebDriverFactory.GetWebDriver(configuration);
            Assertions.AssertThatBrowserWindowSizeIsCorrect(Driver, expectedWidth, expectedHeight);
        }
        
        public void RequestingUnsupportedWebDriverThrowsInformativeException(Browser browser)
        {
            IWebDriverConfiguration configuration = WebDriverConfigurationBuilder.Start()
                .WithBrowser(browser)
                .Build();

            void Act() => WebDriverFactory.GetWebDriver(configuration);
            Assertions.AssertThatRequestingAnUnsupportedBrowserThrowsCorrectException(Act, browser, thisPlatform);
        }
        
        public void RequestingUnsupportedHeadlessBrowserThrowsInformativeException(Browser browser)
        {
            IWebDriverConfiguration configuration = WebDriverConfigurationBuilder.Start()
                .WithBrowser(browser)
                .RunHeadless()
                .Build();

            void Act() => WebDriverFactory.GetWebDriver(configuration);

            Assertions.AssertThatRequestingAnUnsupportedHeadlessBrowserThrowsCorrectException(Act, browser);
        }

        [TearDown]
        public void Teardown()
        {
            Driver?.Quit();
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            WebDriverFactory?.Dispose();
        }
    }
}
