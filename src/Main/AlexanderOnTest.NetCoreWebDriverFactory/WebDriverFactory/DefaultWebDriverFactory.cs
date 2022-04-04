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
using AlexanderOnTest.NetCoreWebDriverFactory.Logging;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace AlexanderOnTest.NetCoreWebDriverFactory.WebDriverFactory
{
    /// <summary>
    /// Implementation of the IWebDriverFactory interface for .NET Core test projects.
    /// </summary>
    public class DefaultWebDriverFactory : IWebDriverFactory
    {
        private static readonly ILogger Logger = WebDriverFactoryLogging.LoggerFactory?.CreateLogger("DefaultWebDriverFactory");
        private static readonly bool IsDebugEnabled = Logger != null && Logger.IsEnabled(LogLevel.Debug);
        private static readonly bool IsInfoEnabled = Logger != null && Logger.IsEnabled(LogLevel.Information);

        /// <summary>
        /// Return a DriverFactory instance.
        /// </summary>
        /// <param name="localWebDriverFactory"></param>
        /// <param name="remoteWebDriverFactory"></param>
        public DefaultWebDriverFactory(
            ILocalWebDriverFactory localWebDriverFactory,
            IRemoteWebDriverFactory remoteWebDriverFactory)
        {
            LocalWebDriverFactory = localWebDriverFactory;
            RemoteWebDriverFactory = remoteWebDriverFactory;
        }

        /// <summary>
        /// The RemoteWebDriverFactory instance to use.
        /// </summary>
        public IRemoteWebDriverFactory RemoteWebDriverFactory { get; set; }

        /// <summary>
        /// The LocalWebDriverFactory instance to use.
        /// </summary>
        public ILocalWebDriverFactory LocalWebDriverFactory { get; set; }

        /// <summary>
        /// Return a WebDriver instance of the given configuration.
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="windowSize"></param>
        /// <param name="isLocal"></param>
        /// <param name="platformType"></param>
        /// <param name="headless"></param>
        /// <param name="windowCustomSize"></param>
        /// <param name="requestedCulture"></param>
        /// <returns></returns>
        public IWebDriver GetWebDriver(
            Browser browser, 
            WindowSize windowSize = WindowSize.Hd, 
            bool isLocal = true,
            PlatformType platformType = PlatformType.Any, 
            bool headless = false, 
            Size windowCustomSize = new Size(),
            CultureInfo requestedCulture = null)
        {
            string configurationDescription = new StringBuilder()
                .Append($"Browser: {browser.ToString()}")
                .Append(headless ? "headless" : "on screen")
                .Append($"WindowSize enum: {windowSize.ToString()}, ")
                .Append($"{((windowSize == WindowSize.Custom || windowSize == WindowSize.Defined) ? $"Size: {windowCustomSize.Width} x {windowCustomSize.Height}, " : string.Empty)}")
                .Append(isLocal ? "running locally)" : $"running remotely on {RemoteWebDriverFactory.GridUri} on platform: {platformType}.)")
                .Append(requestedCulture != null ? $"\", Requested language culture\": \"{requestedCulture}\")" : ")")
                .ToString();

            if (IsInfoEnabled)
            {
                const string messagePattern = "WebDriver requested using parameters: {0}";
                var messageParameters = new object[] { configurationDescription };
                Logger.LogInformation(messagePattern, messageParameters);
            }
            
            IWebDriver driver = isLocal ?
                LocalWebDriverFactory.GetWebDriver(browser, windowSize, headless, windowCustomSize) :
                RemoteWebDriverFactory.GetWebDriver(browser, platformType, windowSize, headless, windowCustomSize);

            if (IsDebugEnabled)
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                if (IsDebugEnabled)
                {
                    const string messagePattern = "{0} successfully launched. Agent String: {1}";
                    object[] messageParameters = { driver, js.ExecuteScript("return navigator.userAgent;") };
                    Logger.LogDebug(messagePattern, messageParameters);
                }
            }

            return driver;
        }

        /// <summary>
        /// Return a WebDriver instance of the given configuration.
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public IWebDriver GetWebDriver(IWebDriverConfiguration configuration)
        {
            if (IsInfoEnabled)
            {
                const string messagePattern = "WebDriver requested using {0}";
                var messageParameters = new object[] { configuration.ToString() };
                Logger.LogInformation(messagePattern, messageParameters);
            }
            
            IWebDriver driver = configuration.IsLocal ?
                LocalWebDriverFactory.GetWebDriver(configuration) :
                RemoteWebDriverFactory.GetWebDriver(configuration);

            if (IsDebugEnabled)
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                if (IsDebugEnabled)
                {
                    const string messagePattern = "{0} successfully launched. Agent String: {1}";
                    object[] messageParameters = { driver, js.ExecuteScript("return navigator.userAgent;") };
                    Logger.LogDebug(messagePattern, messageParameters);
                }
            }
            
            return driver;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                LocalWebDriverFactory?.Dispose();
                RemoteWebDriverFactory?.Dispose();
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
