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
using NUnit.Framework;

namespace AlexanderOnTest.WebDriverFactoryNunitConfig.TestSettings
{
    public static class Utils
    {

        public static string GetStringSettingOrDefault(string settingName, string defaultValue)
        {
            return TestContext.Parameters.Exists(settingName) ?
                TestContext.Parameters.Get(settingName) :
                defaultValue;
        }

        public static bool GetBoolSettingOrDefault(string settingName, bool defaultValue)
        {
            return !TestContext.Parameters.Exists(settingName)
                   || TestContext.Parameters.Get(settingName).ToLower().Equals(defaultValue.ToString().ToLower()) ?
                defaultValue :
                !defaultValue;
        }

        public static T GetEnumSettingOrDefault<T>(string settingName, T defaultValue)
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
