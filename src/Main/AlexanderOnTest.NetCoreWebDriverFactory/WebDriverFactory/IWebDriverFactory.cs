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

namespace AlexanderOnTest.NetCoreWebDriverFactory.WebDriverFactory
{
    /// <summary>
    /// Interface for WebDriverFactory Instances.
    /// </summary>
    public interface IWebDriverFactory : IDisposable
    {
        /// <summary>
        /// Return a WebDriver instance of the given configuration.
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="windowSize"></param>
        /// <param name="isLocal"></param>
        /// <param name="platformType"></param>
        /// <param name="headless"></param>
        /// <param name="windowCustomSize"></param>
        /// <returns></returns>
        IWebDriver GetWebDriver(Browser browser, WindowSize windowSize = WindowSize.Hd, bool isLocal = true,
            PlatformType platformType = PlatformType.Any, bool headless = false, Size windowCustomSize = new Size());

        /// <summary>
        /// Return a WebDriver instance of the given configuration.
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        IWebDriver GetWebDriver(IWebDriverConfiguration configuration);
    }
}

