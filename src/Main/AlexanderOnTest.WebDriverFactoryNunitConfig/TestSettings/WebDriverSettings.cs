﻿// <copyright>
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
using AlexanderOnTest.NetCoreWebDriverFactory;
using AlexanderOnTest.NetCoreWebDriverFactory.Config;
using AlexanderOnTest.NetCoreWebDriverFactory.Utils.Builders;
using AlexanderOnTest.WebDriverFactoryNunitConfig.Logging;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using OpenQA.Selenium;
using static AlexanderOnTest.WebDriverFactoryNunitConfig.TestSettings.Utils;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace AlexanderOnTest.WebDriverFactoryNunitConfig.TestSettings
{
    public static class WebDriverSettings
    {
        private static ILogger _logger = NunitConfigLogging.LoggerFactory.CreateLogger("Information");
        
        public static Browser Browser { get; } = GetEnumSettingOrDefault("browserType", Browser.Firefox);

        /// <summary>
        /// Uri of the grid. Configuration Priority:
        /// 1. A value provided in "My Documents/Config_GridUri.json" (Windows) or "/Config_GridUri.json" (Mac / Linux)
        /// 2. A value provided for GridUri in "My Documents/Config_WebDriver.json" (Windows) or "/Config_WebDriver.json" (Mac / Linux)
        /// 3. The value in an applied .runsettings file
        /// 4. Default (Localhost) grid.
        /// </summary>
        public static Uri GridUri
        {
            get
            {
                var gridUriFileValue = GetConfigFromFileSystemIfPresent<Uri>("Config_GridUri.json");
                if (gridUriFileValue != null)
                {
                    _logger.Log(LogLevel.Information,"GridUri successfully loaded from Config_GridUri.json");
                    return gridUriFileValue;
                }

                var webDriverFileConfig =
                    GetConfigFromFileSystemIfPresent<WebDriverConfiguration>("Config_WebDriver.json");
                if (webDriverFileConfig != null && webDriverFileConfig.GridUri != null)
                {
                    _logger.Log(LogLevel.Information,"GridUri successfully loaded from the value in Config_WebDriver.json");
                    return webDriverFileConfig.GridUri;
                }

                _logger.Log(LogLevel.Information,"Returning GridUri from the gridUri value in the applied runsettings or default");
                return new Uri(GetSettingOrDefault("gridUri", "http://localhost:4444"));
            }
        }

        /// <summary>
        /// Run the webdriver locally (rather than remote)Configuration Priority:
        /// 2. A value provided for IsLocal in "My Documents/Config_WebDriver.json" (Windows) or "/Config_WebDriver.json" (Mac / Linux)
        /// 3. The value in an applied .runsettings file
        /// 4. Default (Localhost) grid.
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
        /// Run the WebDriver instance Headless (Supported only on Chrome, Edge and Firefox)
        /// </summary>
        public static bool Headless { get; } = GetBoolSettingOrDefault("headless", false);

        /// <summary>
        /// Requested Custom browser size for WindowSize.Custom
        /// </summary>
        public static Size CustomWindowSize { get; } = new Size(
            TestContext.Parameters.Get("customWidth", 0 ), 
            TestContext.Parameters.Get("customHeight", 0));

        /// <summary>
        /// Requested language culture for the browser
        /// </summary>
        public static CultureInfo LanguageCulture
        {
            get
            {
                string languageCultureSetting = GetSettingOrDefault("languageCulture", null);
                return languageCultureSetting == null ? null : new CultureInfo(languageCultureSetting);
            }
        }

        /// <summary>
        /// Return the Configuration to use - Priority:
        /// 1. A value provided in "My Documents/Config_WebDriver.json" (Windows) or "/Config_WebDriver.json" (Mac / Linux)
        /// 2. The value in an applied .runsettings file
        /// 3. Default values.
        /// </summary>
        public static IWebDriverConfiguration WebDriverConfiguration 
            => GetConfigFromFileSystemIfPresent<WebDriverConfiguration>("Config_WebDriver.json") 
               ?? WebDriverConfigurationBuilder.Start()
                .WithBrowser(Browser)
                .WithGridUri(GridUri)
                .WithHeadless(Headless)
                .WithIsLocal(IsLocal)
                .WithPlatformType(PlatformType)
                .WithWindowSize(WindowSize)
                .WithWindowDefinedSize(CustomWindowSize)
                .WithLanguageCulture(LanguageCulture)
                .Build();
    }

}