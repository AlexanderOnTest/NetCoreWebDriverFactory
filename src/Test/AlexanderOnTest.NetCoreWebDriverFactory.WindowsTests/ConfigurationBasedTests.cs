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
using System.Runtime.InteropServices;
using AlexanderOnTest.NetCoreWebDriverFactory.Lib.Test;
using NUnit.Framework;
using OpenQA.Selenium;

namespace AlexanderOnTest.NetCoreWebDriverFactory.WindowsTests
{
    [TestFixture]
    public class ConfigurationBasedTests : ConfigurationBasedTestsBase
    {
        private static readonly OSPlatform ThisPlatform = OSPlatform.Windows;
        private static readonly string DriverPath = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
        private static readonly Uri GridUrl = new Uri("http://192.168.0.200:4444/wd/hub");

        public ConfigurationBasedTests() : base(ThisPlatform, DriverPath, GridUrl) { }

        [Test]
        [TestCase(Browser.Chrome, BrowserVisibility.OnScreen)]
        [TestCase(Browser.Edge, BrowserVisibility.OnScreen)]
        [TestCase(Browser.Firefox, BrowserVisibility.OnScreen)]
        [TestCase(Browser.InternetExplorer, BrowserVisibility.OnScreen)]
        [TestCase(Browser.Chrome, BrowserVisibility.Headless)]
        [TestCase(Browser.Firefox, BrowserVisibility.Headless)]
        public new void LocalWebDriverFactoryWorks(Browser browser, BrowserVisibility browserVisibility)
        {
            base.LocalWebDriverFactoryWorks(browser, browserVisibility);
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
        public new void RemoteWebDriverFactoryWorks(PlatformType platformType, Browser browser)
        {
            base.RemoteWebDriverFactoryWorks(platformType, browser);
        }

        [Test]
        [TestCase(WindowSize.Hd, 1366, 768)]
        [TestCase(WindowSize.Fhd, 1920, 1080)]
        public new void BrowserIsOfRequestedSize(WindowSize windowSize, int expectedWidth, int expectedHeight)
        {
            base.BrowserIsOfRequestedSize(windowSize, expectedWidth, expectedHeight);
        }

        [Test]
        [TestCase(WindowSize.Custom, 1280, 1024)]
        [TestCase(WindowSize.Custom, 1360, 768)]
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
        [TestCase(Browser.Edge)]
        [TestCase(Browser.InternetExplorer)]
        [TestCase(Browser.Safari)]
        public new void RequestingUnsupportedHeadlessBrowserThrowsInformativeException(Browser browser)
        {
            base.RequestingUnsupportedHeadlessBrowserThrowsInformativeException(browser);
        }
    }
}
