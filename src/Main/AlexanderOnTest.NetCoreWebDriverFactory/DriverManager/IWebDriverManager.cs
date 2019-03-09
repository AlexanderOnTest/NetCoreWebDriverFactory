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
using OpenQA.Selenium;

// The WebDriverManager was first released in v2.1.0 with this incorrect namespace.
// I prefer this file structure, but do not wish to change the API.
// DO NOT CORRECT THIS WITHOUT A MAJOR VERSION BUMP.
namespace AlexanderOnTest.NetCoreWebDriverFactory
{
    /// <summary>
    /// Interface for a WebDriver Manager class that provides a singleton
    /// </summary>
    public interface IWebDriverManager : IDisposable
    {
        /// <summary>
        /// Return a singleton WebDriver instance;
        /// </summary>
        /// <returns></returns>
        IWebDriver Get();

        /// <summary>
        /// Quit and clear the current singleton WebDriver instance;
        /// </summary>
        IWebDriver Quit();

        /// <summary>
        /// Return a new WebDriver instance independent of the singleton instance;
        /// </summary>
        /// <returns></returns>
        IWebDriver GetAdditionalWebDriver();
    }
}
