// <copyright>
// Copyright 2018 Alexander Dunn
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
using AlexanderOnTest.NetCoreWebDriverFactory.DriverOptionsFactory;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Safari;

namespace AlexanderOnTest.NetCoreWebDriverFactory
{
    /// <summary>
    /// Overridable implementation of the IWebDriverFactory interface.
    /// </summary>
    public class DefaultWebDriverFactory : IWebDriverFactory
    {
        /// <summary>
        /// Return a DefaultWebDriverFactory instance.
        /// Try using installedDriverPath = "Path.GetDirectoryName(Assembly.GetCallingAssembly().Location)" when running from .NET core projects.
        /// </summary>
        /// <param name="installedDriverPath"></param>
        /// <param name="gridUri"></param>
        /// <param name="driverOptionsFactory"></param>
        public DefaultWebDriverFactory(string installedDriverPath, Uri gridUri = null, IDriverOptionsFactory driverOptionsFactory = null)
        {
            InstalledDriverPath = installedDriverPath;
            GridUri = gridUri?? new Uri("http://localhost:4444/wd/hub");
            DriverOptionsFactory = driverOptionsFactory ?? new DefaultDriverOptionsFactory();
        }

        /// <summary>
        /// The Uri of your selenium grid for remote Webdriver instances.
        /// </summary>
        public Uri GridUri { get; set; }

        /// <summary>
        /// The path of installed drivers.
        /// </summary>
        protected string InstalledDriverPath { get; }

        /// <summary>
        /// The DriverOptionsFactory to use.
        /// </summary>
        public IDriverOptionsFactory DriverOptionsFactory { get; set; }

        /// <summary>
        /// Return a RemoteWebDriver of the given windows size.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        public virtual IWebDriver GetRemoteWebDriver(DriverOptions options, WindowSize windowSize = WindowSize.Hd)
        {
            return StaticWebDriverFactory.GetRemoteWebDriver(options, GridUri, windowSize);
        }

        /// <summary>
        /// Return a configured RemoteWebDriver of the given browser type with default settings.
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="platformType"></param>
        /// <param name="windowSize"></param>
        /// <param name="headless"></param>
        /// <returns></returns>
        public virtual IWebDriver GetRemoteWebDriver(Browser browser, PlatformType platformType = PlatformType.Any, WindowSize windowSize = WindowSize.Hd, bool headless = false)
        {
            if (headless && !(browser == Browser.Chrome || browser == Browser.Firefox))
            {
                throw new ArgumentException($"Headless mode is not currently supported for {browser}.");
            }

            switch (browser)
            {
                case Browser.Firefox:
                    return GetRemoteWebDriver(DriverOptionsFactory.GetRemoteDriverOptions<FirefoxOptions>(platformType), windowSize);

                case Browser.Chrome:
                    return GetRemoteWebDriver(DriverOptionsFactory.GetRemoteDriverOptions<ChromeOptions>(platformType), windowSize);

                case Browser.InternetExplorer:
                    return GetRemoteWebDriver(DriverOptionsFactory.GetRemoteDriverOptions<InternetExplorerOptions>(platformType), windowSize);

                case Browser.Edge:
                    return GetRemoteWebDriver(DriverOptionsFactory.GetRemoteDriverOptions<EdgeOptions>(platformType), windowSize);

                case Browser.Safari:
                    return GetRemoteWebDriver(DriverOptionsFactory.GetRemoteDriverOptions<SafariOptions>(platformType), windowSize);

                default:
                    throw new PlatformNotSupportedException($"{browser} is not currently supported.");
            }
        }

