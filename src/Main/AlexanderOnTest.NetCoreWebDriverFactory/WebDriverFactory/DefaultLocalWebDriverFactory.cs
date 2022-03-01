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
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AlexanderOnTest.NetCoreWebDriverFactory.Config;
using AlexanderOnTest.NetCoreWebDriverFactory.DriverOptionsFactory;
using AlexanderOnTest.NetCoreWebDriverFactory.Logging;
using AlexanderOnTest.NetCoreWebDriverFactory.Utils;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Safari;

[assembly: InternalsVisibleTo("AlexanderOnTest.NetCoreWebDriverFactory.UnitTests")]

namespace AlexanderOnTest.NetCoreWebDriverFactory.WebDriverFactory
{
    /// <summary>
    /// Default LocalWebDriverFactory implementation - uses Edge for IE testing on Windows (required for Windows 11).
    /// </summary>
    public class DefaultLocalWebDriverFactory :ILocalWebDriverFactory
    {
        private static readonly ILog Logger = LogProvider.For<DefaultWebDriverFactory>();
        private static readonly bool IsDebugEnabled = Logger.IsDebugEnabled();
        
        /// <summary>
        /// Return a DriverFactory instance. Default to using Edge for IE testing on Windows (required for Windows 11).
        /// Driver paths should not need to be provided for Drivers installed by packages or on the System Path.
        /// </summary>
        /// <param name="driverOptionsFactory"></param>
        /// <param name="webDriverReSizer"></param>
        /// <param name="driverPath"></param>
        public DefaultLocalWebDriverFactory(IDriverOptionsFactory driverOptionsFactory,
            IWebDriverReSizer webDriverReSizer,
            DriverPath driverPath = null)        {
            InstalledDriverPath = driverPath?.PathString;
            DriverOptionsFactory = driverOptionsFactory ?? new DefaultDriverOptionsFactory();
            WebDriverReSizer = webDriverReSizer;
        }

        /// <summary>
        /// The path of installed drivers.
        /// </summary>
        public string InstalledDriverPath { get; }

        /// <summary>
        /// The IWebDriverReSizer implementation to use.
        /// </summary>
        private IWebDriverReSizer WebDriverReSizer { get; }

        /// <summary>
        /// The DriverOptionsFactory to use.
        /// </summary>
        public IDriverOptionsFactory DriverOptionsFactory { get;}

        /// <summary>
        /// Path to the Microsoft Edge Executable (default install location for Testing using IEMode in Edge on Windows)
        /// </summary>
        public string EdgeExecutablePath { get; set; } = "C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\msedge.exe";

        /// <summary>
        /// Use Microsoft Edge for testing Internet Explorer compatibility.
        /// </summary>
        public bool UseEdgeForInternetExplorer { get; set; } = true;

        /// <summary>
        /// Return a local WebDriver of the given browser type with default settings.
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="windowSize"></param>
        /// <param name="headless"></param>
        /// <param name="windowCustomSize"></param>
        /// <param name="requestedCulture"></param>
        /// <returns></returns>
        public virtual IWebDriver GetWebDriver(
            Browser browser, 
            WindowSize windowSize = WindowSize.Hd, 
            bool headless = false, 
            Size windowCustomSize = new Size(),
            CultureInfo requestedCulture = null)
        {
            if (headless && 
                !(browser == Browser.Chrome || 
                  browser == Browser.Edge || 
                  browser == Browser.Firefox))
            {
                Exception ex = new NotSupportedException($"Headless mode is not currently supported for {browser}.");
                Logger.Fatal("Invalid WebDriver Configuration requested.", ex);
                throw ex;
            }

            switch (browser)
            {
                case Browser.Firefox:
                    return GetWebDriver(DriverOptionsFactory.GetLocalDriverOptions<FirefoxOptions>(headless, requestedCulture), windowSize, windowCustomSize);

                case Browser.Chrome:
                    return GetWebDriver(DriverOptionsFactory.GetLocalDriverOptions<ChromeOptions>(headless, requestedCulture), windowSize, windowCustomSize);

                case Browser.InternetExplorer:
                    return GetWebDriver(DriverOptionsFactory.GetLocalDriverOptions<InternetExplorerOptions>(headless, requestedCulture), windowSize, windowCustomSize);

                case Browser.Edge:
                    return GetWebDriver(DriverOptionsFactory.GetLocalDriverOptions<EdgeOptions>(headless, requestedCulture), windowSize, windowCustomSize);

                case Browser.Safari:
                    return GetWebDriver(DriverOptionsFactory.GetLocalDriverOptions<SafariOptions>(headless, requestedCulture), windowSize, windowCustomSize);

                default:
                    Exception ex = new NotSupportedException($"{browser} is not currently supported.");
                    Logger.Fatal("Invalid WebDriver Configuration requested.", ex);
                    throw ex;
            }
        }

