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
using System.Drawing;
using System.Runtime.InteropServices;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;

namespace AlexanderOnTest.NetCoreWebDriverFactory
{
    /// <summary>
    /// Static Factory for WebDriverInstances with configuration.
    /// </summary>
    public static class StaticWebDriverFactory
    {
        /// <summary>
        /// Return a local webdriver of the given browser type with default settings.
        /// Try using driverPath = "Path.GetDirectoryName(Assembly.GetCallingAssembly().Location)" for Chrome, Firefox, Internet Explorer and Edge on Windows 10 version 1803 and earlier 
        /// Try using driverPath = null (default) for Safari and Edge on Windows 10 version 1809 and later
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="driverPath"></param>
        /// <param name="headless"></param>
        /// <returns></returns>
        public static IWebDriver GetLocalWebDriver(Browser browser, string driverPath = null, bool headless = false)
        {
            if (headless && !(browser == Browser.Chrome || browser == Browser.Firefox))
            {
                throw new ArgumentException($"Headless mode is not currently supported for {browser}.");
            }
            switch (browser)
            {
                case Browser.Firefox:
                    return GetLocalWebDriver(StaticDriverOptionsFactory.GetFirefoxOptions(PlatformType.Any, headless), driverPath);

                case Browser.Chrome:
                    return GetLocalWebDriver(StaticDriverOptionsFactory.GetChromeOptions(headless), driverPath);

                case Browser.InternetExplorer:
                    return GetLocalWebDriver(StaticDriverOptionsFactory.GetInternetExplorerOptions(), driverPath);

                case Browser.Edge:
                    return GetLocalWebDriver(StaticDriverOptionsFactory.GetEdgeOptions(), driverPath);

                case Browser.Safari:
                    return GetLocalWebDriver(StaticDriverOptionsFactory.GetSafariOptions());

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
        public static IWebDriver GetLocalWebDriver(
            ChromeOptions options,
            string driverPath = null,
            WindowSize windowSize = WindowSize.Hd)
        {
            IWebDriver driver = null;
            try
            {
                driver = driverPath == null
                    ? new ChromeDriver(options)
                    : new ChromeDriver(driverPath, options);
            }
            catch (DriverServiceNotFoundException ex)
            {
                RethrowWithSuggestedPath(ex);
            }

            return SetWindowSize(driver, windowSize);
        }

        /// <summary>
        /// Return a local Firefox WebDriver instance.
        /// Try using driverPath = "Path.GetDirectoryName(Assembly.GetCallingAssembly().Location)" 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="driverPath"></param>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        public static IWebDriver GetLocalWebDriver(
            FirefoxOptions options, 
            string driverPath = null,
            WindowSize windowSize = WindowSize.Hd)
        {
            IWebDriver driver = null;
            try
            {
                driver = driverPath == null
                    ? new FirefoxDriver(options)
                    : new FirefoxDriver(driverPath, options);
            }
            catch (DriverServiceNotFoundException ex)
            {
                RethrowWithSuggestedPath(ex);
            }

            return SetWindowSize(driver, windowSize);
        }

        /// <summary>
        /// Return a local Edge WebDriver instance. (Only supported on Microsoft Windows 10)
        /// Try using driverPath = null (default) for Windows 10 version 1809 and later.
        /// Try using driverPath = "Path.GetDirectoryName(Assembly.GetCallingAssembly().Location)" for Windows 10 version 1803 and earlier.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="driverPath"></param>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        public static IWebDriver GetLocalWebDriver(
            EdgeOptions options, 
            string driverPath = null,
            WindowSize windowSize = WindowSize.Hd)
        {
            if (!RuntimeInformation.OSDescription.StartsWith("Microsoft Windows 10"))
            {
                throw new PlatformNotSupportedException("Microsoft Edge is only available on Microsoft Windows 10.");
            }

            IWebDriver driver = null;
            try
            {
                driver = driverPath == null
                    ? new EdgeDriver(options)
                    : new EdgeDriver(driverPath, options);
            }
            catch (DriverServiceNotFoundException ex)
            {
                RethrowWithSuggestionOfNoPath(ex);
            }

            return SetWindowSize(driver, windowSize);
        }

