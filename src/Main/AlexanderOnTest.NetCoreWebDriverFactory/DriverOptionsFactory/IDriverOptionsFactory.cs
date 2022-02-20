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
using OpenQA.Selenium;

namespace AlexanderOnTest.NetCoreWebDriverFactory.DriverOptionsFactory
{
    /// <summary>
    /// Interface for DriverOptionsFactory instances.
    /// </summary>
    public interface IDriverOptionsFactory : IDisposable
    {
        /// <summary>
        /// <para>Return a DriverOptions instance of the correct type configured for a Local WebDriver.</para>
        /// <para>Defaults to headless mode where available.</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="headless"></param>
        /// <returns></returns>
        T GetLocalDriverOptions<T>(bool headless = true) where T : DriverOptions;

        /// <summary>
        /// <para>Return a DriverOptions instance of the correct type configured for a Remote WebDriver.</para>
        /// <para>Defaults to headless mode where available.</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="platformType"></param>
        /// <param name="headless"></param>
        /// <returns></returns>
        T GetRemoteDriverOptions<T>(PlatformType platformType, bool headless = true) where T : DriverOptions;
    }
}

