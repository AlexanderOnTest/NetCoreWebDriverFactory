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

using AlexanderOnTest.NetCoreWebDriverFactory.DriverOptionsFactory;
using AlexanderOnTest.NetCoreWebDriverFactory.Utils;

namespace AlexanderOnTest.NetCoreWebDriverFactory.WebDriverFactory
{
    /// <summary>
    /// Overridable implementation of the ILocalWebDriverFactory interface for use with .Net Framework test projects.
    /// </summary>
    public class FrameworkLocalWebDriverFactory : DefaultLocalWebDriverFactory
    {
        /// <summary>
        /// Return a WebDriverFactory instance for use with .NET framework projects
        /// </summary>
        /// <param name="driverOptionsFactory"></param>
        /// <param name="webDriverReSizer"></param>
        public FrameworkLocalWebDriverFactory(IDriverOptionsFactory driverOptionsFactory, IWebDriverReSizer webDriverReSizer) 
            : base(driverOptionsFactory, (string) null, webDriverReSizer) { }
    }
}
