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
using System.Drawing;
using System.Runtime.InteropServices;
using AlexanderOnTest.NetCoreWebDriverFactory.DriverOptionsFactory;
using AlexanderOnTest.NetCoreWebDriverFactory.WebDriverFactory;
using NUnit.Framework;
using OpenQA.Selenium;

namespace AlexanderOnTest.NetCoreWebDriverFactory.Lib.Test
{
    [TestFixture]
    public abstract class StaticWebDriverFactoryTestsBase
    {
        private readonly OSPlatform thisPlatform;
        private readonly string driverPath;
        private readonly Uri gridUrl;

        protected StaticWebDriverFactoryTestsBase(OSPlatform thisPlatform, string driverPath, Uri gridUrl)
        {
            this.thisPlatform = thisPlatform;
            this.driverPath = driverPath;
            this.gridUrl = gridUrl;
        }

        private IWebDriver Driver { get; set; }

        [OneTimeSetUp]
        public void CheckForValidPlatform()
        {
            Assume.That(() => RuntimeInformation.IsOSPlatform(thisPlatform)) ;
        }

        public void LocalWebDriverFactoryWorks(Browser browser, BrowserVisibility headless = BrowserVisibility.OnScreen)
        {
            Driver = StaticWebDriverFactory.GetLocalWebDriver(
                browser, 
                browser == Browser.Edge? null : driverPath, 
                headless == BrowserVisibility.Headless);
            Assertions.AssertThatPageCanBeLoaded(Driver);
        }

        public void RemoteWebDriverFactoryWorks(PlatformType platformType, Browser browser)
        {
            Driver = StaticWebDriverFactory.GetRemoteWebDriver(browser, gridUrl, platformType);
            Assertions.AssertThatPageCanBeLoaded(Driver);
        }
        
        public void BrowserIsOfRequestedSize(WindowSize windowSize, int expectedWidth, int expectedHeight)
        {
            Driver = StaticWebDriverFactory.GetLocalWebDriver(
                StaticDriverOptionsFactory.GetFirefoxOptions(true), 
                driverPath,
                windowSize);
            Assertions.AssertThatBrowserWindowSizeIsCorrect(Driver, expectedWidth, expectedHeight);
        }

        public void CustomSizeBrowserIsOfRequestedSize(WindowSize windowSize, int expectedWidth, int expectedHeight)
        {
            Driver = StaticWebDriverFactory.GetLocalWebDriver(
                StaticDriverOptionsFactory.GetFirefoxOptions(true),
                driverPath,
                windowSize,
                new Size(expectedWidth, expectedHeight));
            Assertions.AssertThatBrowserWindowSizeIsCorrect(Driver, expectedWidth, expectedHeight);
        }

        public void RequestingUnsupportedWebDriverThrowsInformativeException(Browser browser)
        {
            Assertions.AssertThatRequestingAnUnsupportedBrowserThrowsCorrectException(Act, browser, thisPlatform);

            void Act() => StaticWebDriverFactory.GetLocalWebDriver(browser);
        }

        public void RequestingUnsupportedHeadlessBrowserThrowsInformativeException(Browser browser)
        {
            Assertions.AssertThatRequestingAnUnsupportedHeadlessBrowserThrowsCorrectException(Act, browser);

            void Act() => StaticWebDriverFactory.GetLocalWebDriver(browser, driverPath, true);
        }
        
        [TearDown]
        public void Teardown()
        {
            Driver?.Quit();
        }
    }
}
