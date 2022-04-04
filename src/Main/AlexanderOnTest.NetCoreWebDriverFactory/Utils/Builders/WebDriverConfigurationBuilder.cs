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
using System.Text;
using AlexanderOnTest.NetCoreWebDriverFactory.Config;
using OpenQA.Selenium;

namespace AlexanderOnTest.NetCoreWebDriverFactory.Utils.Builders
{
    /// <summary>
    /// Builder class for easy configuration
    /// </summary>
    public class WebDriverConfigurationBuilder
    {
        private Browser browser;
        private PlatformType platformType;
        private WindowSize windowSize;
        private Size windowDefinedSize;
        private Uri gridUri;
        private bool isLocal;
        private bool headless;
        private CultureInfo languageCulture;

        /// <summary>
        /// return a new instance of this class
        /// </summary>
        /// <returns></returns>
        public static WebDriverConfigurationBuilder Start()
        {
            return new WebDriverConfigurationBuilder();
        }

        private WebDriverConfigurationBuilder()
        {
            browser = Browser.Firefox;
            platformType = PlatformType.Windows;
            windowSize = WindowSize.Hd;
            windowDefinedSize = Size.Empty;
            gridUri = new Uri("http://localhost:4444");
            isLocal = true;
            headless = false;
            languageCulture = null;
        }

        /// <summary>
        /// Generate the WebDriverConfiguration from the current state of the builder.
        /// </summary>
        /// <returns></returns>
        public WebDriverConfiguration Build()
        {
            WebDriverConfiguration webDriverConfiguration = new WebDriverConfiguration(
                browser, 
                gridUri, 
                headless, 
                isLocal, 
                platformType, 
                windowSize, 
                windowDefinedSize,
                languageCulture);
            return webDriverConfiguration;
        }

        /// <summary>
        /// Set the required Browser
        /// </summary>
        /// <param name="browser"></param>
        /// <returns></returns>
        public WebDriverConfigurationBuilder WithBrowser(Browser browser)
        {
            this.browser = browser;
            return this;
        }

        /// <summary>
        /// Set the required Uri for the Selenium Grid
        /// </summary>
        /// <param name="gridUri"></param>
        /// <returns></returns>
        public WebDriverConfigurationBuilder WithGridUri(Uri gridUri)
        {
            this.gridUri = gridUri;
            return this;
        }

        /// <summary>
        /// Set the Headless switch as required
        /// </summary>
        /// <param name="headless"></param>
        /// <returns></returns>
        public WebDriverConfigurationBuilder WithHeadless(bool headless)
        {
            this.headless = headless;
            return this;
        }

        /// <summary>
        /// Set the IsLocal switch as required
        /// </summary>
        /// <param name="isLocal"></param>
        /// <returns></returns>
        public WebDriverConfigurationBuilder WithIsLocal(bool isLocal)
        {
            this.isLocal = isLocal;
            return this;
        }

        /// <summary>
        /// Set the required PlatformType
        /// </summary>
        /// <param name="platformType"></param>
        /// <returns></returns>
        public WebDriverConfigurationBuilder WithPlatformType(PlatformType platformType)
        {
            this.platformType = platformType;
            return this;
        }

        /// <summary>
        /// Set the required value for the WindowSize Enum
        /// </summary>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        public WebDriverConfigurationBuilder WithWindowSize(WindowSize windowSize)
        {
            this.windowSize = windowSize;
            return this;
        }

        /// <summary>
        /// Set the required WindowDefinedSize (Caution: only used for WindowSize.Custom/ WindowSize.Defined)
        /// </summary>
        /// <param name="customWindowSize"></param>
        /// <returns></returns>
        public WebDriverConfigurationBuilder WithWindowDefinedSize(Size customWindowSize)
        {
            windowDefinedSize = customWindowSize;
            return this;
        }

        /// <summary>
        /// Shortcut method to request the given Custom Size
        /// </summary>
        /// <param name="customWindowSize"></param>
        /// <returns></returns>
        public WebDriverConfigurationBuilder WithCustomSize(Size customWindowSize)
        {
            return WithWindowSize(WindowSize.Defined).WithWindowDefinedSize(customWindowSize);
        }

        /// <summary>
        /// Shortcut method to request running remotely on the given Uri
        /// </summary>
        /// <param name="gridUri"></param>
        /// <returns></returns>
        public WebDriverConfigurationBuilder RunRemotelyOn(Uri gridUri)
        {
            this.gridUri = gridUri;
            isLocal = false;
            return this;
        }

        /// <summary>
        /// Set the Browser to run in headless mode
        /// </summary>
        /// <returns></returns>
        public WebDriverConfigurationBuilder RunHeadless()
        {
            return WithHeadless(true);
        }

        /// <summary>
        /// Set the requested browser language culture
        /// </summary>
        /// <param name="languageCulture"></param>
        /// <returns></returns>
        public WebDriverConfigurationBuilder WithLanguageCulture(CultureInfo languageCulture)
        {
            this.languageCulture = languageCulture;
            return this;
        }

        /// <summary>
        /// Return a formatted, human readable Json string suitable for saving locally in a Config_WebDriver.json file
        /// </summary>
        /// <returns></returns>
        public string GetJsonConfigString()
        {
            // Serialising using json.net produces minimal, non readable json: Manually generate a readable, formatted string.
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.AppendLine("{");
            jsonBuilder.AppendLine($"  \"Browser\": \"{browser.ToString()}\",");
            jsonBuilder.AppendLine($"  \"IsLocal\": {isLocal.ToString().ToLower()},");
            jsonBuilder.AppendLine($"  \"PlatformType\": \"{platformType.ToString()}\",");
            jsonBuilder.AppendLine($"  \"Headless\": {headless.ToString().ToLower()},");
            if (windowSize == WindowSize.Maximise || windowSize == WindowSize.Maximize || windowSize == WindowSize.Unchanged)
            {
                jsonBuilder.AppendLine($"  \"WindowSize\": \"{windowSize.ToString()}\",");
            }
            else
            {
                jsonBuilder.AppendLine($"  \"WindowSize\": \"{WindowSize.Defined.ToString()}\",");
                if (windowSize == WindowSize.Custom || windowSize == WindowSize.Defined)
                {
                    jsonBuilder.AppendLine($"  \"WindowDefinedSize\": {{\"width\":{windowDefinedSize.Width}, \"height\":{windowDefinedSize.Height}}},");
                }
                else
                {
                    jsonBuilder.AppendLine($"  \"WindowDefinedSize\": {{\"width\":{windowSize.Size().Width}, \"height\":{windowSize.Size().Height}}},");
                }
            }
            jsonBuilder.AppendLine($"  \"GridUri\": \"{gridUri}\"");
            if (languageCulture != null)
            {
                jsonBuilder.Append(",");
                jsonBuilder.AppendLine($"  \"LanguageCulture\": \"{languageCulture}\"");
            }
            jsonBuilder.AppendLine("}");
            return jsonBuilder.ToString();
        }
    }
}
