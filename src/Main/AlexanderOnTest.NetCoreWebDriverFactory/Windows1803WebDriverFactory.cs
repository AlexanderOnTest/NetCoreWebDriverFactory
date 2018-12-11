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
using System.Collections.Generic;
using System.Text;
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
    /// Example Override implementation, This will allow Edge to work on Windows 10 version 1803 and Earlier.
    /// </summary>
    public class Windows1803WebDriverFactory : DefaultWebDriverFactory
    {
        /// <summary>
        /// Return a WebDriverFactory for Windows 10 version 1803 and earlier
        /// </summary>
        /// <param name="installedDriverPath"></param>
        /// <param name="gridUri"></param>
        /// <param name="driverOptionsFactory"></param>
        public Windows1803WebDriverFactory(string installedDriverPath, Uri gridUri = null, IDriverOptionsFactory driverOptionsFactory = null) : base(installedDriverPath, gridUri, driverOptionsFactory)
        {
        }
        
        /// <summary>
        /// Return a local webdriver of the given browser type with default settings.
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="windowSize"></param>
        /// <param name="headless"></param>
        /// <returns></returns>
        public override IWebDriver GetLocalWebDriver(Browser browser, WindowSize windowSize = WindowSize.Hd, bool headless = false)
        {
            // The only case needing to be overidden is Edge not headless
            return (browser == Browser.Edge && !headless) ?
                GetLocalWebDriver(DriverOptionsFactory.GetLocalDriverOptions<EdgeOptions>(), windowSize) :
                base.GetLocalWebDriver(browser, windowSize, headless);
        }

        /// <summary>
        /// Return a local Edge WebDriver instance. (Only supported on Microsoft Windows 10 version 1803 or earlier)
        /// </summary>
        /// <param name="options"></param>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        public override IWebDriver GetLocalWebDriver(EdgeOptions options, WindowSize windowSize = WindowSize.Hd)
        {
            return StaticWebDriverFactory.GetLocalWebDriver(options, InstalledDriverPath, windowSize);
        }
    }
}
