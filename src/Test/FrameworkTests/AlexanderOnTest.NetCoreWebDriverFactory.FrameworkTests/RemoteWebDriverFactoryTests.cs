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
    public class RemoteWebDriverFactoryTests : RemoteWebDriverFactoryTestsBase
    {
        private static readonly OSPlatform ThisPlatform = OSPlatform.Windows;
        private static readonly Uri GridUrl = WebDriverSettings.GridUri;

        public RemoteWebDriverFactoryTests() : base(ThisPlatform, GridUrl) { }

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
        public new void RemoteWebDriverFactoryWorksForLinux(
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
        public new void RemoteWebDriverFactoryWorksForMacOs(
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
        public new void RemoteWebDriverFactoryWorksForWindows(
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

        [Category(TestCategories.NotSupported)]
        [TestCase(PlatformType.Linux, Browser.Safari)]
        [TestCase(PlatformType.Windows, Browser.Safari)]
        [TestCase(PlatformType.Linux, Browser.InternetExplorer)]
        [TestCase(PlatformType.Mac, Browser.InternetExplorer)]
        public new void RequestingUnsupportedWebDriverThrowsInformativeException(PlatformType platformType, Browser browser)
        {
            base.RequestingUnsupportedWebDriverThrowsInformativeException(platformType, browser);
        }

        [Category(TestCategories.NotSupported)]
        [TestCase(PlatformType.Windows, Browser.InternetExplorer)]
        [TestCase(PlatformType.Mac, Browser.Safari)]
        public new void RequestingUnsupportedHeadlessBrowserThrowsInformativeException(PlatformType platformType, Browser browser)
        {
            base.RequestingUnsupportedHeadlessBrowserThrowsInformativeException(platformType, browser);
        }

        [Category(TestCategories.NotSupported)]
        [TestCase(PlatformType.Windows, Browser.InternetExplorer,BrowserVisibility.OnScreen)]
        [TestCase(PlatformType.Mac, Browser.Safari, BrowserVisibility.OnScreen)]
        [TestCase(PlatformType.Linux, Browser.Chrome, BrowserVisibility.Headless)]
        [TestCase(PlatformType.Linux, Browser.Edge, BrowserVisibility.Headless)]
        [TestCase(PlatformType.Mac, Browser.Chrome, BrowserVisibility.Headless)]
        [TestCase(PlatformType.Mac, Browser.Edge, BrowserVisibility.Headless)]
        [TestCase(PlatformType.Windows, Browser.Chrome, BrowserVisibility.Headless)]
        [TestCase(PlatformType.Windows, Browser.Edge, BrowserVisibility.Headless)]
        public new void RequestingUnsupportedCulturedBrowserThrowsInformativeException(
            PlatformType platformType, 
            Browser browser,
            BrowserVisibility browserVisibility)
        {
            base.RequestingUnsupportedCulturedBrowserThrowsInformativeException(platformType, browser, browserVisibility);
        }
    }
}