        /// <summary>
        /// Return a local webdriver of the given browser type with default settings.
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="windowSize"></param>
        /// <param name="headless"></param>
        /// <returns></returns>
        public virtual IWebDriver GetLocalWebDriver(Browser browser, WindowSize windowSize = WindowSize.Hd, bool headless = false)
        {
            if (headless && !(browser == Browser.Chrome || browser == Browser.Firefox))
            {
                throw new ArgumentException($"Headless mode is not currently supported for {browser}.");
            }

            switch (browser)
            {
                case Browser.Firefox:
                    return GetLocalWebDriver(DriverOptionsFactory.GetLocalDriverOptions<FirefoxOptions>(headless), windowSize);

                case Browser.Chrome:
                    return GetLocalWebDriver(DriverOptionsFactory.GetLocalDriverOptions<ChromeOptions>(headless), windowSize);

                case Browser.InternetExplorer:
                    return GetLocalWebDriver(DriverOptionsFactory.GetLocalDriverOptions<InternetExplorerOptions>(), windowSize);

                case Browser.Edge:
                    return GetLocalWebDriver(DriverOptionsFactory.GetLocalDriverOptions<EdgeOptions>(), windowSize);

                case Browser.Safari:
                    return GetLocalWebDriver(DriverOptionsFactory.GetLocalDriverOptions<SafariOptions>(), windowSize);

                default:
                    throw new PlatformNotSupportedException($"{browser} is not currently supported.");
            }
        }

        /// <summary>
        /// Return a Local Chrome WebDriver instance.
        /// Try using driverPath = "Path.GetDirectoryName(Assembly.GetCallingAssembly().Location)"
        /// </summary>
        /// <param name="options"></param>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        public virtual IWebDriver GetLocalWebDriver(ChromeOptions options, WindowSize windowSize = WindowSize.Hd)
        {
            return StaticWebDriverFactory.GetLocalWebDriver(options, InstalledDriverPath, windowSize);
        }

        /// <summary>
        /// Return a local Firefox WebDriver instance.
        /// Try using driverPath = "Path.GetDirectoryName(Assembly.GetCallingAssembly().Location)"
        /// </summary>
        /// <param name="options"></param>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        public virtual IWebDriver GetLocalWebDriver(FirefoxOptions options, WindowSize windowSize = WindowSize.Hd)
        {
            return StaticWebDriverFactory.GetLocalWebDriver(options, InstalledDriverPath, windowSize);
        }

        /// <summary>
        /// Return a local Edge WebDriver instance. (Only supported on Microsoft Windows 10 version 1809 or later)
        /// </summary>
        /// <param name="options"></param>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        public virtual IWebDriver GetLocalWebDriver(EdgeOptions options, WindowSize windowSize = WindowSize.Hd)
        {
            return StaticWebDriverFactory.GetLocalWebDriver(options, null, windowSize);
        }

        /// <summary>
        /// Return a local Internet Explorer WebDriver instance. (Only supported on Microsoft Windows)
        /// </summary>
        /// <param name="options"></param>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        public virtual IWebDriver GetLocalWebDriver(InternetExplorerOptions options, WindowSize windowSize = WindowSize.Hd)
        {
            return StaticWebDriverFactory.GetLocalWebDriver(options, InstalledDriverPath, windowSize);
        }

        /// <summary>
        /// Return a local Safari WebDriver instance. (Only supported on Mac Os)
        /// </summary>
        /// <param name="options"></param>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        public virtual IWebDriver GetLocalWebDriver(SafariOptions options, WindowSize windowSize = WindowSize.Hd)
        {
            return StaticWebDriverFactory.GetLocalWebDriver(options, null, windowSize);
        }

        /// <summary>
        /// Return a WebDriver instance of the given configuration.
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="windowSize"></param>
        /// <param name="isLocal"></param>
        /// <param name="platformType"></param>
        /// <param name="headless"></param>
        /// <returns></returns>
        public virtual IWebDriver GetWebDriver(Browser browser, WindowSize windowSize = WindowSize.Hd, bool isLocal = true,
            PlatformType platformType = PlatformType.Any, bool headless = false)
        {
            return isLocal ? 
                GetLocalWebDriver(browser, windowSize, headless) :
                GetRemoteWebDriver(browser, platformType, windowSize, headless);
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
