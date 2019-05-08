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
using OpenQA.Selenium;

namespace AlexanderOnTest.NetCoreWebDriverFactory.Config
{
    /// <summary>
    /// Interface for a WebDriver Configuration object.
    /// </summary>
    public interface IWebDriverConfiguration
    {
        /// <summary>
        /// Browser type to request.
        /// </summary>
        Browser Browser
        {
            get;
        }

        /// <summary>
        /// Platform to request for a RemoteWebDriver.
        /// </summary>
        PlatformType PlatformType
        {
            get;
        }

        /// <summary>
        /// Defined WindowSize to request.
        /// </summary>
        WindowSize WindowSize
        {
            get;
        }


        /// <summary>
        /// Actual window size requested (if not Maximize/Maximise or Unchanged)
        /// </summary>
        Size WindowDefinedSize
        {
            get;
        }

        /// <summary>
        /// The Uri of the Selenium grid to use for remote calls.
        /// </summary>
        Uri GridUri
        {
            get;
        }

        /// <summary>
        /// Use a local WebDriver.
        /// </summary>
        bool IsLocal
        {
            get;
        }

        /// <summary>
        /// Run headless if available.
        /// </summary>
        bool Headless
        {
            get;
        }
    }
}
