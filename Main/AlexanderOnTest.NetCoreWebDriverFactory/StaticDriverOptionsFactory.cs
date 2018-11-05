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

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Safari;

namespace AlexanderOnTest.NetCoreWebDriverFactory
{
    /// <summary>
    /// Static Factory for DriverOptions
    /// </summary>
    public static class StaticDriverOptionsFactory
    {
        /// <summary>
        /// Return a configured ChromeOptions instance.
        /// </summary>
        /// <param name="platformType"></param>
        /// <returns></returns>
        public static ChromeOptions GetChromeOptions(PlatformType platformType = PlatformType.Any)
        {
            return GetChromeOptions(false, platformType);
        }

        /// <summary>
        /// Return a configured ChromeOptions instance.
        /// </summary>
        /// <param name="headless"></param>
        /// <param name="platformType"></param>
        /// <returns></returns>
        public static ChromeOptions GetChromeOptions(bool headless = false, PlatformType platformType = PlatformType.Any)
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArguments("disable-infobars", "test-type");
            if (headless)
            {
                options.AddArgument("headless");
            }

            SetPlatform(options, platformType);
            return options;
        }

        /// <summary>
        /// Return a configured FirefoxOptions instance.
        /// </summary>
        /// <param name="platformType"></param>
        /// <returns></returns>
        public static FirefoxOptions GetFirefoxOptions(PlatformType platformType = PlatformType.Any)
        {
            return GetFirefoxOptions(false, platformType);
        }

        /// <summary>
        /// Return a configured FirefoxOptions instance.
        /// </summary>
        /// <param name="headless"></param>
        /// <param name="platformType"></param>
        /// <returns></returns>
        public static FirefoxOptions GetFirefoxOptions(bool headless = false, PlatformType platformType = PlatformType.Any)
        {
            FirefoxOptions options = new FirefoxOptions
            {
                AcceptInsecureCertificates = true
            };

            if (headless)
            {
                options.AddArgument("--headless");
            }

            SetPlatform(options, platformType);
            return options;
        }

        /// <summary>
        /// Return a configured EdgeOptions instance.
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
    }
}
