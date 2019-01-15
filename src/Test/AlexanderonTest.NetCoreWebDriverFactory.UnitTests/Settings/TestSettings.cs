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
using AlexanderOnTest.NetCoreWebDriverFactory;
using NUnit.Framework;
using OpenQA.Selenium;

namespace AlexanderonTest.NetCoreWebDriverFactory.UnitTests.Settings
{
    /// <summary>
    /// This static class provides values to build a test configuration from a xxx.runsettings file.
    /// Default values are returned if a setting is not set or valid.
    /// </summary>
    internal static class TestSettings
    {
        internal static bool UseRealWebDriver { get; } = GetSettingAsBoolOrDefault("useRealWebDriver", false);

        internal static Browser Browser { get; } = GetEnumSettingOrDefault("browserType", Browser.Firefox);

        internal static Uri GridUri { get; } 
            = new Uri(GetSettingOrDefault("gridUri", "http://localhost:4444/wd/hub"));

        internal static bool IsLocal { get; } 
            = GetSettingAsBoolOrDefault("isLocal", true);

        internal static PlatformType PlatformType { get; } = GetEnumSettingOrDefault("platform", PlatformType.Windows);

        internal static WindowSize WindowSize { get; } = GetEnumSettingOrDefault("windowSize", WindowSize.Hd);

        internal static bool Headless { get; } = GetSettingAsBoolOrDefault("headless", false);

        internal static IWebDriverConfiguration WebDriverConfiguration { get; }
            = new WebDriverConfiguration
            {
                Browser = Browser,
                IsLocal = IsLocal,
                WindowSize = WindowSize,
                GridUri = GridUri,
                PlatformType = PlatformType,
                Headless = Headless
            };

        /// <summary>
        /// Returns the TestContext.Parameters value if it exists, else the defaultValue.
        /// </summary>
        /// <param name="settingName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private static string GetSettingOrDefault(string settingName, string defaultValue)
        {
            return TestContext.Parameters.Exists(settingName) ?
                TestContext.Parameters.Get(settingName) :
                defaultValue;
        }

        /// <summary>
        /// Returns the defaultValue unless the TestContext.Parameters exists and correctly negates it.
        /// </summary>
        /// <param name="settingName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private static bool GetSettingAsBoolOrDefault(string settingName, bool defaultValue)
        {
            return !TestContext.Parameters.Exists(settingName)
                   || TestContext.Parameters.Get(settingName).ToLower().Equals(defaultValue.ToString().ToLower()) ?
                defaultValue :
                !defaultValue;
        }

        /// <summary>
        /// Returns the TestContext.Parameters value if it exists and parses to a valid Enum value,
        /// otherwise returning the defaultValue.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="settingName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private static T GetEnumSettingOrDefault<T>(string settingName, T defaultValue)
        {
            T returnValue;
            if (!TestContext.Parameters.Exists(settingName))
            {
                returnValue = defaultValue;
            }
            else
            {
                try
                {
                    returnValue = (T)Enum.Parse(typeof(T), TestContext.Parameters.Get(settingName), true);
                }
                catch (Exception ex) when (ex is ArgumentException || ex is OverflowException)
                {
                    returnValue = defaultValue;
                }
            }
            return returnValue;
        }
    }
}
