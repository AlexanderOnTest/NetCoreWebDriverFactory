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
using System.Globalization;
using System.Runtime.InteropServices;
using AlexanderOnTest.NetCoreWebDriverFactory.Config;
using AlexanderOnTest.NetCoreWebDriverFactory.DependencyInjection;
using AlexanderOnTest.NetCoreWebDriverFactory.Utils;
using AlexanderOnTest.NetCoreWebDriverFactory.Utils.Builders;
using AlexanderOnTest.NetCoreWebDriverFactory.Utils.Converters;
using AlexanderOnTest.NetCoreWebDriverFactory.WebDriverFactory;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;
using OpenQA.Selenium;

namespace AlexanderOnTest.NetCoreWebDriverFactory.Lib.Test
{
    public abstract class ConfigurationBasedTestsBase
    {
        private readonly OSPlatform thisPlatform;
        private readonly Uri gridUrl;
        // private readonly bool useDotNetFramework;
        private readonly CultureInfo testCultureInfo = new CultureInfo("es-Es");
        private SizeJsonConverter sizeJsonConverter = new SizeJsonConverter();

        protected ConfigurationBasedTestsBase(OSPlatform thisPlatform, Uri gridUri)
        {
            this.thisPlatform = thisPlatform;
            this.gridUrl = gridUri;
        }

        private IWebDriver Driver { get; set; }
        private IWebDriverFactory WebDriverFactory { get; set; }

        [OneTimeSetUp]
        public void SetUp()
        {
            Assume.That(() => RuntimeInformation.IsOSPlatform(thisPlatform));

            if (thisPlatform == OSPlatform.Windows)
            {
                TestContext.Progress.WriteLine("Information: These tests are configured to run local Internet Explorer tests against Microsoft Edge.");
            }
            
            IServiceCollection serviceCollection = ServiceCollectionFactory.GetDefaultServiceCollection(gridUrl);
            IServiceProvider provider = serviceCollection.BuildServiceProvider();
            WebDriverFactory = provider.GetRequiredService<IWebDriverFactory>();
        }

        public void LocalWebDriverFactoryWorks(
            Browser browser, 
            BrowserVisibility browserVisibility, 
            bool useCulturedBrowser = false)
        {
            CultureInfo requestedCulture = useCulturedBrowser ? testCultureInfo : null;
            IWebDriverConfiguration configuration = WebDriverConfigurationBuilder.Start()
                .WithBrowser(browser)
                .WithHeadless(browserVisibility == BrowserVisibility.Headless)
                .WithLanguageCulture(requestedCulture)
                .Build();
            TestContext.WriteLine($"Configuration = {configuration}");
            Driver = WebDriverFactory.GetWebDriver(configuration);
            Assertions.AssertThatPageCanBeLoadedInExpectedLanguage(Driver, requestedCulture);
        }
        
        public void RemoteWebDriverFactoryWorks(
            PlatformType platformType, 
            Browser browser, 
            BrowserVisibility browserVisibility = BrowserVisibility.OnScreen, 
            bool useCulturedBrowser = false)
        {
            CultureInfo requestedCulture = useCulturedBrowser ? testCultureInfo : null;
            IWebDriverConfiguration configuration = WebDriverConfigurationBuilder.Start()
                .WithBrowser(browser)
                .WithHeadless(browserVisibility == BrowserVisibility.Headless)
                .RunRemotelyOn(gridUrl)
                .WithPlatformType(platformType)
                .WithLanguageCulture(requestedCulture)
                .Build();
            TestContext.WriteLine($"Configuration = {configuration}");
            Driver = WebDriverFactory.GetWebDriver(configuration);
            Assertions.AssertThatPageCanBeLoadedInExpectedLanguage(Driver, requestedCulture);
            Driver.IsRunningHeadless().Should()
                .Be(browserVisibility == BrowserVisibility.Headless, 
                    $"{browserVisibility.ToString()} was requested");
        }
        
        public void BrowserIsOfRequestedSize(WindowSize windowSize, int expectedWidth, int expectedHeight)
        {
            IWebDriverConfiguration configuration = WebDriverConfigurationBuilder.Start()
                .WithBrowser(Browser.Firefox)
                .RunHeadless()
                .WithWindowSize(windowSize)
                .Build();
            TestContext.WriteLine($"Configuration = {configuration}");
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
            TestContext.WriteLine($"Configuration = {configuration}");
            Driver = WebDriverFactory.GetWebDriver(configuration);
            Assertions.AssertThatBrowserWindowSizeIsCorrect(Driver, expectedWidth, expectedHeight);
        }
        
