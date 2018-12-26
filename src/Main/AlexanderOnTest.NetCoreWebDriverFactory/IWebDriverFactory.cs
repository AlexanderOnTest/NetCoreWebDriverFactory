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
    /// Interface for WebDriverFactory Instances.
    /// </summary>
    public interface IWebDriverFactory
    {
        /// <summary>
        /// The Uri of your selenium grid for remote Webdriver instances.
        /// </summary>
        Uri GridUri
        {
            get;
            set;
        }

        /// <summary>
        /// Return a configured RemoteWebDriver of the given browser type with default settings.
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="platformType"></param>
        /// <param name="windowSize"></param>
        /// <param name="headless"></param>
        /// <returns></returns>
        IWebDriver GetRemoteWebDriver(Browser browser, PlatformType platformType = PlatformType.Any, WindowSize windowSize = WindowSize.Hd, bool headless = false);

        /// <summary>
        /// Return a RemoteWebDriver of the given windows size.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        IWebDriver GetRemoteWebDriver(DriverOptions options, WindowSize windowSize = WindowSize.Hd);

        /// <summary>
        /// Return a local webdriver of the given browser type with default settings.
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="windowSize"></param>
        /// <param name="headless"></param>
        /// <returns></returns>
        IWebDriver GetLocalWebDriver(Browser browser, WindowSize windowSize = WindowSize.Hd, bool headless = false);

        /// <summary>
        /// Return a Local Chrome WebDriver instance.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        IWebDriver GetLocalWebDriver(ChromeOptions options, WindowSize windowSize = WindowSize.Hd);

        /// <summary>
        /// Return a local Firefox WebDriver instance.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        IWebDriver GetLocalWebDriver(FirefoxOptions options, WindowSize windowSize = WindowSize.Hd);

        /// <summary>
        /// Return a local Edge WebDriver instance. (Only supported on Microsoft Windows 10)
        /// </summary>
        /// <param name="options"></param>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        IWebDriver GetLocalWebDriver(EdgeOptions options, WindowSize windowSize = WindowSize.Hd);

        /// <summary>
        /// Return a local Internet Explorer WebDriver instance. (Only supported on Microsoft Windows)
        /// </summary>
        /// <param name="options"></param>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        IWebDriver GetLocalWebDriver(InternetExplorerOptions options, WindowSize windowSize = WindowSize.Hd);

        /// <summary>
        /// Return a local Safari WebDriver instance. (Only supported on Mac Os)
        /// </summary>
        /// <param name="options"></param>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        IWebDriver GetLocalWebDriver(SafariOptions options, WindowSize windowSize = WindowSize.Hd);

        /// <summary>
        /// Return a WebDriver instance of the given configuration.
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="windowSize"></param>
        /// <param name="isLocal"></param>
        /// <param name="platformType"></param>
        /// <param name="headless"></param>
        /// <returns></returns>
        IWebDriver GetWebDriver(Browser browser, WindowSize windowSize = WindowSize.Hd, bool isLocal = true,
            PlatformType platformType = PlatformType.Any, bool headless = false);
    }
}

