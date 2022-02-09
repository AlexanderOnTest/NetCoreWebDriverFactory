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

namespace AlexanderOnTest.NetCoreWebDriverFactory.MacOsTests
{
    [TestFixture]
    public class RemoteWebDriverFactoryTests : RemoteWebDriverFactoryTestsBase
    {
        private static readonly OSPlatform ThisPlatform = OSPlatform.OSX;
        private static readonly Uri GridUrl = WebDriverSettings.GridUri;

        public RemoteWebDriverFactoryTests() : base(ThisPlatform, GridUrl) { }

        [Test]
        [TestCase(PlatformType.Linux, Browser.Chrome)]
        [TestCase(PlatformType.Linux, Browser.Edge)]
        [TestCase(PlatformType.Linux, Browser.Firefox)]
        [TestCase(PlatformType.Mac, Browser.Chrome)]
        [TestCase(PlatformType.Mac, Browser.Edge)]
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
    }
}