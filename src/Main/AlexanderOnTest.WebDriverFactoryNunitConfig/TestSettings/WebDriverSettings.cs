using System;
using System.IO;
using AlexanderOnTest.NetCoreWebDriverFactory;
using Newtonsoft.Json;
using OpenQA.Selenium;
using static AlexanderOnTest.WebDriverFactoryNunitConfig.TestSettings.Utils;

namespace AlexanderOnTest.WebDriverFactoryNunitConfig.TestSettings
{
    public static class WebDriverSettings
    {
        public static Browser Browser { get; } = GetEnumSettingOrDefault("browserType", Browser.Firefox);

        /// <summary>
        /// Uri of the grid. Configurtion Priority:
        /// 1. A value provided in "My Documents/Config_GridUri.json" (Windows) or "/Config_GridUri.json" (Mac / Linux)
        /// 2. The value in an applied .runsettings file
        /// 3. Default Localhost grid.
        /// </summary>
        public static Uri GridUri { get; }
            = GetLocalGridUri() ??
              new Uri(GetStringSettingOrDefault("gridUri", "http://localhost:4444/wd/hub"));

        public static bool IsLocal { get; }
            = GetBoolSettingOrDefault("isLocal", true);

        public static PlatformType PlatformType { get; } = GetEnumSettingOrDefault("platform", PlatformType.Windows);

        public static WindowSize WindowSize { get; } = GetEnumSettingOrDefault("windowSize", WindowSize.Hd);

        public static bool Headless { get; } = GetBoolSettingOrDefault("headless", false);

        public static IWebDriverConfiguration WebDriverConfiguration { get; }
            = GetLocalConfig() ??
              new WebDriverConfiguration
            {
                Browser = Browser,
                IsLocal = IsLocal,
                WindowSize = WindowSize,
                GridUri = GridUri,
                PlatformType = PlatformType,
                Headless = Headless
            };

        private static Uri GetLocalGridUri()
        {
            var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string configFile = $@"{folderPath}\Config_GridUri.json";

            string localGridUriString = null;
            if (!string.IsNullOrEmpty(folderPath) && File.Exists(configFile))
            {
                using (StreamReader file = File.OpenText(configFile))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    localGridUriString = (string)serializer.Deserialize(file, typeof(string));
                }
            }

            return string.IsNullOrEmpty(localGridUriString) ?
                null :
                new Uri(localGridUriString);
        }

        /// <summary>
        /// Return a WebDriverConfiguration object deserialised from:
        /// "My Documents/Config_WebDriver.json" (Windows) or "/Config_WebDriver.json" (Mac / Linux) if present
        /// </summary>
        /// <returns></returns>
        private static IWebDriverConfiguration GetLocalConfig()
        {
            IWebDriverConfiguration configFromHome = null;

            var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string configFile = $@"{folderPath}\Config_WebDriver.json";

            if (!string.IsNullOrEmpty(folderPath) && File.Exists(configFile))
            {
                using (StreamReader file = File.OpenText(configFile))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    configFromHome = (IWebDriverConfiguration)serializer.Deserialize(file, typeof(WebDriverConfiguration));
                }
            }

            return configFromHome;
        }
    }
}