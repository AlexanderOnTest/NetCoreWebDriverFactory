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
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace AlexanderOnTest.NetCoreWebDriverFactory.Utils
{
    /// <summary>
    /// Default implementation of the IWebDriverReSizer interface
    /// </summary>
    public class WebDriverReSizer : IWebDriverReSizer
    {
        /// <summary>
        /// Set the WebDriver to the requested Browser size.
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="windowSize"></param>
        /// <param name="windowCustomSize"></param>
        /// <returns></returns>
        public IWebDriver SetWindowSize(IWebDriver driver, WindowSize windowSize, Size windowCustomSize = new Size())
        {
            switch (windowSize)
            {
                case WindowSize.Unchanged:
                    return driver;

                case WindowSize.Maximise:
                    driver.Manage().Window.Maximize();
                    return driver;

                case WindowSize.Maximize:
                    driver.Manage().Window.Maximize();
                    return driver;

                case WindowSize.Defined:
                    if (!((WebDriver)driver).Capabilities.GetCapability("browserName").Equals("Safari"))
                    {
                        driver.Manage().Window.Position = Point.Empty;
                    }
                    driver.Manage().Window.Size = windowCustomSize;
                    return driver;

                default:
                    if (!((WebDriver)driver).Capabilities.GetCapability("browserName").Equals("Safari"))
                    {
                        driver.Manage().Window.Position = Point.Empty;
                    }
                    driver.Manage().Window.Size = windowSize.Size();
                    return driver;
            }
        }
    }
}
