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

using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using AlexanderOnTest.NetCoreWebDriverFactory.Lib.Test;
using NUnit.Framework;

namespace AlexanderOnTest.NetCoreWebDriverFactory.WindowsTests
{
    [TestFixture]
    public class LocalWebDriverFactoryTests : LocalWebDriverFactoryTestsBase
    {
        private static readonly OSPlatform ThisPlatform = OSPlatform.Windows;
        
        public LocalWebDriverFactoryTests() : base(ThisPlatform)
        { }

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
        [TestCase(WindowSize.Hd, 1366, 768)]
        [TestCase(WindowSize.Fhd, 1920, 1080)]
        public new void BrowserIsOfRequestedSize(WindowSize windowSize, int expectedWidth, int expectedHeight)
        {
            base.BrowserIsOfRequestedSize(windowSize, expectedWidth, expectedHeight);
        }

        [Test]
        [TestCase(WindowSize.Defined, 1366, 760)]
        [TestCase(WindowSize.Defined, 1280, 1024)]
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