        /// <summary>
        /// Return a local Internet Explorer WebDriver instance. (Only supported on Microsoft Windows)
        /// Try using driverPath = "Path.GetDirectoryName(Assembly.GetCallingAssembly().Location)"
        /// </summary>
        /// <param name="options"></param>
        /// <param name="driverPath"></param>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        public static IWebDriver GetLocalWebDriver(
            InternetExplorerOptions options, 
            string driverPath = null,
            WindowSize windowSize = WindowSize.Hd)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                throw new PlatformNotSupportedException("Microsoft Internet Explorer is only available on Microsoft Windows.");
            }

            IWebDriver driver = null;
            try
            {
                driver = driverPath == null
                    ? new InternetExplorerDriver(options)
                    : new InternetExplorerDriver(driverPath, options);
            }
            catch (DriverServiceNotFoundException ex)
            {
                RethrowWithSuggestedPath(ex);
            }

            return SetWindowSize(driver, windowSize);
        }

        /// <summary>
        /// Return a local Safari WebDriver instance. (Only supported on Mac Os)
        /// Try using driverPath = null (default)
        /// </summary>
        /// <param name="options"></param>
        /// <param name="driverPath"></param>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        public static IWebDriver GetLocalWebDriver(
            SafariOptions options, 
            string driverPath = null,
            WindowSize windowSize = WindowSize.Hd)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                throw new PlatformNotSupportedException("Safari is only available on Mac Os.");
            }

            IWebDriver driver = null;
            try
            {
                driver = driverPath == null
                    ? new SafariDriver(options)
                    : new SafariDriver(driverPath, options);
            }
            catch (DriverServiceNotFoundException ex)
            {
                RethrowWithSuggestionOfNoPath(ex);
            }
            return SetWindowSize(driver, windowSize);
        }

        /// <summary>
        /// Return a RemoteWebDriver of the given browser type with default settings.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="gridUrl"></param>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        public static IWebDriver GetRemoteWebDriver(
            DriverOptions options,
            Uri gridUrl,
            WindowSize windowSize = WindowSize.Hd)
        {
            IWebDriver driver = new RemoteWebDriver(gridUrl, options);
            return SetWindowSize(driver, windowSize);
        }

        /// <summary>
        /// Return a configured RemoteWebDriver of the given browser type with default settings.
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="gridUrl"></param>
        /// <param name="platformType"></param>
        /// <returns></returns>
        public static IWebDriver GetRemoteWebDriver(
            Browser browser,
            Uri gridUrl,
            PlatformType platformType = PlatformType.Any)
        {
            switch (browser)
            {
                case Browser.Firefox:
                    return GetRemoteWebDriver(StaticDriverOptionsFactory.GetFirefoxOptions(platformType), gridUrl);

                case Browser.Chrome:
                    return GetRemoteWebDriver(StaticDriverOptionsFactory.GetChromeOptions(platformType), gridUrl);

                case Browser.InternetExplorer:
                    return GetRemoteWebDriver(StaticDriverOptionsFactory.GetInternetExplorerOptions(platformType), gridUrl);

                case Browser.Edge:
                    return GetRemoteWebDriver(StaticDriverOptionsFactory.GetEdgeOptions(platformType), gridUrl);

                case Browser.Safari:
                    return GetRemoteWebDriver(StaticDriverOptionsFactory.GetSafariOptions(platformType), gridUrl);

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
        public static IWebDriver SetWindowSize(IWebDriver driver, WindowSize windowSize)
        {
            switch (windowSize)
            {
                case WindowSize.Unchanged:
                    return driver;

                case WindowSize.Maximise:
                    driver.Manage().Window.Maximize();
                    return driver;

                case WindowSize.Hd:
                    if (!((RemoteWebDriver)driver).Capabilities.GetCapability("browserName").Equals("Safari"))
                    {
                    driver.Manage().Window.Position = Point.Empty;
                    }
                    driver.Manage().Window.Size = new Size(1366, 768);
                    return driver;

                case WindowSize.Fhd:
                    if (!((RemoteWebDriver)driver).Capabilities.GetCapability("browserName").Equals("Safari"))
                    {
                        driver.Manage().Window.Position = Point.Empty;
                    }
                    driver.Manage().Window.Size = new Size(1920, 1080);
                    return driver;

                default:
                    return driver;
            }
        }

        private static void RethrowWithSuggestedPath(DriverServiceNotFoundException driverServiceNotFoundException)
        {
            throw new DriverServiceNotFoundException("Try calling with the DriverPath set to 'Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)' or add the driverPath to the Path", driverServiceNotFoundException);
        }

        private static void RethrowWithSuggestionOfNoPath(DriverServiceNotFoundException driverServiceNotFoundException)
        {
            throw new DriverServiceNotFoundException("Try calling with the DriverPath set to 'null' and ensure that the driverPath is added to the environment Path", driverServiceNotFoundException);
        }
    }
}
