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

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Safari;

namespace AlexanderOnTest.NetCoreWebDriverFactory
{
    /// <summary>
    /// Interface for DriverOptionsFactory instances.
    /// </summary>
    public interface IDriverOptionsFactory
    {
        /// <summary>
        /// Return a configured ChromeOptions instance.
        /// </summary>
        /// <param name="platformType"></param>
        /// <returns></returns>
        ChromeOptions GetChromeOptions(PlatformType platformType = PlatformType.Any);

        /// <summary>
        /// Return a configured ChromeOptions instance.
        /// </summary>
        /// <param name="headless"></param>
        /// <param name="platformType"></param>
        /// <returns></returns>
        ChromeOptions GetChromeOptions(bool headless = false, PlatformType platformType = PlatformType.Any);

        /// <summary>
        /// Return a configured FirefoxOptions instance.
        /// </summary>
        /// <param name="platformType"></param>
        /// <returns></returns>
        FirefoxOptions GetFirefoxOptions(PlatformType platformType = PlatformType.Any);

        /// <summary>
        /// Return a configured FirefoxOptions instance.
        /// </summary>
        /// <param name="headless"></param>
        /// <param name="platformType"></param>
        /// <returns></returns>
        FirefoxOptions GetFirefoxOptions(bool headless = false, PlatformType platformType = PlatformType.Any);

        /// <summary>
        /// Return a configured EdgeOptions instance.
        /// </summary>
        /// <param name="platformType"></param>
        /// <returns></returns>
        EdgeOptions GetEdgeOptions(PlatformType platformType = PlatformType.Any);

        /// <summary>
        /// Return a configured InternetExplorerOptions instance.
        /// </summary>
        /// <param name="platformType"></param>
        /// <returns></returns>
        InternetExplorerOptions GetInternetExplorerOptions(PlatformType platformType = PlatformType.Any);

        /// <summary>
        /// Return a configured SafariOptions instance.
        /// </summary>
        /// <param name="platformType"></param>
        /// <returns></returns>
        SafariOptions GetSafariOptions(PlatformType platformType = PlatformType.Any);

        /// <summary>
        /// Add the platform configuration to a DriverOptions instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="options"></param>
        /// <param name="platformType"></param>
        /// <returns></returns>
        T SetPlatform<T>(T options, PlatformType platformType) where T : DriverOptions;

    }
}
