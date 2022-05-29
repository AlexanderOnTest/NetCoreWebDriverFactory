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
    [TestFixture(Category = TestCategories.ConfigBased)]
    public class ConfigurationBasedTests : ConfigurationBasedTestsBase
    {
        private static readonly OSPlatform ThisPlatform = OSPlatform.Windows;
        private static readonly Uri GridUrl = WebDriverSettings.GridUri;

        public ConfigurationBasedTests() : base(ThisPlatform, GridUrl) { }

        [TestCase(Browser.Chrome, BrowserVisibility.OnScreen, BrowserCulture.Undefined)]
        [TestCase(Browser.Edge, BrowserVisibility.OnScreen, BrowserCulture.Undefined)]
        [TestCase(Browser.Firefox, BrowserVisibility.OnScreen, BrowserCulture.Undefined)]
        [TestCase(Browser.InternetExplorer, BrowserVisibility.OnScreen, BrowserCulture.Undefined)]
        [TestCase(Browser.Chrome, BrowserVisibility.Headless, BrowserCulture.Undefined)]
        [TestCase(Browser.Edge, BrowserVisibility.Headless, BrowserCulture.Undefined)]
        [TestCase(Browser.Firefox, BrowserVisibility.Headless, BrowserCulture.Undefined)]
        [TestCase(Browser.Chrome, BrowserVisibility.OnScreen, BrowserCulture.Spanish)]
        [TestCase(Browser.Edge, BrowserVisibility.OnScreen, BrowserCulture.Spanish)]
        [TestCase(Browser.Firefox, BrowserVisibility.OnScreen, BrowserCulture.Spanish)]
        [TestCase(Browser.Firefox, BrowserVisibility.Headless, BrowserCulture.Spanish)]
        [Category(TestCategories.Local)]
        public void LocalWebDriverFactoryWorks(
            Browser browser, 
            BrowserVisibility browserVisibility, 
            BrowserCulture browserCulture = BrowserCulture.Undefined)
        {
            base.LocalWebDriverFactoryWorks(browser, browserVisibility, browserCulture == BrowserCulture.Spanish);
        }

        [TestCase(Browser.Chrome, BrowserVisibility.OnScreen, BrowserCulture.Undefined)]
        [TestCase(Browser.Edge, BrowserVisibility.OnScreen, BrowserCulture.Undefined)]
        [TestCase(Browser.Firefox, BrowserVisibility.OnScreen, BrowserCulture.Undefined)]
        [TestCase(Browser.Chrome, BrowserVisibility.Headless, BrowserCulture.Undefined)]
        [TestCase(Browser.Edge, BrowserVisibility.Headless, BrowserCulture.Undefined)]
        [TestCase(Browser.Firefox, BrowserVisibility.Headless, BrowserCulture.Undefined)]
        [TestCase(Browser.Chrome, BrowserVisibility.OnScreen, BrowserCulture.Spanish)]
        [TestCase(Browser.Edge, BrowserVisibility.OnScreen, BrowserCulture.Spanish)]
        [TestCase(Browser.Firefox, BrowserVisibility.OnScreen, BrowserCulture.Spanish)]
        [TestCase(Browser.Firefox, BrowserVisibility.Headless, BrowserCulture.Spanish)]
        [Category(TestCategories.RemoteLinux)]
        public void RemoteWebDriverFactoryWorksForLinux(
            Browser browser,
            BrowserVisibility browserVisibility,
            BrowserCulture browserCulture)
        {
            base.RemoteWebDriverFactoryWorks(
                PlatformType.Linux, 
                browser, 
                browserVisibility, 
                browserCulture == BrowserCulture.Spanish);
        }
        
        [TestCase(Browser.Chrome, BrowserVisibility.OnScreen, BrowserCulture.Undefined)]
        [TestCase(Browser.Edge, BrowserVisibility.OnScreen, BrowserCulture.Undefined)]
        [TestCase(Browser.Firefox, BrowserVisibility.OnScreen, BrowserCulture.Undefined)]
        [TestCase(Browser.Safari, BrowserVisibility.OnScreen, BrowserCulture.Undefined)]
        [TestCase(Browser.Chrome, BrowserVisibility.Headless, BrowserCulture.Undefined)]
        [TestCase(Browser.Edge, BrowserVisibility.Headless, BrowserCulture.Undefined)]
        [TestCase(Browser.Firefox, BrowserVisibility.Headless, BrowserCulture.Undefined)]
        [TestCase(Browser.Chrome, BrowserVisibility.OnScreen, BrowserCulture.Spanish)]
        [TestCase(Browser.Edge, BrowserVisibility.OnScreen, BrowserCulture.Spanish)]
        [TestCase(Browser.Firefox, BrowserVisibility.OnScreen, BrowserCulture.Spanish)]
        [TestCase(Browser.Firefox, BrowserVisibility.Headless, BrowserCulture.Spanish)]
        [Category(TestCategories.RemoteMacOs)]
        public void RemoteWebDriverFactoryWorksForMacOs(
            Browser browser, 
            BrowserVisibility browserVisibility,
            BrowserCulture browserCulture)
        {
            base.RemoteWebDriverFactoryWorks(
                PlatformType.Mac, 
                browser, 
                browserVisibility, 
                browserCulture == BrowserCulture.Spanish);
        }

        [TestCase(Browser.Chrome, BrowserVisibility.OnScreen, BrowserCulture.Undefined)]
        [TestCase(Browser.Edge, BrowserVisibility.OnScreen, BrowserCulture.Undefined)]
        [TestCase(Browser.Firefox, BrowserVisibility.OnScreen, BrowserCulture.Undefined)]
        [TestCase(Browser.InternetExplorer, BrowserVisibility.OnScreen, BrowserCulture.Undefined)]
        [TestCase(Browser.Chrome, BrowserVisibility.Headless, BrowserCulture.Undefined)]
        [TestCase(Browser.Edge, BrowserVisibility.Headless, BrowserCulture.Undefined)]
        [TestCase(Browser.Firefox, BrowserVisibility.Headless, BrowserCulture.Undefined)]
        [TestCase(Browser.Chrome, BrowserVisibility.OnScreen, BrowserCulture.Spanish)]
        [TestCase(Browser.Edge, BrowserVisibility.OnScreen, BrowserCulture.Spanish)]
        [TestCase(Browser.Firefox, BrowserVisibility.OnScreen, BrowserCulture.Spanish)]
        [TestCase(Browser.Firefox, BrowserVisibility.Headless, BrowserCulture.Spanish)]
        [Category(TestCategories.RemoteWindows)]
        public void RemoteWebDriverFactoryWorksForWindows(
            Browser browser, 
            BrowserVisibility browserVisibility,
            BrowserCulture browserCulture)
        {
            base.RemoteWebDriverFactoryWorks(
                PlatformType.Windows, 
                browser, 
                browserVisibility, 
                browserCulture == BrowserCulture.Spanish);
        }

        [TestCase(WindowSize.Hd, 1366, 768)]
        [TestCase(WindowSize.Fhd, 1920, 1080)]
        [Category(TestCategories.Local)]
        public new void BrowserIsOfRequestedSize(WindowSize windowSize, int expectedWidth, int expectedHeight)
        {
            base.BrowserIsOfRequestedSize(windowSize, expectedWidth, expectedHeight);
        }

        [TestCase(WindowSize.Defined, 1280, 1024)]
        [TestCase(WindowSize.Defined, 1360, 768)]
        [Category(TestCategories.Local)]
        public new void CustomSizeBrowserIsOfRequestedSize(WindowSize windowSize, int expectedWidth, int expectedHeight)
        {
            base.CustomSizeBrowserIsOfRequestedSize(windowSize, expectedWidth, expectedHeight);
        }

        [TestCase(Browser.Safari)]
        [Category(TestCategories.NotSupported)]
        public new void RequestingUnsupportedLocalWebDriverThrowsInformativeException(Browser browser)
        {
            base.RequestingUnsupportedLocalWebDriverThrowsInformativeException(browser);
        }

        [TestCase(Browser.InternetExplorer)]
        [TestCase(Browser.Safari)]
        [Category(TestCategories.NotSupported)]
        public new void RequestingUnsupportedLocalHeadlessBrowserThrowsInformativeException(Browser browser)
        {
            base.RequestingUnsupportedLocalHeadlessBrowserThrowsInformativeException(browser);
        }

        [TestCase(Browser.Chrome, BrowserVisibility.Headless)]
        [TestCase(Browser.Edge, BrowserVisibility.Headless)]
        [TestCase(Browser.InternetExplorer, BrowserVisibility.OnScreen)]
        [Category(TestCategories.NotSupported)]
        public void RequestingUnsupportedLocalCulturedBrowserThrowsInformativeException(
            Browser browser, 
            BrowserVisibility browserVisibility)
        {
            base.RequestingUnsupportedLocalCulturedBrowserThrowsAppropriateException(browser,browserVisibility);
        }

        [TestCase(PlatformType.Linux, Browser.InternetExplorer)]
        [TestCase(PlatformType.Linux, Browser.Safari)]
        [TestCase(PlatformType.Mac, Browser.InternetExplorer)]
        [TestCase(PlatformType.Windows, Browser.Safari)]
        [Category(TestCategories.NotSupported)]
        public new void RequestingUnsupportedRemoteWebDriverThrowsInformativeException(
            PlatformType platformType, 
            Browser browser)
        {
            base.RequestingUnsupportedRemoteWebDriverThrowsInformativeException(platformType, browser);
        }

        [TestCase(PlatformType.Windows, Browser.InternetExplorer)]
        [TestCase(PlatformType.Mac, Browser.Safari)]
        [Category(TestCategories.NotSupported)]
        public new void RequestingUnsupportedRemoteHeadlessBrowserThrowsInformativeException(
            PlatformType platformType, 
            Browser browser)
        {
            base.RequestingUnsupportedRemoteHeadlessBrowserThrowsInformativeException(platformType, browser);
        }

        [TestCase(PlatformType.Linux, Browser.Chrome, BrowserVisibility.Headless)]
        [TestCase(PlatformType.Linux, Browser.Edge, BrowserVisibility.Headless)]
        [TestCase(PlatformType.Mac, Browser.Chrome, BrowserVisibility.Headless)]
        [TestCase(PlatformType.Mac, Browser.Edge, BrowserVisibility.Headless)]
        [TestCase(PlatformType.Mac, Browser.Safari, BrowserVisibility.OnScreen)]
        [TestCase(PlatformType.Windows, Browser.Chrome, BrowserVisibility.Headless)]
        [TestCase(PlatformType.Windows, Browser.Edge, BrowserVisibility.Headless)]
        [TestCase(PlatformType.Windows, Browser.InternetExplorer, BrowserVisibility.OnScreen)]
        [Category(TestCategories.NotSupported)]
        public void RequestingUnsupportedRemoteCulturedBrowserThrowsInformativeException(
            PlatformType platformType,
            Browser browser, 
            BrowserVisibility browserVisibility)
        {
            base.RequestingUnsupportedRemoteCulturedBrowserThrowsAppropriateException(
                platformType,
                browser,
                browserVisibility);
        }
    }
}
