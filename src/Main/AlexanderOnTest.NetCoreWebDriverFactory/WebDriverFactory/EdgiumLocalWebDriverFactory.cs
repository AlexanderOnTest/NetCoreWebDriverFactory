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
using AlexanderOnTest.NetCoreWebDriverFactory.DriverOptionsFactory;
using AlexanderOnTest.NetCoreWebDriverFactory.Logging;
using AlexanderOnTest.NetCoreWebDriverFactory.Utils;
using Microsoft.Edge.SeleniumTools;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Safari;

namespace AlexanderOnTest.NetCoreWebDriverFactory.WebDriverFactory
{
    /// <summary>
    /// Default LocalWebDriverFactory implementation for .NET Core projects using Chromium based Microsoft Edge.
    /// Note: As The Chrome based EdgeDriver is not currently available in a nuget package, it MUST be on the System Path.
    /// </summary>
    public class EdgiumLocalWebDriverFactory : LocalWebDriverFactoryBase
    {
        private static readonly ILog Logger = LogProvider.For<DefaultWebDriverFactory>();
        private static readonly bool IsDebugEnabled = Logger.IsDebugEnabled();

        /// <summary>
        /// Return a DriverFactory instance for use in .NET Core projects.
        /// Try using installedDriverPath = "Path.GetDirectoryName(Assembly.GetCallingAssembly().Location)" when running from .NET core projects.
        /// </summary>
        /// <param name="driverOptionsFactory"></param>
        /// <param name="installedDriverPath"></param>
        /// <param name="webDriverReSizer"></param>
        public EdgiumLocalWebDriverFactory(IDriverOptionsFactory driverOptionsFactory, string installedDriverPath, IWebDriverReSizer webDriverReSizer) : base(driverOptionsFactory, installedDriverPath, webDriverReSizer)
        {
        }

        /// <summary>
        /// Return a DriverFactory instance for use in .NET Core projects.
        /// Try using driverPath = new DriverPath(Assembly.GetCallingAssembly()) when testing locally from .NET core projects.
        /// </summary>
        /// <param name="driverOptionsFactory"></param>
        /// <param name="driverPath"></param>
        /// <param name="webDriverReSizer"></param>
        public EdgiumLocalWebDriverFactory(IDriverOptionsFactory driverOptionsFactory, DriverPath driverPath, IWebDriverReSizer webDriverReSizer) : base(driverOptionsFactory, driverPath, webDriverReSizer)
        {
        }

        /// <summary>
        /// Return a local WebDriver of the given browser type with default settings.
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="windowSize"></param>
        /// <param name="headless"></param>
        /// <param name="windowCustomSize"></param>
        /// <returns></returns>
        public override IWebDriver GetWebDriver(Browser browser, WindowSize windowSize = WindowSize.Hd, bool headless = false, Size windowCustomSize = new Size())
        {
            if (headless && (browser == Browser.InternetExplorer || browser == Browser.Safari))
            {
                Exception ex = new ArgumentException($"Headless mode is not currently supported for {browser}.");
                Logger.Fatal("Invalid WebDriver Configuration requested.", ex);
                throw ex;
            }

            switch (browser)
            {
                case Browser.Firefox:
                    return GetWebDriver(DriverOptionsFactory.GetLocalDriverOptions<FirefoxOptions>(headless), windowSize, windowCustomSize);

                case Browser.Chrome:
                    return GetWebDriver(DriverOptionsFactory.GetLocalDriverOptions<ChromeOptions>(headless), windowSize, windowCustomSize);

                case Browser.InternetExplorer:
                    return GetWebDriver(DriverOptionsFactory.GetLocalDriverOptions<InternetExplorerOptions>(), windowSize, windowCustomSize);

                case Browser.Edge:
                    return GetWebDriver(DriverOptionsFactory.GetLocalDriverOptions<EdgeOptions>(), windowSize, windowCustomSize);

                case Browser.Safari:
                    return GetWebDriver(DriverOptionsFactory.GetLocalDriverOptions<SafariOptions>(), windowSize, windowCustomSize);

                default:
                    Exception ex = new PlatformNotSupportedException($"{browser} is not currently supported.");
                    Logger.Fatal("Invalid WebDriver Configuration requested.", ex);
                    throw ex;
            }
        }

        /// <summary>
        /// Return a local Chromium based Edge WebDriver instance.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="windowSize"></param>
        /// <param name="windowCustomSize"></param>
        /// <returns></returns>
        [Experimental("For now this does not support nuget WebDriver installation")]
        public virtual IWebDriver GetWebDriver(EdgeOptions options, WindowSize windowSize = WindowSize.Hd, Size windowCustomSize = new Size())
        {
            return GetLocalWebDriver(options, null, windowSize, windowCustomSize);
        }

        /// <summary>
        /// Return a local Chromium based Edge WebDriver instance.
        /// Try using driverPath = null (default) for Windows 10 version 1809 and later.
        /// Try using driverPath = "Path.GetDirectoryName(Assembly.GetCallingAssembly().Location)" for Windows 10 version 1803 and earlier.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="driverPath"></param>
        /// <param name="windowSize"></param>
        /// <param name="windowCustomSize"></param>
        /// <returns></returns>
        protected IWebDriver GetLocalWebDriver(
            EdgeOptions options,
            string driverPath = null,
            WindowSize windowSize = WindowSize.Hd,
            Size windowCustomSize = new Size())
        {
            IWebDriver driver= driverPath == null
                ? new EdgeDriver(options)
                : new EdgeDriver(driverPath, options);

            return WebDriverReSizer.SetWindowSize(driver, windowSize, windowCustomSize);
        }
    }
}
