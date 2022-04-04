﻿// <copyright>
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
using AlexanderOnTest.NetCoreWebDriverFactory.Config;
using AlexanderOnTest.NetCoreWebDriverFactory.DriverOptionsFactory;
using AlexanderOnTest.NetCoreWebDriverFactory.Logging;
using AlexanderOnTest.NetCoreWebDriverFactory.Utils;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace AlexanderOnTest.NetCoreWebDriverFactory.WebDriverFactory
{
    /// <summary>
    /// Default RemoteWebDriverFactory implementation.
    /// </summary>
    public class DefaultRemoteWebDriverFactory : IRemoteWebDriverFactory
    {
        private static readonly ILogger Logger = WebDriverFactoryLogging.LoggerFactory?.CreateLogger("DefaultRemoteWebDriverFactory");
        private static readonly bool IsDebugEnabled = Logger != null && Logger.IsEnabled(LogLevel.Debug);
        private static readonly bool IsInfoEnabled = Logger != null && Logger.IsEnabled(LogLevel.Information);
        
        private readonly IWebDriverReSizer webDriverReSizer;

        /// <summary>
        /// Return a DriverFactory instance for use in .NET Core projects.
        /// Try using installedDriverPath = "Path.GetDirectoryName(Assembly.GetCallingAssembly().Location)" when running from .NET core projects.
        /// </summary>
        /// <param name="gridUri"></param>
        /// <param name="driverOptionsFactory"></param>
        /// <param name="webDriverReSizer"></param>
        public DefaultRemoteWebDriverFactory(IDriverOptionsFactory driverOptionsFactory, Uri gridUri, IWebDriverReSizer webDriverReSizer)
        {
            DriverOptionsFactory = driverOptionsFactory;
            this.webDriverReSizer = webDriverReSizer;
            GridUri = gridUri ?? new Uri("http://localhost:4444");
        }

        /// <summary>
        /// Return a DriverFactory instance for use in .NET Core projects.
        /// Try using driverPath = new DriverPath(Assembly.GetCallingAssembly()) when testing locally from .NET core projects.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="driverOptionsFactory"></param>
        /// <param name="webDriverReSizer"></param>
        public DefaultRemoteWebDriverFactory(IDriverOptionsFactory driverOptionsFactory, IWebDriverConfiguration configuration, IWebDriverReSizer webDriverReSizer)
            : this(driverOptionsFactory, configuration.GridUri, webDriverReSizer)
        {
        }

        /// <summary>
        /// The DriverOptionsFactory instance to use.
        /// </summary>
        public IDriverOptionsFactory DriverOptionsFactory { get; set; }

        /// <inheritdoc />
        public Uri GridUri { get; set; }
        
        /// <summary>
        /// Return a RemoteWebDriver of the given browser type with default settings.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="windowSize"></param>
        /// <param name="windowCustomSize"></param>
        /// <returns></returns>
        public IWebDriver GetWebDriver(
            DriverOptions options,
            WindowSize windowSize = WindowSize.Hd,
            Size windowCustomSize = new Size())
        {
            if (IsDebugEnabled)
            {
                const string messagePattern = "Remote WebDriver requested using the passed in DriverOptions:- Browser : {0}, WindowSize : {1}, (Custom Size: Height {2}, Width {3}) )";
                var messageParameters = new object[] { options.BrowserName, windowSize, windowCustomSize.Height, windowCustomSize.Width };
                Logger.LogDebug(messagePattern, messageParameters);
            }

            IWebDriver driver = new RemoteWebDriver(GridUri, options);
            return webDriverReSizer.SetWindowSize(driver, windowSize, windowCustomSize);
        }

        /// <summary>
        /// Return a RemoteWebDriver instance of the given configuration.
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public IWebDriver GetWebDriver(IWebDriverConfiguration configuration)
        {
            if (IsDebugEnabled)
            {
                const string messagePattern = "Remote WebDriver requested using {0}";
                var messageParameters = new object[] { configuration.ToString() };
                Logger.LogDebug(messagePattern, messageParameters);
            }

            if (configuration.IsLocal)
            {
                throw new ArgumentException("A Local WebDriver Instance cannot be generated by this method.");
            }

            return GetWebDriver(
                configuration.Browser,
                configuration.PlatformType,
                configuration.WindowSize,
                configuration.Headless,
                configuration.WindowDefinedSize,
                configuration.LanguageCulture
            );
        }

        /// <summary>
        /// Return a configured RemoteWebDriver of the given browser type with default settings.
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="platformType"></param>
        /// <param name="windowSize"></param>
        /// <param name="headless"></param>
        /// <param name="windowCustomSize"></param>
        /// <param name="requestedCulture"></param>
        /// <returns></returns>
        public IWebDriver GetWebDriver(
            Browser browser, 
            PlatformType platformType = PlatformType.Any, 
            WindowSize windowSize = WindowSize.Hd, 
            bool headless = false, 
            Size windowCustomSize = new Size(),
            CultureInfo requestedCulture = null)
        {
            if (browser == Browser.Safari && platformType != PlatformType.Mac ||
                browser == Browser.InternetExplorer && platformType != PlatformType.Windows)
            {
                throw new PlatformNotSupportedException($"{browser} is not currently supported on {platformType.ToString()}.");
            }
            
            if (headless && !(browser == Browser.Chrome || browser == Browser.Edge || browser == Browser.Firefox))
            {
                throw new NotSupportedException($"Headless mode is not currently supported for {browser}.");
            }
            
            if (requestedCulture != null && !(browser == Browser.Chrome || browser == Browser.Edge || browser == Browser.Firefox))
            {
                throw new NotSupportedException("The requested browser does not support requesting a given language culture.");
            }

            switch (browser)
            {
                case Browser.Firefox:
                    return GetWebDriver(DriverOptionsFactory.GetRemoteDriverOptions<FirefoxOptions>(platformType, headless, requestedCulture), windowSize, windowCustomSize);

                case Browser.Chrome:
                    return GetWebDriver(DriverOptionsFactory.GetRemoteDriverOptions<ChromeOptions>(platformType, headless, requestedCulture), windowSize, windowCustomSize);

                case Browser.InternetExplorer:
                    return GetWebDriver(DriverOptionsFactory.GetRemoteDriverOptions<InternetExplorerOptions>(platformType, headless), windowSize, windowCustomSize);

                case Browser.Edge:
                    return GetWebDriver(DriverOptionsFactory.GetRemoteDriverOptions<EdgeOptions>(platformType, headless, requestedCulture), windowSize, windowCustomSize);

                case Browser.Safari:
                    return GetWebDriver(DriverOptionsFactory.GetRemoteDriverOptions<SafariOptions>(platformType, headless), windowSize, windowCustomSize);

                default:
                    throw new NotSupportedException($"{browser} is not currently supported.");
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                DriverOptionsFactory?.Dispose();
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}