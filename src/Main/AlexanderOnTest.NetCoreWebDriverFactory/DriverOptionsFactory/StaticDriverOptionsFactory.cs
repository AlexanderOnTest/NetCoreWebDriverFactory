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

namespace AlexanderOnTest.NetCoreWebDriverFactory.DriverOptionsFactory
{
    /// <summary>
    /// Static Factory for DriverOptions
    /// </summary>
    public static class StaticDriverOptionsFactory
    {
        /// <summary>
        /// Return a configured ChromeOptions instance for a RemoteWebDriver.
        /// </summary>
        /// <param name="platformType"></param>
        /// <param name="headless"></param>
        /// <returns></returns>
        public static ChromeOptions GetChromeOptions(PlatformType platformType, bool headless = false)
        {
            ChromeOptions options = GetChromeOptions(headless);
            SetPlatform(options, platformType);
            return options;
        }

        /// <summary>
        /// Return a configured ChromeOptions instance  for a local WebDriver.
        /// </summary>
        /// <param name="headless"></param>
        /// <returns></returns>
        public static ChromeOptions GetChromeOptions(bool headless = false)
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArguments("disable-infobars", "test-type");

            return headless ? AddHeadlessOption(options) : options;
        }

        /// <summary>
        /// Return a configured FirefoxOptions instance for a RemoteWebDriver.
        /// </summary>
        /// <param name="platformType"></param>
        /// <param name="headless"></param>
        /// <returns></returns>
        public static FirefoxOptions GetFirefoxOptions(PlatformType platformType, bool headless = false)
        {
            FirefoxOptions options = GetFirefoxOptions(headless);
            SetPlatform(options, platformType);
            return options;
        }

        /// <summary>
        /// Return a configured FirefoxOptions instance for a local WebDriver.
        /// </summary>
        /// <param name="headless"></param>
        /// <returns></returns>
        public static FirefoxOptions GetFirefoxOptions(bool headless = false)
        {
            FirefoxOptions options = new FirefoxOptions
            {
                AcceptInsecureCertificates = true
            };
            
            return headless ? AddHeadlessOption(options) : options;
        }

        /// <summary>
        /// Return a configured OpenQA.Selenium.Edge.EdgeOptions instance for EdgeHtml based Edge
        /// </summary>
        /// <param name="platformType"></param>
        /// <returns></returns>
        public static EdgeOptions GetEdgeOptions(PlatformType platformType = PlatformType.Any)
        {
            EdgeOptions options =  new EdgeOptions();
            SetPlatform(options, platformType);
            return options;
        }
        
        /// <summary>
        /// Return a Microsoft.Edge.SeleniumTools.EdgeOptions instance configured for Chromium Based Edge.
        /// </summary>
        /// <param name="platformType"></param>
        /// <returns></returns>
        public static Microsoft.Edge.SeleniumTools.EdgeOptions GetEdgiumOptions(PlatformType platformType = PlatformType.Any)
        {
            Microsoft.Edge.SeleniumTools.EdgeOptions options = new Microsoft.Edge.SeleniumTools.EdgeOptions
            {
                UseChromium = true
            };
            SetPlatform(options, platformType);
            return options;
        }

        /// <summary>
        /// Return a configured InternetExplorerOptions instance.
        /// </summary>
        /// <param name="platformType"></param>
        /// <returns></returns>
        public static InternetExplorerOptions GetInternetExplorerOptions(PlatformType platformType = PlatformType.Any)
        {
            InternetExplorerOptions options = new InternetExplorerOptions
            {
                EnablePersistentHover = true,
                IgnoreZoomLevel = true,
                UnhandledPromptBehavior = UnhandledPromptBehavior.Dismiss,
            };

            SetPlatform(options, platformType);
            return options;
        }

        /// <summary>
        /// Return a configured SafariOptions instance.
        /// </summary>
        /// <param name="platformType"></param>
        /// <returns></returns>
        public static SafariOptions GetSafariOptions(PlatformType platformType = PlatformType.Any)
        {
            SafariOptions options = new SafariOptions();
            SetPlatform(options, platformType);
            return options;
        }

        /// <summary>
        /// Add the platform configuration to a DriverOptions instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="options"></param>
        /// <param name="platformType"></param>
        /// <returns></returns>
        public static T SetPlatform<T>(T options, PlatformType platformType) where T : DriverOptions
        {
            switch (platformType)
            {
                case PlatformType.Any:
                    return options;

                case PlatformType.Windows:
                    options.PlatformName = "WINDOWS";
                    return options;

                case PlatformType.Linux:
                    options.PlatformName = "LINUX";
                    return options;

                case PlatformType.Mac:
                    options.PlatformName = "MAC";
                    return options;

                default:
                    return options;
            }
        }

        /// <summary>
        /// Add the headless flag if available.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="driverOptions"></param>
        /// <returns></returns>
        public static T AddHeadlessOption<T>(T driverOptions) where T : DriverOptions
        {
            switch (driverOptions)
            {
                case ChromeOptions chromeOptions:
                    chromeOptions.AddArgument("headless");
                    return driverOptions;
                case FirefoxOptions firefoxOptions:
                    firefoxOptions.AddArgument("--headless");
                    return driverOptions;
            }

            throw new ArgumentException("Only Chrome and Firefox support headless operation");
        }
    }
}