        public void RequestingUnsupportedLocalWebDriverThrowsInformativeException(Browser browser)
        {
            IWebDriverConfiguration configuration = WebDriverConfigurationBuilder.Start()
                .WithBrowser(browser)
                .WithIsLocal(true)
                .Build();
            TestContext.WriteLine($"Configuration = {configuration}");
            void Act() => WebDriverFactory.GetWebDriver(configuration);
            Assertions.AssertThatRequestingAnUnsupportedBrowserThrowsCorrectException(Act, browser, thisPlatform);
        }
        
        public void RequestingUnsupportedLocalHeadlessBrowserThrowsInformativeException(Browser browser)
        {
            IWebDriverConfiguration configuration = WebDriverConfigurationBuilder.Start()
                .WithBrowser(browser)
                .WithIsLocal(true)
                .RunHeadless()
                .Build();

            void Act() => WebDriverFactory.GetWebDriver(configuration);

            Assertions.AssertThatRequestingAnUnsupportedHeadlessBrowserThrowsCorrectException(Act, browser);
        }
        
        public void RequestingUnsupportedLocalCulturedBrowserThrowsAppropriateException(
            Browser browser, 
            BrowserVisibility browserVisibility)
        {
            IWebDriverConfiguration configuration = WebDriverConfigurationBuilder.Start()
                .WithIsLocal(true)
                .WithBrowser(browser)
                .WithHeadless(browserVisibility == BrowserVisibility.Headless)
                .WithLanguageCulture(testCultureInfo)
                .Build();

            void Act() => WebDriverFactory.GetWebDriver(configuration);

            if (browser == Browser.Chrome || browser == Browser.Edge)
            {
                Assertions.AssertThatRequestingAHeadlessCulturedChromiumBrowserThrowsCorrectException(Act, browser);
            }
            else
            {
                Assertions.AssertThatRequestingAnUnsupportedCulturedBrowserThrowsCorrectException(Act, browser);
            }
        }
        
        public void RequestingUnsupportedRemoteWebDriverThrowsInformativeException(
            PlatformType platformType,
            Browser browser)
        {
            IWebDriverConfiguration configuration = WebDriverConfigurationBuilder.Start()
                .RunRemotelyOn(gridUrl)
                .WithPlatformType(platformType)
                .WithBrowser(browser)
                .Build();
            TestContext.WriteLine($"Configuration = {configuration}");
            void Act() => WebDriverFactory.GetWebDriver(configuration);
            Assertions.AssertThatRequestingAnUnsupportedBrowserThrowsCorrectException(Act, browser, platformType);
        }
        
        public void RequestingUnsupportedRemoteHeadlessBrowserThrowsInformativeException(
            PlatformType platformType,
            Browser browser)
        {
            IWebDriverConfiguration configuration = WebDriverConfigurationBuilder.Start()
                .RunRemotelyOn(gridUrl)
                .WithPlatformType(platformType)
                .WithBrowser(browser)
                .RunHeadless()
                .Build();

            void Act() => WebDriverFactory.GetWebDriver(configuration);

            Assertions.AssertThatRequestingAnUnsupportedHeadlessBrowserThrowsCorrectException(Act, browser);
        }
        
        public void RequestingUnsupportedRemoteCulturedBrowserThrowsAppropriateException(
            PlatformType platformType,
            Browser browser, 
            BrowserVisibility browserVisibility)
        {
            IWebDriverConfiguration configuration = WebDriverConfigurationBuilder.Start()
                .RunRemotelyOn(gridUrl)
                .WithPlatformType(platformType)
                .WithBrowser(browser)
                .WithHeadless(browserVisibility == BrowserVisibility.Headless)
                .WithLanguageCulture(testCultureInfo)
                .Build();

            void Act() => WebDriverFactory.GetWebDriver(configuration);

            if (browser == Browser.Chrome || browser == Browser.Edge)
            {
                Assertions.AssertThatRequestingAHeadlessCulturedChromiumBrowserThrowsCorrectException(Act, browser);
            }
            else
            {
                Assertions.AssertThatRequestingAnUnsupportedCulturedBrowserThrowsCorrectException(Act, browser);
            }
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
