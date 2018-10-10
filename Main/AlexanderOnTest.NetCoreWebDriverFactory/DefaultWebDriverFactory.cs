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
using System.IO;
using System.Reflection;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Safari;

namespace AlexanderOnTest.NetCoreWebDriverFactory
{
    public class DefaultWebDriverFactory : IWebDriverFactory
    {
        public DefaultWebDriverFactory(Uri gridUri = null, IDriverOptionsFactory driverOptionsFactory = null, string driverPath = null)
        {
            GridUri = gridUri?? new Uri("http://localhost:4444/wd/hub");
            DriverOptionsFactory = driverOptionsFactory ?? new DefaultDriverOptionsFactory();
        }

        public Uri GridUri { get; set; }

        public IDriverOptionsFactory DriverOptionsFactory { get; set; }

        public virtual IWebDriver GetLocalWebDriver(Browser browser, string driverPath = null, bool headless = false)
        {
            if (headless && !(browser == Browser.Chrome || browser == Browser.Firefox))
            {
                throw new ArgumentException($"Headless mode is not currently supported for {browser}.");
            }
            switch (browser)
            {
                case Browser.Firefox:
                    return GetLocalWebDriver(StaticDriverOptionsFactory.GetFirefoxOptions(headless), driverPath);

                case Browser.Chrome:
                    return GetLocalWebDriver(StaticDriverOptionsFactory.GetChromeOptions(headless), driverPath);

                case Browser.InternetExplorer:
                    return GetLocalWebDriver(StaticDriverOptionsFactory.GetInternetExplorerOptions(), driverPath);

                case Browser.Edge:
                    return GetLocalWebDriver(StaticDriverOptionsFactory.GetEdgeOptions(), driverPath);

                case Browser.Safari:
                    return GetLocalWebDriver(StaticDriverOptionsFactory.GetSafariOptions(), driverPath);

                default:
                    throw new PlatformNotSupportedException($"{browser} is not currently supported.");
            }
        }

        public virtual IWebDriver GetLocalWebDriver(
            ChromeOptions options,
            string driverPath = null, 
            WindowSize windowSize = WindowSize.Hd)
        {
            return StaticWebDriverFactory.GetLocalWebDriver(options, driverPath, windowSize);
        }

        public virtual IWebDriver GetLocalWebDriver(
            FirefoxOptions options, 
            string driverPath = null,
            WindowSize windowSize = WindowSize.Hd)
        {
            return StaticWebDriverFactory.GetLocalWebDriver(options, driverPath, windowSize);
        }

        public virtual IWebDriver GetLocalWebDriver(
            EdgeOptions options,
            string driverPath = null, 
            WindowSize windowSize = WindowSize.Hd)
        {
            return StaticWebDriverFactory.GetLocalWebDriver(options, driverPath, windowSize);
        }

        public virtual IWebDriver GetLocalWebDriver(
            InternetExplorerOptions options,
            string driverPath = null, 
            WindowSize windowSize = WindowSize.Hd)
        {
            return StaticWebDriverFactory.GetLocalWebDriver(options, driverPath, windowSize);
        }

        public virtual IWebDriver GetLocalWebDriver(
            SafariOptions options,
            string driverPath = null, 
            WindowSize windowSize = WindowSize.Hd)
        {
            return StaticWebDriverFactory.GetLocalWebDriver(options, driverPath, windowSize);
        }

        public virtual IWebDriver GetRemoteWebDriver(DriverOptions options,
            Uri gridUrl = null,
            WindowSize windowSize = WindowSize.Hd)
        {
            return StaticWebDriverFactory.GetRemoteWebDriver(options, gridUrl, windowSize);
        }

        public virtual IWebDriver GetRemoteWebDriver(Browser browser,
            Uri gridUrl = null,
            PlatformType platformType = PlatformType.Any)
        {
            Uri actualGridUrl = gridUrl ?? GridUri;
            switch (browser)
            {
                case Browser.Firefox:
                    return GetRemoteWebDriver(DriverOptionsFactory.GetFirefoxOptions(platformType), actualGridUrl);

                case Browser.Chrome:
                    return GetRemoteWebDriver(DriverOptionsFactory.GetChromeOptions(platformType), actualGridUrl);

                case Browser.InternetExplorer:
                    return GetRemoteWebDriver(DriverOptionsFactory.GetInternetExplorerOptions(platformType), actualGridUrl);

                case Browser.Edge:
                    return GetRemoteWebDriver(DriverOptionsFactory.GetEdgeOptions(platformType), actualGridUrl);

                case Browser.Safari:
                    return GetRemoteWebDriver(DriverOptionsFactory.GetSafariOptions(platformType), actualGridUrl);

                default:
                    throw new PlatformNotSupportedException($"{browser} is not currently supported.");
            }
        }

        public virtual IWebDriver SetWindowSize(IWebDriver driver, WindowSize windowSize)
        {
            return StaticWebDriverFactory.SetWindowSize(driver, windowSize);
        }
    }
}
