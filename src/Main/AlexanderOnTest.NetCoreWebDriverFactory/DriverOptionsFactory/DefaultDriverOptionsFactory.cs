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
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Safari;

namespace AlexanderOnTest.NetCoreWebDriverFactory.DriverOptionsFactory
{
    /// <summary>
    /// An overridable implementation of the IDriverOptionsFactory Interface.
    /// </summary>
    public class DefaultDriverOptionsFactory : IDriverOptionsFactory
    {
        /// <summary>
        /// Construct a default DriverOptionsFactory.
        /// </summary>
        public DefaultDriverOptionsFactory()
        {
            this.DriverOptionsDictionary = new Dictionary<Type, DriverOptions>
            {
                {typeof(ChromeOptions), StaticDriverOptionsFactory.GetChromeOptions()},
                {typeof(EdgeOptions), StaticDriverOptionsFactory.GetEdgeOptions()},
                {typeof(FirefoxOptions), StaticDriverOptionsFactory.GetFirefoxOptions()},
                {typeof(InternetExplorerOptions), StaticDriverOptionsFactory.GetInternetExplorerOptions()},
                {typeof(SafariOptions), StaticDriverOptionsFactory.GetSafariOptions()}
            };
        }

        /// <summary>
        /// Constructor to override the default DriverOptions.
        /// </summary>
        /// <param name="driverOptionsDictionary"></param>
        public DefaultDriverOptionsFactory(Dictionary<Type, DriverOptions> driverOptionsDictionary)
        {
            DriverOptionsDictionary = driverOptionsDictionary;
        }

        /// <summary>
        /// Dictionary of basically configured DriverOptions instances.
        /// </summary>
        protected Dictionary<Type, DriverOptions> DriverOptionsDictionary { get; }

        /// <summary>
        /// <para>Return a DriverOptions instance of the correct type configured for a Local WebDriver.</para>
        /// <para>Defaults to headless mode where available.</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="headless"></param>
        /// <returns></returns>
        public T GetLocalDriverOptions<T>(bool headless = true) where T : DriverOptions
        {
            Type type = typeof(T);
            DriverOptionsDictionary.TryGetValue(type, out DriverOptions driverOptions);
            T options = driverOptions as T;
            return headless ? AddHeadlessOption(options) : options;
        }

        /// <summary>
        /// <para>Return a DriverOptions instance of the correct type configured for a Remote WebDriver.</para>
        /// <para>Defaults to headless mode where available.</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="platformType"></param>
        /// <param name="headless"></param>
        /// <returns></returns>
        public T GetRemoteDriverOptions<T>(PlatformType platformType, bool headless = true) where T : DriverOptions
        {
            Type type = typeof(T);
            DriverOptionsDictionary.TryGetValue(type, out DriverOptions driverOptions);
            SetPlatform(driverOptions, platformType);
            T options = driverOptions as T;
            return headless ? AddHeadlessOption(options) : options;
        }

        /// <summary>
        /// Add the platform configuration to a DriverOptions instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="options"></param>
        /// <param name="platformType"></param>
        /// <returns></returns>
        protected virtual T SetPlatform<T>(T options, PlatformType platformType) where T : DriverOptions
        {
            return StaticDriverOptionsFactory.SetPlatform(options, platformType);
        }

        /// <summary>
        /// Add the headless flag if available.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="driverOptions"></param>
        /// <returns></returns>
        protected virtual T AddHeadlessOption<T>(T driverOptions) where T : DriverOptions
        {
            return StaticDriverOptionsFactory.AddHeadlessOption(driverOptions);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // nothing to dispose of
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
