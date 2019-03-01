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
using AlexanderOnTest.NetCoreWebDriverFactory.DriverOptionsFactory;

namespace AlexanderOnTest.NetCoreWebDriverFactory
{
    /// <summary>
    /// Overridable implementation of the IWebDriverFactory interface for use with .Net Framework test projects.
    /// </summary>
    public class FrameworkWebDriverFactory : DefaultWebDriverFactory
    {
        /// <summary>
        /// Return a WebDriverFactory instance for use with .NET framework projects
        /// </summary>
        /// <param name="gridUri"></param>
        /// <param name="driverOptionsFactory"></param>
        public FrameworkWebDriverFactory(Uri gridUri = null, IDriverOptionsFactory driverOptionsFactory = null) : base((string) null, gridUri, driverOptionsFactory) { }

        /// <summary>
        /// Return a WebDriverFactory instance for use with .NET framework projects
        /// </summary>
        /// <param name="driverPath"></param>
        /// <param name="configuration"></param>
        /// <param name="driverOptionsFactory"></param>
        public FrameworkWebDriverFactory(DriverPath driverPath, IWebDriverConfiguration configuration, IDriverOptionsFactory driverOptionsFactory = null)
            : this(configuration.GridUri, driverOptionsFactory)
        {
        }
    }
}
