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
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Safari;

namespace AlexanderOnTest.NetCoreWebDriverFactory.WebDriverFactory
{
    /// <summary>
    /// Default LocalWebDriverFactory implementation for .NET Core projects.
    /// </summary>
    public class DefaultLocalWebDriverFactory : ILocalWebDriverFactory
    {
        private static readonly ILog Logger = LogProvider.For<DefaultWebDriverFactory>();
        private static readonly bool IsDebugEnabled = Logger.IsDebugEnabled();

        /// <summary>
        /// Return a DriverFactory instance for use in .NET Core projects.
        /// Try using installedDriverPath = "Path.GetDirectoryName(Assembly.GetCallingAssembly().Location)" when running from .NET core projects.
        /// </summary>
        /// <param name="installedDriverPath"></param>
        /// <param name="driverOptionsFactory"></param>
        public DefaultLocalWebDriverFactory(IDriverOptionsFactory driverOptionsFactory, string installedDriverPath)
        {
            InstalledDriverPath = installedDriverPath;
            DriverOptionsFactory = driverOptionsFactory ?? new DefaultDriverOptionsFactory();
        }

        /// <summary>
        /// Return a DriverFactory instance for use in .NET Core projects.
        /// Try using driverPath = new DriverPath(Assembly.GetCallingAssembly()) when testing locally from .NET core projects.
        /// </summary>
        /// <param name="driverPath"></param>
        /// <param name="driverOptionsFactory"></param>
        public DefaultLocalWebDriverFactory(IDriverOptionsFactory driverOptionsFactory, DriverPath driverPath)
            : this(driverOptionsFactory, driverPath.PathString)
        {
        }

        /// <summary>
        /// The path of installed drivers.
        /// </summary>
        protected string InstalledDriverPath { get; }

        /// <summary>
        /// The DriverOptionsFactory to use.
        /// </summary>
        public IDriverOptionsFactory DriverOptionsFactory { get; set; }

        /// <summary>
        /// Return a local WebDriver of the given browser type with default settings.
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="windowSize"></param>
        /// <param name="headless"></param>
        /// <param name="windowCustomSize"></param>
        /// <returns></returns>
        public virtual IWebDriver GetWebDriver(Browser browser, WindowSize windowSize = WindowSize.Hd, bool headless = false, Size windowCustomSize = new Size())
        {
            if (headless && !(browser == Browser.Chrome || browser == Browser.Firefox))
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
                configuration.WindowCustomSize
            );
        }

        /// <summary>
        /// Return a Local Chrome WebDriver instance.
        /// Try using driverPath = "Path.GetDirectoryName(Assembly.GetCallingAssembly().Location)"
        /// </summary>
        /// <param name="options"></param>
        /// <param name="windowSize"></param>
        /// <param name="windowCustomSize"></param>
        /// <returns></returns>
        public virtual IWebDriver GetWebDriver(ChromeOptions options, WindowSize windowSize = WindowSize.Hd, Size windowCustomSize = new Size())
        {
            return StaticWebDriverFactory.GetLocalWebDriver(options, InstalledDriverPath, windowSize, windowCustomSize);
        }

        /// <summary>
        /// Return a local Firefox WebDriver instance.
        /// Try using driverPath = "Path.GetDirectoryName(Assembly.GetCallingAssembly().Location)"
        /// </summary>
        /// <param name="options"></param>
        /// <param name="windowSize"></param>
        /// <param name="windowCustomSize"></param>
        /// <returns></returns>
        public virtual IWebDriver GetWebDriver(FirefoxOptions options, WindowSize windowSize = WindowSize.Hd, Size windowCustomSize = new Size())
        {
            return StaticWebDriverFactory.GetLocalWebDriver(options, InstalledDriverPath, windowSize, windowCustomSize);
        }

        /// <summary>
        /// Return a local Edge WebDriver instance. (Only supported on Microsoft Windows 10 version 1809 or later)
        /// </summary>
        /// <param name="options"></param>
        /// <param name="windowSize"></param>
        /// <param name="windowCustomSize"></param>
        /// <returns></returns>
        public virtual IWebDriver GetWebDriver(EdgeOptions options, WindowSize windowSize = WindowSize.Hd, Size windowCustomSize = new Size())
        {
            return StaticWebDriverFactory.GetLocalWebDriver(options, null, windowSize, windowCustomSize);
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
            return StaticWebDriverFactory.GetLocalWebDriver(options, InstalledDriverPath, windowSize, windowCustomSize);
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
            return StaticWebDriverFactory.GetLocalWebDriver(options, null, windowSize, windowCustomSize);
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
