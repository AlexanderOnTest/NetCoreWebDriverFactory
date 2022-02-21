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
            base.RemoteWebDriverFactoryWorks(platformType, browser, browserVisibility);
        }
    }
}
