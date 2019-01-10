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

namespace AlexanderOnTest.NetCoreWebDriverFactory
{
    /// <summary>
    /// A simple implementation of a WebDriverManager
    /// </summary>
    public class WebDriverManager : IWebDriverManager
    {
        private IWebDriver driver;
        private readonly Func<IWebDriver> webDriverConstructor;

        private WebDriverManager() { }

        /// <summary>
        /// Required constructor for a working WebDriverManager.
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="browser"></param>
        /// <param name="windowSize"></param>
        /// <param name="isLocal"></param>
        /// <param name="platformType"></param>
        /// <param name="headless"></param>
        public WebDriverManager(IWebDriverFactory factory, Browser browser, WindowSize windowSize = WindowSize.Hd, bool isLocal = true,
            PlatformType platformType = PlatformType.Any, bool headless = false)
        {
            this.webDriverConstructor = () => factory.GetWebDriver(browser, windowSize, isLocal, platformType, headless);
        }

        /// <summary>
        /// The singleton WebDriver instance.
        /// </summary>
        public IWebDriver Driver {
            get => driver ?? (driver = webDriverConstructor());
            private set => driver = value;
        }
        
        /// <summary>
        /// Return a singleton WebDriver instance
        /// </summary>
        /// <returns></returns>
        public virtual IWebDriver Get() => Driver;

        /// <summary>
        /// Quit and clear the current singleton WebDriver instance;
        /// </summary>
        public virtual IWebDriver Quit()
        {
            if (driver != null)
            {
                driver.Quit();
                driver = null;
            }

            return driver;
        }

        /// <summary>
        /// Return a new WebDriver instance independent of the singleton instance;
        /// </summary>
        /// <returns></returns>
        public virtual IWebDriver GetAdditionalWebDriver()
        {
            return webDriverConstructor();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                driver?.Dispose();
            }
        }
        
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}