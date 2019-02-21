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
using AlexanderOnTest.NetCoreWebDriverFactory;
using OpenQA.Selenium;
using static AlexanderOnTest.WebDriverFactoryNunitConfig.TestSettings.Utils;

namespace AlexanderOnTest.WebDriverFactoryNunitConfig.TestSettings
{
    public static class WebDriverSettings
    {
        public static Browser Browser { get; } = GetEnumSettingOrDefault("browserType", Browser.Firefox);

        /// <summary>
        /// Uri of the grid. Configuration Priority:
        /// 1. A value provided in "My Documents/Config_GridUri.json" (Windows) or "/Config_GridUri.json" (Mac / Linux)
        /// 2. The value in an applied .runsettings file
        /// 3. Default (Localhost) grid.
        /// </summary>
        public static Uri GridUri { get; }
            = Utils.GetConfigFromFileSystemIfPresent<Uri>("Config_GridUri.json") ??
              new Uri(GetSettingOrDefault("gridUri", "http://localhost:4444/wd/hub"));

        /// <summary>
        /// Run the webdriver locally (rather than remote)
        /// </summary>
        public static bool IsLocal { get; }
            = GetBoolSettingOrDefault("isLocal", true);

        /// <summary>
        /// PlatformType for the RemoteWebDriver request
        /// </summary>
        public static PlatformType PlatformType { get; } = GetEnumSettingOrDefault("platform", PlatformType.Windows);

        /// <summary>
        /// Requested Browser Window Size
        /// </summary>
        public static WindowSize WindowSize { get; } = GetEnumSettingOrDefault("windowSize", WindowSize.Hd);

        /// <summary>
        /// Run the WebDriver instance Headless (Supported only on Firefox and Chrome)
        /// </summary>
        public static bool Headless { get; } = GetBoolSettingOrDefault("headless", false);

        /// <summary>
        /// Return the Configuration to use - Priority:
        /// 1. A value provided in "My Documents/Config_WebDriver.json" (Windows) or "/Config_WebDriver.json" (Mac / Linux)
        /// 2. The value in an applied .runsettings file
        /// 3. Default values.
        /// </summary>
        public static IWebDriverConfiguration WebDriverConfiguration { get; }
            = Utils.GetConfigFromFileSystemIfPresent<WebDriverConfiguration>("Config_WebDriver.json") ??
              new WebDriverConfiguration
            {
                Browser = Browser,
                IsLocal = IsLocal,
                WindowSize = WindowSize,
                GridUri = GridUri,
                PlatformType = PlatformType,
                Headless = Headless
            };
    }
}