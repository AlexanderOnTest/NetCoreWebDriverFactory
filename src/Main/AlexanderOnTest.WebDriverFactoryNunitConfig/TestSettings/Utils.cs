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
using System.IO;
using AlexanderOnTest.NetCoreWebDriverFactory.Utils.Converters;
using AlexanderOnTest.WebDriverFactoryNunitConfig.Logging;
using Newtonsoft.Json;
using NUnit.Framework;

namespace AlexanderOnTest.WebDriverFactoryNunitConfig.TestSettings
{
    public static class Utils
    {
        /// <summary>
        /// Return the string value of the setting in the applied .runsettings file or the passed in default.
        /// </summary>
        /// <param name="settingName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetSettingOrDefault(string settingName, string defaultValue)
        {
            return TestContext.Parameters.Exists(settingName) ?
                TestContext.Parameters.Get(settingName) :
                defaultValue;
        }

        /// <summary>
        /// Return the Enum value parsed from the setting in the applied .runsettings file, or the passed in default.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="settingName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T GetEnumSettingOrDefault<T>(string settingName, T defaultValue) where T : Enum
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

        /// <summary>
        /// Return
        /// </summary>
        /// <param name="settingName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int GetIntSettingOrDefault(string settingName, int defaultValue)
        {
            int returnValue;
            if (!TestContext.Parameters.Exists(settingName))
            {
                returnValue = defaultValue;
            }
            else
            {
                try
                {
                    returnValue = TestContext.Parameters.Get<int>(settingName, defaultValue);
                }
                catch (Exception ex) when (ex is ArgumentException || ex is OverflowException)
                {
                    returnValue = defaultValue;
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Return the passed in default unless the setting in the applied .runsettings file is !default.
        /// </summary>
        /// <param name="settingName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static bool GetBoolSettingOrDefault(string settingName, bool defaultValue)
        {
            return !TestContext.Parameters.Exists(settingName)
                   || TestContext.Parameters.Get(settingName).ToLower().Equals(defaultValue.ToString().ToLower()) ?
                defaultValue :
                !defaultValue;
        }

        /// <summary>
        /// Return the object deserialised from the given json file if present in the given folder
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filename"></param>
        /// <param name="fileLocation"></param>
        /// <returns></returns>
        public static T GetConfigFromFileSystemIfPresent<T>(
            string filename,
            Environment.SpecialFolder fileLocation = Environment.SpecialFolder.MyDocuments)
        {
            string fileLocationPath = Environment.GetFolderPath(fileLocation);
            string configFilePath = Path.Combine(fileLocationPath, filename);

            if (string.IsNullOrEmpty(configFilePath) || !File.Exists(configFilePath)) return default;

            T localConfig;
            using (StreamReader file = File.OpenText(configFilePath))
            {
                string json = file.ReadToEnd();

                localConfig = JsonConvert.DeserializeObject<T>(json, new SizeJsonConverter());
            }
            
            return localConfig;
        }
    }
}
