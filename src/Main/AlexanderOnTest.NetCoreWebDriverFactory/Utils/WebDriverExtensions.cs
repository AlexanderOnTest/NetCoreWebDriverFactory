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

using OpenQA.Selenium;

namespace AlexanderOnTest.NetCoreWebDriverFactory.Utils
{
    /// <summary>
    /// WebDriver Extensions
    /// </summary>
    public static class WebDriverExtensions
    {
        /// <summary>
        /// <para> Is this WebDriver instance running Headless </para>
        /// <para> Caution this relies upon implmentation details and is liable to breaking. </para>
        /// <para> See <cref>https://stackoverflow.com/questions/47559054/generic-way-to-check-browser-options-of-running-webdriver-instance</cref></para>
        /// </summary>
        /// <param name="driver"></param>
        /// <returns></returns>
        public static bool IsRunningHeadless(this IWebDriver driver)
        {
            // Check for Firefox headless flag
            var webDriver = driver as WebDriver;
            object firefoxHeadlessCapability = webDriver.Capabilities.GetCapability("moz:headless");
            if (firefoxHeadlessCapability != null &&
                firefoxHeadlessCapability.ToString()!.ToLower().Equals("true"))
            {
                return true;
            }

            // all other instances
            var jse = driver as IJavaScriptExecutor;
            var agent = jse.ExecuteScript("return window.navigator.userAgent").ToString();
            return agent.ToLower().Contains("headless");
        }
    }
}
