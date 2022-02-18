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
using AlexanderOnTest.NetCoreWebDriverFactory.DependencyInjection;
using AlexanderOnTest.NetCoreWebDriverFactory.Logging;
using AlexanderOnTest.NetCoreWebDriverFactory.WebDriverFactory;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using OpenQA.Selenium;

namespace AlexanderOnTest.NetCoreWebDriverFactory.Lib.Test
{
    [TestFixture]
    public abstract class LocalWebDriverFactoryTestsBase
    {
        private readonly OSPlatform thisPlatform;
        private readonly bool useDotNetFramework;


        protected LocalWebDriverFactoryTestsBase(OSPlatform thisPlatform, bool useDotNetFramework = false)
        {
            this.thisPlatform = thisPlatform;
            if (thisPlatform != OSPlatform.Windows && useDotNetFramework)
            {
                throw new PlatformNotSupportedException(".NET Framework is only available on Microsoft Windows platforms.");
            }
            this.useDotNetFramework = useDotNetFramework;
        }

        private ILocalWebDriverFactory LocalWebDriverFactory { get; set; }
        private IWebDriver Driver { get; set; }

        [OneTimeSetUp]
        public void SetUp()
        {
            Assume.That(() => RuntimeInformation.IsOSPlatform(thisPlatform));

            if (thisPlatform == OSPlatform.Windows)
            {
                TestContext.Progress.WriteLine("Information: These tests are configured to run local Internet Explorer tests against Microsoft Edge.");
            }
            
            IServiceCollection serviceCollection;
            if (useDotNetFramework)
            {
                serviceCollection = ServiceCollectionFactory
                    .GetDefaultServiceCollection();
            }
            else
            {
                serviceCollection = ServiceCollectionFactory
                    .GetDefaultServiceCollection();
            }
            IServiceProvider provider = serviceCollection.BuildServiceProvider();
            LocalWebDriverFactory = provider.GetRequiredService<ILocalWebDriverFactory>();
        }
        
        public void LocalWebDriverFactoryWorks(Browser browser, BrowserVisibility browserVisibility)
        {
            Driver = LocalWebDriverFactory.GetWebDriver(browser, WindowSize.Hd, browserVisibility == BrowserVisibility.Headless);
            Assertions.AssertThatPageCanBeLoaded(Driver);
        }
        
        public void BrowserIsOfRequestedSize(WindowSize windowSize, int expectedWidth, int expectedHeight)
        {
            Driver = LocalWebDriverFactory.GetWebDriver(Browser.Firefox, windowSize, true);
            Assertions.AssertThatBrowserWindowSizeIsCorrect(Driver, expectedWidth, expectedHeight);
        }
        
        public void CustomSizeBrowserIsOfRequestedSize(WindowSize windowSize, int expectedWidth, int expectedHeight)
        {
            Driver = LocalWebDriverFactory.GetWebDriver(
                Browser.Firefox, 
                windowSize, 
                true, 
                new Size(expectedWidth, expectedHeight));
            Assertions.AssertThatBrowserWindowSizeIsCorrect(Driver, expectedWidth, expectedHeight);
        }
        
        public void RequestingUnsupportedWebDriverThrowsInformativeException(Browser browser)
        {
            Assertions.AssertThatRequestingAnUnsupportedBrowserThrowsCorrectException(Act, browser, thisPlatform);

            void Act() => LocalWebDriverFactory.GetWebDriver(browser);
        }
        
        public void RequestingUnsupportedHeadlessBrowserThrowsInformativeException(Browser browser)
        {
            Assertions.AssertThatRequestingAnUnsupportedHeadlessBrowserThrowsCorrectException(Act, browser);

            void Act() => LocalWebDriverFactory.GetWebDriver(browser, WindowSize.Hd, true);
        }



        [TearDown]
        public void Teardown()
        {
            Driver?.Quit();
        }
    }
}
