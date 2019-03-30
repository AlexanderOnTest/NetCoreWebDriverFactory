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

using System.Drawing;
using AlexanderOnTest.NetCoreWebDriverFactory.DriverOptionsFactory;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;

namespace AlexanderOnTest.NetCoreWebDriverFactory.WebDriverFactory
{
    /// <summary>
    /// Implementation of the ILocalWebDriverFactory interface for .NET Core test projects to allow Edge to work on Windows 10 version 1803 and Earlier.
    /// </summary>
    public class Windows1803LocalWebDriverFactory : DefaultLocalWebDriverFactory
    {
        /// <summary>
        /// Return a WebDriverFactory for Windows 10 version 1803 and earlier
        /// </summary>
        /// <param name="driverOptionsFactory"></param>
        /// <param name="installedDriverPath"></param>
        public Windows1803LocalWebDriverFactory(IDriverOptionsFactory driverOptionsFactory, string installedDriverPath) : base(driverOptionsFactory, installedDriverPath)
        {
        }

        /// <summary>
        /// Return a WebDriverFactory for Windows 10 version 1803 and earlier
        /// </summary>
        /// <param name="driverOptionsFactory"></param>
        /// <param name="driverPath"></param>
        public Windows1803LocalWebDriverFactory(IDriverOptionsFactory driverOptionsFactory, DriverPath driverPath)
            : this(driverOptionsFactory, driverPath.PathString)
        {
        }

        /// <summary>
        /// Return a local webdriver of the given browser type with default settings.
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="windowSize"></param>
        /// <param name="headless"></param>
        /// <param name="windowCustomSize"></param>
        /// <returns></returns>
        public override IWebDriver GetWebDriver(Browser browser, WindowSize windowSize = WindowSize.Hd, bool headless = false, Size windowCustomSize = new Size())
        {
            // The only case needing to be overidden is Edge not headless
            return (browser == Browser.Edge && !headless) ?
                GetWebDriver(DriverOptionsFactory.GetLocalDriverOptions<EdgeOptions>(), windowSize, windowCustomSize) :
                base.GetWebDriver(browser, windowSize, headless, windowCustomSize);
        }

        /// <summary>
        /// Return a local Edge WebDriver instance. (Only supported on Microsoft Windows 10 version 1803 or earlier)
        /// </summary>
        /// <param name="options"></param>
        /// <param name="windowSize"></param>
        /// <param name="windowCustomSize"></param>
        /// <returns></returns>
        public override IWebDriver GetWebDriver(EdgeOptions options, WindowSize windowSize = WindowSize.Hd, Size windowCustomSize = new Size())
        {
            return StaticWebDriverFactory.GetLocalWebDriver(options, InstalledDriverPath, windowSize, windowCustomSize);
        }
    }
}
