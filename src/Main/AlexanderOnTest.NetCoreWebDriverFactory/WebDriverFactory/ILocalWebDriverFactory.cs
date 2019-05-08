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
using AlexanderOnTest.NetCoreWebDriverFactory.Config;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Safari;

namespace AlexanderOnTest.NetCoreWebDriverFactory.WebDriverFactory
{
    /// <summary>
    /// Interface for a Factory for WebDrivers running on the local machine.
    /// </summary>
    public interface ILocalWebDriverFactory : IDisposable
    {
        /// <summary>
        /// Return a local webdriver of the given browser type with default settings.
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="windowSize"></param>
        /// <param name="headless"></param>
        /// <param name="windowCustomSize"></param>
        /// <returns></returns>
        IWebDriver GetWebDriver(Browser browser, WindowSize windowSize = WindowSize.Hd, bool headless = false, Size windowCustomSize = new Size());

        /// <summary>
        /// Return a Local IWebDriver instance of the given configuration.
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        IWebDriver GetWebDriver(IWebDriverConfiguration configuration);

        /// <summary>
        /// Return a Local Chrome WebDriver instance.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="windowSize"></param>
        /// <param name="windowCustomSize"></param>
        /// <returns></returns>
        IWebDriver GetWebDriver(ChromeOptions options, WindowSize windowSize = WindowSize.Hd, Size windowCustomSize = new Size());

        /// <summary>
        /// Return a local Firefox WebDriver instance.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="windowSize"></param>
        /// <param name="windowCustomSize"></param>
        /// <returns></returns>
        IWebDriver GetWebDriver(FirefoxOptions options, WindowSize windowSize = WindowSize.Hd, Size windowCustomSize = new Size());

        /// <summary>
        /// Return a local Edge WebDriver instance. (Only supported on Microsoft Windows 10)
        /// </summary>
        /// <param name="options"></param>
        /// <param name="windowSize"></param>
        /// <param name="windowCustomSize"></param>
        /// <returns></returns>
        IWebDriver GetWebDriver(EdgeOptions options, WindowSize windowSize = WindowSize.Hd, Size windowCustomSize = new Size());

        /// <summary>
        /// Return a local Internet Explorer WebDriver instance. (Only supported on Microsoft Windows)
        /// </summary>
        /// <param name="options"></param>
        /// <param name="windowSize"></param>
        /// <param name="windowCustomSize"></param>
        /// <returns></returns>
        IWebDriver GetWebDriver(InternetExplorerOptions options, WindowSize windowSize = WindowSize.Hd, Size windowCustomSize = new Size());

        /// <summary>
        /// Return a local Safari WebDriver instance. (Only supported on Mac Os)
        /// </summary>
        /// <param name="options"></param>
        /// <param name="windowSize"></param>
        /// <param name="windowCustomSize"></param>
        /// <returns></returns>
        IWebDriver GetWebDriver(SafariOptions options, WindowSize windowSize = WindowSize.Hd, Size windowCustomSize = new Size());
    }
}