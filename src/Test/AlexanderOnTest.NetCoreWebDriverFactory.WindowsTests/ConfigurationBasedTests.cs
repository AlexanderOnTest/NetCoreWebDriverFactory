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
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using AlexanderonTest.NetCoreWebDriverFactory.Lib.Test;
using AlexanderOnTest.NetCoreWebDriverFactory.DriverOptionsFactory;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using OpenQA.Selenium;

namespace AlexanderOnTest.NetCoreWebDriverFactory.WindowsTests
{
    [TestFixture]
    public class ConfigurationBasedTests
    {
        private static readonly OSPlatform ThisPlatform = OSPlatform.Windows;
        private static readonly string DriverPath = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
        private static readonly Uri GridUrl = new Uri("http://192.168.0.200:4444/wd/hub");

        private IWebDriver Driver { get; set; }
        private IWebDriverFactory WebDriverFactory { get; set; }
        private IDriverOptionsFactory DriverOptionsFactory { get; set; }

        [OneTimeSetUp]
        public void SetUp()
        {
            Assume.That(() => RuntimeInformation.IsOSPlatform(ThisPlatform));
            DriverOptionsFactory = new DefaultDriverOptionsFactory();
            WebDriverFactory = new DefaultWebDriverFactory(DriverPath, GridUrl, DriverOptionsFactory);
        }

        [Test]
        [TestCase(Browser.Chrome, BrowserVisibility.OnScreen)]
        [TestCase(Browser.Edge, BrowserVisibility.OnScreen)]
        [TestCase(Browser.Firefox, BrowserVisibility.OnScreen)]
        [TestCase(Browser.InternetExplorer, BrowserVisibility.OnScreen)]
        [TestCase(Browser.Chrome, BrowserVisibility.Headless)]
        [TestCase(Browser.Firefox, BrowserVisibility.Headless)]
        public void LocalWebDriverFactoryWorks(Browser browser, BrowserVisibility browserVisibility)
        {
            IWebDriverConfiguration configuration =
                GetConfiguration(browser, WindowSize.Hd, headless: browserVisibility == BrowserVisibility.Headless);
            Driver = WebDriverFactory.GetWebDriver(configuration);
            Assertions.AssertThatPageCanBeLoaded(Driver);
        }

        [Test]
        [TestCase(PlatformType.Linux, Browser.Chrome)]
        [TestCase(PlatformType.Linux, Browser.Firefox)]
        [TestCase(PlatformType.Mac, Browser.Chrome)]
        [TestCase(PlatformType.Mac, Browser.Firefox)]
        [TestCase(PlatformType.Mac, Browser.Safari)]
        [TestCase(PlatformType.Windows, Browser.Chrome)]
        [TestCase(PlatformType.Windows, Browser.Edge)]
        [TestCase(PlatformType.Windows, Browser.Firefox)]
        [TestCase(PlatformType.Windows, Browser.InternetExplorer)]
        public void RemoteWebDriverFactoryWorks(PlatformType platformType, Browser browser)
        {
            IWebDriverConfiguration configuration =
                GetConfiguration(browser, isLocal: false, platformType: platformType);
            Driver = WebDriverFactory.GetWebDriver(configuration);
            Assertions.AssertThatPageCanBeLoaded(Driver);
        }

        [Test]
        [TestCase(WindowSize.Hd, 1366, 768)]
        [TestCase(WindowSize.Fhd, 1920, 1080)]
        public void BrowserIsOfRequestedSize(WindowSize windowSize, int expectedWidth, int expectedHeight)
        {
            IWebDriverConfiguration configuration =
                GetConfiguration(Browser.Firefox, windowSize: windowSize, headless: true);
            Driver = WebDriverFactory.GetWebDriver(configuration);
            Assertions.AssertThatBrowserWindowSizeIsCorrect(Driver, expectedWidth, expectedHeight);
        }

        [Test]
        [TestCase(Browser.Safari)]
        public void RequestingUnsupportedWebDriverThrowsInformativeException(Browser browser)
        {
            IWebDriverConfiguration configuration =
                GetConfiguration(browser);

            void Act() => WebDriverFactory.GetWebDriver(configuration);
            Assertions.AssertThatRequestingAnUnsupportedBrowserThrowsCorrectException(Act, browser, ThisPlatform);
        }

        [Test]
        [TestCase(Browser.Edge)]
        [TestCase(Browser.InternetExplorer)]
        [TestCase(Browser.Safari)]
        public void RequestingUnsupportedHeadlessBrowserThrowsInformativeException(Browser browser)
        {
            IWebDriverConfiguration configuration =
                GetConfiguration(browser, headless: true);

            void Act() => WebDriverFactory.GetWebDriver(configuration);

            Assertions.AssertThatRequestingAnUnsupportedHeadlessBrowserThrowsCorrectException(Act, browser);
        }

        [TearDown]
        public void Teardown()
        {
            Driver?.Quit();
        }

        // The intention is to have a configuration builder, this allows me to refactor in one place only
        private static IWebDriverConfiguration GetConfiguration(Browser browser, WindowSize windowSize = WindowSize.Hd,
            PlatformType platformType = PlatformType.Any, bool headless = false, bool isLocal = true)
        {
            return new WebDriverConfiguration()
            {
                Browser = browser,
                GridUri = GridUrl,
                Headless = headless,
                IsLocal = isLocal,
                PlatformType = platformType,
                WindowSize = windowSize
            };
        }
    }
}