        /// <summary>
        /// Return a local WebDriver instance of the given configuration.
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public IWebDriver GetWebDriver(IWebDriverConfiguration configuration)
        {
            Logger.Info($"Local WebDriver requested using configuration: {configuration}");

            if (!configuration.IsLocal)
            {
                Exception ex = new ArgumentException("A RemoteWebDriver Instance cannot be generated by this method.");
                Logger.Fatal("Invalid WebDriver Configuration requested.", ex);
                throw ex;
            }

            return GetWebDriver(
                configuration.Browser,
                configuration.WindowSize,
                configuration.Headless,
                configuration.WindowDefinedSize,
                configuration.LanguageCulture
            );
        }

        /// <summary>
        /// Return a Local Chrome WebDriver instance.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="windowSize"></param>
        /// <param name="windowCustomSize"></param>
        /// <returns></returns>
        public virtual IWebDriver GetWebDriver(ChromeOptions options, WindowSize windowSize = WindowSize.Hd, Size windowCustomSize = new Size())
        {
            IWebDriver driver = InstalledDriverPath == null
                ? new ChromeDriver(options)
                : new ChromeDriver(InstalledDriverPath, options);

            return WebDriverReSizer.SetWindowSize(driver, windowSize, windowCustomSize);
        }

        /// <summary>
        /// Return a local Edge WebDriver instance.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="windowSize"></param>
        /// <param name="windowCustomSize"></param>
        /// <returns></returns>
        public virtual IWebDriver GetWebDriver(EdgeOptions options, WindowSize windowSize = WindowSize.Hd, Size windowCustomSize = new Size())
        {
            IWebDriver driver = InstalledDriverPath == null
                ? new EdgeDriver(options)
                : new EdgeDriver(InstalledDriverPath, options);

            return WebDriverReSizer.SetWindowSize(driver, windowSize, windowCustomSize);
        }

        /// <summary>
        /// Return a local Firefox WebDriver instance.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="windowSize"></param>
        /// <param name="windowCustomSize"></param>
        /// <returns></returns>
        public virtual IWebDriver GetWebDriver(FirefoxOptions options, WindowSize windowSize = WindowSize.Hd, Size windowCustomSize = new Size())
        {
            
            IWebDriver driver = InstalledDriverPath == null
                ? new FirefoxDriver(options)
                : new FirefoxDriver(InstalledDriverPath, options);

            return WebDriverReSizer.SetWindowSize(driver, windowSize, windowCustomSize);
        }

        /// <summary>
        /// Return a local Internet Explorer WebDriver instance. (Only supported on Microsoft Windows)
        /// </summary>
        /// <param name="options"></param>
        /// <param name="windowSize"></param>
        /// <param name="windowCustomSize"></param>
        /// <returns></returns>
        public virtual IWebDriver GetWebDriver(InternetExplorerOptions options, WindowSize windowSize = WindowSize.Hd, Size windowCustomSize = new Size())
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                throw new PlatformNotSupportedException("Microsoft Internet Explorer is only available on Microsoft Windows.");
            }
            
            if (UseEdgeForInternetExplorer)
            {
                options.AttachToEdgeChrome = true;
                options.EdgeExecutablePath = EdgeExecutablePath;
            }

            IWebDriver driver = InstalledDriverPath == null
                ? new InternetExplorerDriver(options)
                : new InternetExplorerDriver(InstalledDriverPath, options);

            return WebDriverReSizer.SetWindowSize(driver, windowSize, windowCustomSize);
        }

        /// <summary>
        /// Return a local Safari WebDriver instance. (Only supported on Mac Os)
        /// </summary>
        /// <param name="options"></param>
        /// <param name="windowSize"></param>
        /// <param name="windowCustomSize"></param>
        /// <returns></returns>
        public virtual IWebDriver GetWebDriver(SafariOptions options, WindowSize windowSize = WindowSize.Hd, Size windowCustomSize = new Size())
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                throw new PlatformNotSupportedException("Safari is only available on Mac Os.");
            }

            IWebDriver driver = InstalledDriverPath == null
                ? new SafariDriver(options)
                : new SafariDriver(InstalledDriverPath, options);

            return WebDriverReSizer.SetWindowSize(driver, windowSize, windowCustomSize);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
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
