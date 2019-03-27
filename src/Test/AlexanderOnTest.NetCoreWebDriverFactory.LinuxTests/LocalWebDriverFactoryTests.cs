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
using AlexanderOnTest.NetCoreWebDriverFactory.Lib.Test;
using NUnit.Framework;

namespace AlexanderOnTest.NetCoreWebDriverFactory.LinuxTests
{
    [TestFixture]
    public class LocalWebDriverFactoryTests : LocalWebDriverFactoryTestsBase
    {
        private static readonly OSPlatform ThisPlatform = OSPlatform.Linux;
        private static readonly string DriverPath = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);

        public LocalWebDriverFactoryTests() : base(ThisPlatform, DriverPath) { }

        [Test]
        [Category("CI")]
        [TestCase(WindowSize.Hd, 1366, 768)]
        [TestCase(WindowSize.Fhd, 1920, 1080)]
        public new void BrowserIsOfRequestedSize(WindowSize windowSize, int expectedWidth, int expectedHeight)
        {
            base.BrowserIsOfRequestedSize(windowSize, expectedWidth, expectedHeight);
        }

        [Test]
        [Category("CI")]
        [TestCase(WindowSize.Custom, 1366, 760)]
        [TestCase(WindowSize.Custom, 1280, 1024)]
        public new void CustomSizeBrowserIsOfRequestedSize(WindowSize windowSize, int expectedWidth, int expectedHeight)
        {
            base.CustomSizeBrowserIsOfRequestedSize(windowSize, expectedWidth, expectedHeight);
        }

        [Test]
        [Category("CI")]
        [TestCase(Browser.Edge)]
        [TestCase(Browser.InternetExplorer)]
        [TestCase(Browser.Safari)]
        public new void RequestingUnsupportedWebDriverThrowsInformativeException(Browser browser)
        {
            base.RequestingUnsupportedWebDriverThrowsInformativeException(browser);
        }

        [Test]
        [Category("CI")]
        [TestCase(Browser.Edge)]
        [TestCase(Browser.InternetExplorer)]
        [TestCase(Browser.Safari)]
        public new void RequestingUnsupportedHeadlessBrowserThrowsInformativeException(Browser browser)
        {
            base.RequestingUnsupportedHeadlessBrowserThrowsInformativeException(browser);
        }

        [Test]
        [TestCase(Browser.Chrome, BrowserVisibility.OnScreen)]
        [TestCase(Browser.Chrome, BrowserVisibility.Headless)]
        [TestCase(Browser.Firefox, BrowserVisibility.OnScreen)]
        public void LocalWebDriverCallWorks(Browser browser, BrowserVisibility visibility = BrowserVisibility.OnScreen)
        {
            LocalWebDriverFactoryWorks(browser, visibility);
        }
        
        [Test]
        [Category("CI")]
        [TestCase(BrowserVisibility.Headless)]
        public void LocalHeadlessFirefoxDriverCallWorks(BrowserVisibility visibility = BrowserVisibility.OnScreen)
        {
            LocalWebDriverFactoryWorks(Browser.Firefox, visibility);
        }
    }
}
