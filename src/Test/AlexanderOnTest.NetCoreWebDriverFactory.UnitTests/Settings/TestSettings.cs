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

using static AlexanderOnTest.WebDriverFactoryNunitConfig.TestSettings.Utils;

namespace AlexanderOnTest.NetCoreWebDriverFactory.UnitTests.Settings
{
    /// <summary>
    /// This static class provides values to build a test configuration from a xxx.runsettings file.
    /// Default values are returned if a setting is not set or valid.
    /// </summary>
    internal static class TestSettings
    {
        internal static bool UseRealWebDriver { get; } = GetBoolSettingOrDefault("useRealWebDriver", false);
    }
}
