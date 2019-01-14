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
using System.Text;
using OpenQA.Selenium;

namespace AlexanderOnTest.NetCoreWebDriverFactory
{
    public interface IWebDriverConfiguration
    {
        /// <summary>
        /// Browser type to request.
        /// </summary>
        Browser Browser
        {
            get;
            set;
        }

        /// <summary>
        /// Platform to request for a RemoteWebDriver
        /// </summary>
        PlatformType PlatformType
        {
            get;
            set;
        }

        /// <summary>
        /// WindowSize to request
        /// </summary>
        WindowSize WindowSize
        {
            get;
            set;
        }

        Uri GridUri
        {
            get;
            set;
        }

        bool IsLocal
        {
            get;
            set;
        }

        bool Headless
        {
            get;
            set;
        }
    }
}
