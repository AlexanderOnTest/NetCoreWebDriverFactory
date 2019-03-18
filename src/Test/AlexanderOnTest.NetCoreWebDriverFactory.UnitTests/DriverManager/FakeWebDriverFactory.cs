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
using System.Reflection;
using log4net;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Safari;
using LogProvider = AlexanderOnTest.NetCoreWebDriverFactory.Logging.LogProvider;

namespace AlexanderOnTest.NetCoreWebDriverFactory.UnitTests.DriverManager
{
    class FakeWebDriverFactory : IWebDriverFactory
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public Uri GridUri { get; set; }

        public IWebDriver GetRemoteWebDriver(Browser browser, PlatformType platformType = PlatformType.Any,
            WindowSize windowSize = WindowSize.Hd, bool headless = false, Size windowCustomSize = new Size())
        {
            throw new NotImplementedException();
        }

        public IWebDriver GetRemoteWebDriver(IWebDriverConfiguration configuration)
        {
            throw new NotImplementedException();
        }

        public IWebDriver GetRemoteWebDriver(DriverOptions options, WindowSize windowSize = WindowSize.Hd, Size windowCustomSize = new Size())
        {
            throw new NotImplementedException();
        }

        public IWebDriver GetLocalWebDriver(Browser browser, WindowSize windowSize = WindowSize.Hd, bool headless = false, Size windowCustomSize = new Size())
        {
            throw new NotImplementedException();
        }

        public IWebDriver GetLocalWebDriver(IWebDriverConfiguration configuration)
        {
            throw new NotImplementedException();
        }

        public IWebDriver GetLocalWebDriver(ChromeOptions options, WindowSize windowSize = WindowSize.Hd, Size windowCustomSize = new Size())
        {
            throw new NotImplementedException();
        }

        public IWebDriver GetLocalWebDriver(FirefoxOptions options, WindowSize windowSize = WindowSize.Hd, Size windowCustomSize = new Size())
        {
            throw new NotImplementedException();
        }

        public IWebDriver GetLocalWebDriver(EdgeOptions options, WindowSize windowSize = WindowSize.Hd, Size windowCustomSize = new Size())
        {
            throw new NotImplementedException();
        }

        public IWebDriver GetLocalWebDriver(InternetExplorerOptions options, WindowSize windowSize = WindowSize.Hd, Size windowCustomSize = new Size())
        {
            throw new NotImplementedException();
        }

        public IWebDriver GetLocalWebDriver(SafariOptions options, WindowSize windowSize = WindowSize.Hd, Size windowCustomSize = new Size())
        {
            throw new NotImplementedException();
        }

        public IWebDriver GetWebDriver(Browser browser, WindowSize windowSize = WindowSize.Hd, bool isLocal = true,
            PlatformType platformType = PlatformType.Any, bool headless = false, Size windowCustomSize = new Size())
        {
            Logger.Info($"Fake WebDriver requested.");
            return new FakeWebDriver();
        }

        public IWebDriver GetWebDriver(IWebDriverConfiguration configuration)
        {
            Logger.Info($"Fake WebDriver requested.");

            return new FakeWebDriver();
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