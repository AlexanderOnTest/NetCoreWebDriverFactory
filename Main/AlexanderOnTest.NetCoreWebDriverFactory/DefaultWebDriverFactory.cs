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
        /// </summary>
        /// <param name="gridUri"></param>
        /// <param name="driverOptionsFactory"></param>
        /// <param name="driverPath"></param>
        public DefaultWebDriverFactory(Uri gridUri = null, IDriverOptionsFactory driverOptionsFactory = null, string driverPath = null)
        {
            GridUri = gridUri?? new Uri("http://localhost:4444/wd/hub");
            DriverOptionsFactory = driverOptionsFactory ?? new DefaultDriverOptionsFactory();
        }
        
        /// <summary>
        /// The Uri of your selenium grid for remote Webdriver instances.
        /// </summary>
        public Uri GridUri { get; set; }

        /// <summary>
        /// The DriverOptionsFactory to use.
        /// </summary>
        public IDriverOptionsFactory DriverOptionsFactory { get; set; }

        /// <summary>
        /// Return a local webdriver of the given browser type with default settings.
        /// Try using driverPath = "Path.GetDirectoryName(Assembly.GetCallingAssembly().Location)" for Chrome, Firefox, Internet Explorer and Edge on Windows 10 version 1803 and earlier 
        /// Try using driverPath = null (default) for Safari and Edge on Windows 10 version 1809 and later
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="driverPath"></param>
        /// <param name="headless"></param>
        /// <returns></returns>
        public virtual IWebDriver GetLocalWebDriver(Browser browser, string driverPath = null, bool headless = false)
        {
            if (headless && !(browser == Browser.Chrome || browser == Browser.Firefox))
            {
                throw new ArgumentException($"Headless mode is not currently supported for {browser}.");
            }
            switch (browser)
            {
                case Browser.Firefox:
                    return GetLocalWebDriver(DriverOptionsFactory.GetLocalDriverOptions<FirefoxOptions>(headless), driverPath);

                case Browser.Chrome:
                    return GetLocalWebDriver(DriverOptionsFactory.GetLocalDriverOptions<ChromeOptions>(headless), driverPath);

                case Browser.InternetExplorer:
                    return GetLocalWebDriver(DriverOptionsFactory.GetLocalDriverOptions<InternetExplorerOptions>(), driverPath);

                case Browser.Edge:
                    return GetLocalWebDriver(DriverOptionsFactory.GetLocalDriverOptions<EdgeOptions>(), driverPath);

                case Browser.Safari:
                    return GetLocalWebDriver(DriverOptionsFactory.GetLocalDriverOptions<SafariOptions>(), driverPath);

                default:
                    throw new PlatformNotSupportedException($"{browser} is not currently supported.");
            }
        }

        /// <summary>
        /// Return a Local Chrome WebDriver instance.
        /// Try using driverPath = "Path.GetDirectoryName(Assembly.GetCallingAssembly().Location)"
        /// </summary>
        /// <param name="options"></param>
        /// <param name="driverPath"></param>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        public virtual IWebDriver GetLocalWebDriver(
            ChromeOptions options,
            string driverPath = null, 
            WindowSize windowSize = WindowSize.Hd)
        {
            return StaticWebDriverFactory.GetLocalWebDriver(options, driverPath, windowSize);
        }

        /// <summary>
        /// Return a local Firefox WebDriver instance.
        /// Try using driverPath = "Path.GetDirectoryName(Assembly.GetCallingAssembly().Location)"
        /// </summary>
        /// <param name="options"></param>
        /// <param name="driverPath"></param>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        public virtual IWebDriver GetLocalWebDriver(
            FirefoxOptions options, 
            string driverPath = null,
            WindowSize windowSize = WindowSize.Hd)
        {
            return StaticWebDriverFactory.GetLocalWebDriver(options, driverPath, windowSize);
        }

        /// <summary>
        /// Return a local Edge WebDriver instance. (Only supported on Microsoft Windows 10)
        /// Try using driverPath = null for Windows 10 v1809 onwards.
        /// Try using driverPath = "Path.GetDirectoryName(Assembly.GetCallingAssembly().Location)" for Windows 10 v 1803 and earlier
        /// </summary>
        /// <param name="options"></param>
        /// <param name="driverPath"></param>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        public virtual IWebDriver GetLocalWebDriver(
            EdgeOptions options,
            string driverPath = null, 
            WindowSize windowSize = WindowSize.Hd)
        {
            return StaticWebDriverFactory.GetLocalWebDriver(options, driverPath, windowSize);
        }

        /// <summary>
        /// Return a local Internet Explorer WebDriver instance. (Only supported on Microsoft Windows)
        /// Try using driverPath = "Path.GetDirectoryName(Assembly.GetCallingAssembly().Location)"
        /// </summary>
        /// <param name="options"></param>
        /// <param name="driverPath"></param>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        public virtual IWebDriver GetLocalWebDriver(
            InternetExplorerOptions options,
            string driverPath = null, 
            WindowSize windowSize = WindowSize.Hd)
        {
            return StaticWebDriverFactory.GetLocalWebDriver(options, driverPath, windowSize);
        }

        /// <summary>
        /// Return a local Safari WebDriver instance. (Only supported on Mac Os)
        /// Try using driverPath = null (default value)
        /// </summary>
        /// <param name="options"></param>
        /// <param name="driverPath"></param>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        public virtual IWebDriver GetLocalWebDriver(
            SafariOptions options,
            string driverPath = null, 
            WindowSize windowSize = WindowSize.Hd)
        {
            return StaticWebDriverFactory.GetLocalWebDriver(options, driverPath, windowSize);
        }

        /// <summary>
        /// Return a RemoteWebDriver of the given browser type with default settings.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="gridUrl"></param>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        public virtual IWebDriver GetRemoteWebDriver(DriverOptions options,
            Uri gridUrl = null,
            WindowSize windowSize = WindowSize.Hd)
        {
            return StaticWebDriverFactory.GetRemoteWebDriver(options, gridUrl, windowSize);
        }

        /// <summary>
        /// Return a configured RemoteWebDriver of the given browser type with default settings.
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="gridUrl"></param>
        /// <param name="platformType"></param>
        /// <returns></returns>
        public virtual IWebDriver GetRemoteWebDriver(
            Browser browser,
            Uri gridUrl = null,
            PlatformType platformType = PlatformType.Any)
        {
            Uri actualGridUrl = gridUrl ?? GridUri;
            switch (browser)
            {
                case Browser.Firefox:
                    return GetRemoteWebDriver(DriverOptionsFactory.GetRemoteDriverOptions<FirefoxOptions>(platformType), actualGridUrl);

                case Browser.Chrome:
                    return GetRemoteWebDriver(DriverOptionsFactory.GetRemoteDriverOptions<ChromeOptions>(platformType), actualGridUrl);

                case Browser.InternetExplorer:
                    return GetRemoteWebDriver(DriverOptionsFactory.GetRemoteDriverOptions<InternetExplorerOptions>(platformType), actualGridUrl);

                case Browser.Edge:
                    return GetRemoteWebDriver(DriverOptionsFactory.GetRemoteDriverOptions<EdgeOptions>(platformType), actualGridUrl);

                case Browser.Safari:
                    return GetRemoteWebDriver(DriverOptionsFactory.GetRemoteDriverOptions<SafariOptions>(platformType), actualGridUrl);

                default:
                    throw new PlatformNotSupportedException($"{browser} is not currently supported.");
            }
        }

        /// <summary>
        /// Convenience method for setting the Window Size of a WebDriver to common values. (768P, 1080P and fullscreen)
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        public virtual IWebDriver SetWindowSize(IWebDriver driver, WindowSize windowSize)
        {
            return StaticWebDriverFactory.SetWindowSize(driver, windowSize);
        }
    }
}
