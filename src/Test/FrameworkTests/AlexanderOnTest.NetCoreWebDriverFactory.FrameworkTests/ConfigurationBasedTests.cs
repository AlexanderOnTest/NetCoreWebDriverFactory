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
using System.Runtime.InteropServices;
using AlexanderOnTest.NetCoreWebDriverFactory.Lib.Test;
using AlexanderOnTest.WebDriverFactoryNunitConfig.TestSettings;
using NUnit.Framework;
using OpenQA.Selenium;

namespace AlexanderOnTest.NetCoreWebDriverFactory.FrameworkTests
{
    [TestFixture]
    public class ConfigurationBasedTests : ConfigurationBasedTestsBase
    {
        private static readonly OSPlatform ThisPlatform = OSPlatform.Windows;
        private static readonly Uri GridUrl = WebDriverSettings.GridUri;

        public ConfigurationBasedTests() : base(ThisPlatform, GridUrl, true) { }

        [Test]
        [TestCase(Browser.Chrome, BrowserVisibility.OnScreen)]
        [TestCase(Browser.Edge, BrowserVisibility.OnScreen)]
        [TestCase(Browser.Firefox, BrowserVisibility.OnScreen)]
        [TestCase(Browser.InternetExplorer, BrowserVisibility.OnScreen)]
        [TestCase(Browser.Chrome, BrowserVisibility.Headless)]
        [TestCase(Browser.Edge, BrowserVisibility.Headless)]
        [TestCase(Browser.Firefox, BrowserVisibility.Headless)]
        public new void LocalWebDriverFactoryWorks(Browser browser, BrowserVisibility browserVisibility)
        {
            base.LocalWebDriverFactoryWorks(browser, browserVisibility);
        }

        [TestCase(PlatformType.Linux, Browser.Chrome, BrowserVisibility.OnScreen)]
        [TestCase(PlatformType.Linux, Browser.Edge, BrowserVisibility.OnScreen)]
        [TestCase(PlatformType.Linux, Browser.Firefox, BrowserVisibility.OnScreen)]
        [TestCase(PlatformType.Mac, Browser.Chrome, BrowserVisibility.OnScreen)]
        [TestCase(PlatformType.Mac, Browser.Edge, BrowserVisibility.OnScreen)]
        [TestCase(PlatformType.Mac, Browser.Firefox, BrowserVisibility.OnScreen)]
        [TestCase(PlatformType.Mac, Browser.Safari, BrowserVisibility.OnScreen)]
        [TestCase(PlatformType.Windows, Browser.Chrome, BrowserVisibility.OnScreen)]
        [TestCase(PlatformType.Windows, Browser.Edge, BrowserVisibility.OnScreen)]
        [TestCase(PlatformType.Windows, Browser.Firefox, BrowserVisibility.OnScreen)]
        [TestCase(PlatformType.Windows, Browser.InternetExplorer, BrowserVisibility.OnScreen)]
        [TestCase(PlatformType.Linux, Browser.Chrome, BrowserVisibility.Headless)]
        [TestCase(PlatformType.Linux, Browser.Edge, BrowserVisibility.Headless)]
        [TestCase(PlatformType.Linux, Browser.Firefox, BrowserVisibility.Headless)]
        [TestCase(PlatformType.Mac, Browser.Chrome, BrowserVisibility.Headless)]
        [TestCase(PlatformType.Mac, Browser.Edge, BrowserVisibility.Headless)]
        [TestCase(PlatformType.Mac, Browser.Firefox, BrowserVisibility.Headless)]
        [TestCase(PlatformType.Windows, Browser.Chrome, BrowserVisibility.Headless)]
        [TestCase(PlatformType.Windows, Browser.Edge, BrowserVisibility.Headless)]
        [TestCase(PlatformType.Windows, Browser.Firefox, BrowserVisibility.Headless)]
        public new void RemoteWebDriverFactoryWorks(
            PlatformType platformType, 
            Browser browser,
            BrowserVisibility browserVisibility)
        {
            TestContext.WriteLine($"mode = {browserVisibility}");
            base.RemoteWebDriverFactoryWorks(platformType, browser, browserVisibility);
        }

        [Test]
        [TestCase(WindowSize.Hd, 1366, 768)]
        [TestCase(WindowSize.Fhd, 1920, 1080)]
        public new void BrowserIsOfRequestedSize(WindowSize windowSize, int expectedWidth, int expectedHeight)
        {
            base.BrowserIsOfRequestedSize(windowSize, expectedWidth, expectedHeight);
        }

        [Test]
        [TestCase(WindowSize.Defined, 1280, 1024)]
        [TestCase(WindowSize.Defined, 1360, 768)]
        public new void CustomSizeBrowserIsOfRequestedSize(WindowSize windowSize, int expectedWidth, int expectedHeight)
        {
            base.CustomSizeBrowserIsOfRequestedSize(windowSize, expectedWidth, expectedHeight);
        }

        [Test]
        [TestCase(Browser.Safari)]
        public new void RequestingUnsupportedWebDriverThrowsInformativeException(Browser browser)
        {
            base.RequestingUnsupportedWebDriverThrowsInformativeException(browser);
        }

        [Test]
        [TestCase(Browser.InternetExplorer)]
        [TestCase(Browser.Safari)]
        public new void RequestingUnsupportedHeadlessBrowserThrowsInformativeException(Browser browser)
        {
            base.RequestingUnsupportedHeadlessBrowserThrowsInformativeException(browser);
        }
    }
}
