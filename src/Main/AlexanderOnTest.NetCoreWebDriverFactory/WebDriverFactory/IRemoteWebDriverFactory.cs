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
using AlexanderOnTest.NetCoreWebDriverFactory.Config;
using OpenQA.Selenium;

namespace AlexanderOnTest.NetCoreWebDriverFactory.WebDriverFactory
{
    /// <summary>
    /// Interface for a Factory for WebDrivers running on a Selenium grid.
    /// </summary>
    public interface IRemoteWebDriverFactory : IDisposable
    {
        /// <summary>
        /// The Uri of your selenium grid for remote WebDriver instances.
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
        /// <param name="windowCustomSize"></param>
        /// <param name="requestedCulture"></param>
        /// <returns></returns>
        IWebDriver GetWebDriver(
            Browser browser, 
            PlatformType platformType = PlatformType.Any, 
            WindowSize windowSize = WindowSize.Hd, 
            bool headless = false, 
            Size windowCustomSize = new Size(),
            CultureInfo requestedCulture = null);

        /// <summary>
        /// Return a RemoteWebDriver instance of the given configuration.
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        IWebDriver GetWebDriver(IWebDriverConfiguration configuration);

        /// <summary>
        /// Return a RemoteWebDriver of the given windows size.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="windowSize"></param>
        /// <param name="windowCustomSize"></param>
        /// <returns></returns>
        IWebDriver GetWebDriver(DriverOptions options, WindowSize windowSize = WindowSize.Hd, Size windowCustomSize = new Size());
    }
}