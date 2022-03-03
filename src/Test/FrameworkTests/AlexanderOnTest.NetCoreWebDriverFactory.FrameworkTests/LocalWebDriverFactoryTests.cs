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

using System.Runtime.InteropServices;
using AlexanderOnTest.NetCoreWebDriverFactory.Lib.Test;
using NUnit.Framework;

namespace AlexanderOnTest.NetCoreWebDriverFactory.FrameworkTests
{
    [TestFixture]
    public class LocalWebDriverFactoryTests : LocalWebDriverFactoryTestsBase
    {
        private static readonly OSPlatform ThisPlatform = OSPlatform.Windows;
        
        public LocalWebDriverFactoryTests() : base(ThisPlatform)
        { }

        [Category(TestCategories.Local)]
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
        public void LocalWebDriverFactoryWorks(
            Browser browser, 
            BrowserVisibility browserVisibility, 
            BrowserCulture browserCulture = BrowserCulture.Undefined)
        {
            base.LocalWebDriverFactoryWorks(
                browser, 
                browserVisibility, 
                browserCulture == BrowserCulture.Spanish);
        }
        
        [Category(TestCategories.Local)]
        [TestCase(WindowSize.Hd, 1366, 768)]
        [TestCase(WindowSize.Fhd, 1920, 1080)]
        public new void BrowserIsOfRequestedSize(WindowSize windowSize, int expectedWidth, int expectedHeight)
        {
            base.BrowserIsOfRequestedSize(windowSize, expectedWidth, expectedHeight);
        }
        
        [Category(TestCategories.Local)]
        [TestCase(WindowSize.Defined, 1366, 760)]
        [TestCase(WindowSize.Defined, 1280, 1024)]
        public new void CustomSizeBrowserIsOfRequestedSize(WindowSize windowSize, int expectedWidth, int expectedHeight)
        {
            base.CustomSizeBrowserIsOfRequestedSize(windowSize, expectedWidth, expectedHeight);
        }

        [Category(TestCategories.NotSupported)]
        [TestCase(Browser.Safari)]
        public new void RequestingUnsupportedWebDriverThrowsInformativeException(Browser browser)
        {
            base.RequestingUnsupportedWebDriverThrowsInformativeException(browser);
        }

        [Category(TestCategories.NotSupported)]
        [TestCase(Browser.InternetExplorer)]
        [TestCase(Browser.Safari)]
        public new void RequestingUnsupportedHeadlessBrowserThrowsInformativeException(Browser browser)
        {
            base.RequestingUnsupportedHeadlessBrowserThrowsInformativeException(browser);
        }
        
        [Category(TestCategories.NotSupported)]
        [TestCase(Browser.InternetExplorer, BrowserVisibility.OnScreen)]
        [TestCase(Browser.Chrome, BrowserVisibility.Headless)]
        [TestCase(Browser.Edge, BrowserVisibility.Headless)]
        public new void RequestingUnsupportedCulturedBrowserThrowsInformativeException(
            Browser browser,
            BrowserVisibility browserVisibility)
        {
            base.RequestingUnsupportedCulturedBrowserThrowsInformativeException(browser, browserVisibility);
        }
    }
}
