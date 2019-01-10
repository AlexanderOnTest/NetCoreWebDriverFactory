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
using System.Collections.ObjectModel;
using System.Text;
using OpenQA.Selenium;

namespace AlexanderonTest.NetCoreWebDriverFactory.UnitTests.DriverManager
{
#pragma warning disable S3881 // "IDisposable" should be implemented correctly
    internal class FakeWebDriver : IWebDriver
#pragma warning restore S3881 // "IDisposable" should be implemented correctly
    {
        private bool isQuit = false;

        public IWebElement FindElement(By @by)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IWebElement> FindElements(By @by)
        {
            throw new NotImplementedException();
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

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void Quit()
        {
            isQuit = true;
        }

        public IOptions Manage()
        {
            // to simulate the behaviour of a quit WebDriver: a WebDriverException is thrown when a call is made to "driver.Manage().Window.FullScreen()"
            if (isQuit)
            {
                throw new WebDriverException();
            }

            return null;
        }

        public INavigation Navigate()
        {
            throw new NotImplementedException();
        }

        public ITargetLocator SwitchTo()
        {
            throw new NotImplementedException();
        }
        
        public string Url { get; set; }
        public string Title { get; }
        public string PageSource { get; }
        public string CurrentWindowHandle { get; }
        public ReadOnlyCollection<string> WindowHandles { get; }
    }
}
