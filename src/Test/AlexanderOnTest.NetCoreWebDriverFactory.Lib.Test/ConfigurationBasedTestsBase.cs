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
using AlexanderOnTest.NetCoreWebDriverFactory.Utils.Builders;
using AlexanderOnTest.NetCoreWebDriverFactory.WebDriverFactory;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using OpenQA.Selenium;

namespace AlexanderOnTest.NetCoreWebDriverFactory.Lib.Test
{
    [TestFixture]
    public abstract class ConfigurationBasedTestsBase
    {
        private readonly OSPlatform thisPlatform;
        private readonly Uri gridUrl;
        private readonly bool useDotNetFramework;

        protected ConfigurationBasedTestsBase(OSPlatform thisPlatform, Uri gridUri, bool useDotNetFramework = false)
        {
            this.thisPlatform = thisPlatform;
            if (thisPlatform != OSPlatform.Windows && useDotNetFramework)
            {
                throw new PlatformNotSupportedException(".NET Framework is only available on Microsoft Windows platforms.");
            }
            this.useDotNetFramework = useDotNetFramework;
            this.gridUrl = gridUri;
        }

        private IWebDriver Driver { get; set; }
        private IWebDriverFactory WebDriverFactory { get; set; }

        [OneTimeSetUp]
        public void SetUp()
        {
            Assume.That(() => RuntimeInformation.IsOSPlatform(thisPlatform));
            
            IServiceCollection serviceCollection;
            if (useDotNetFramework)
            {
                serviceCollection = ServiceCollectionFactory
                    .GetDefaultServiceCollection();
            }
            else
            {
                serviceCollection = ServiceCollectionFactory
                    .GetDefaultServiceCollection(true);
            }
            IServiceProvider provider = serviceCollection.BuildServiceProvider();

            WebDriverFactory = provider.GetRequiredService<IWebDriverFactory>();
        }

        public void LocalWebDriverFactoryWorks(Browser browser, BrowserVisibility browserVisibility)
        {
            IWebDriverConfiguration configuration = WebDriverConfigurationBuilder.Start()
                .WithBrowser(browser)
                .WithHeadless(browserVisibility == BrowserVisibility.Headless)
                .Build();
            Driver = WebDriverFactory.GetWebDriver(configuration);
            Assertions.AssertThatPageCanBeLoaded(Driver);
        }
        
        public void RemoteWebDriverFactoryWorks(PlatformType platformType, Browser browser)
        {
            IWebDriverConfiguration configuration = WebDriverConfigurationBuilder.Start()
                .WithBrowser(browser)
                .RunRemotelyOn(gridUrl)
                .WithPlatformType(platformType)
                .Build();
            Driver = WebDriverFactory.GetWebDriver(configuration);
            Assertions.AssertThatPageCanBeLoaded(Driver);
        }
        
        public void BrowserIsOfRequestedSize(WindowSize windowSize, int expectedWidth, int expectedHeight)
        {
            IWebDriverConfiguration configuration = WebDriverConfigurationBuilder.Start()
                .WithBrowser(Browser.Firefox)
                .RunHeadless()
                .WithWindowSize(windowSize)
                .Build();
            Driver = WebDriverFactory.GetWebDriver(configuration);
            Assertions.AssertThatBrowserWindowSizeIsCorrect(Driver, expectedWidth, expectedHeight);
        }
        
        public void CustomSizeBrowserIsOfRequestedSize(WindowSize windowSize, int expectedWidth, int expectedHeight)
        {
            WebDriverConfiguration configuration = WebDriverConfigurationBuilder.Start()
                .WithBrowser(Browser.Firefox)
                .RunHeadless()
                .WithCustomSize(new Size(expectedWidth, expectedHeight))
                .Build();
            Driver = WebDriverFactory.GetWebDriver(configuration);
            Assertions.AssertThatBrowserWindowSizeIsCorrect(Driver, expectedWidth, expectedHeight);
        }
        
        public void RequestingUnsupportedWebDriverThrowsInformativeException(Browser browser)
        {
            IWebDriverConfiguration configuration = WebDriverConfigurationBuilder.Start()
                .WithBrowser(browser)
                .Build();

            void Act() => WebDriverFactory.GetWebDriver(configuration);
            Assertions.AssertThatRequestingAnUnsupportedBrowserThrowsCorrectException(Act, browser, thisPlatform);
        }
        
        public void RequestingUnsupportedHeadlessBrowserThrowsInformativeException(Browser browser)
        {
            IWebDriverConfiguration configuration = WebDriverConfigurationBuilder.Start()
                .WithBrowser(browser)
                .RunHeadless()
                .Build();

            void Act() => WebDriverFactory.GetWebDriver(configuration);

            Assertions.AssertThatRequestingAnUnsupportedHeadlessBrowserThrowsCorrectException(Act, browser);
        }

        [TearDown]
        public void Teardown()
        {
            Driver?.Quit();
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            WebDriverFactory?.Dispose();
        }
    }
}
