﻿// <copyright>
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
    /// Default LocalWebDriverFactory implementation for .NET Core projects  using Internet Explorer for IE testing (e.g. Windows 10). 
    /// </summary>
    public class InternetExplorerLocalWebDriverFactory : LocalWebDriverFactoryBase
    {
        /// <summary>
        /// Return a DriverFactory instance for use in .NET Core projects using Internet Explorer for IE testing.
        /// Try using installedDriverPath = "Path.GetDirectoryName(Assembly.GetCallingAssembly().Location)" when running from .NET core projects.
        /// </summary>
        /// <param name="installedDriverPath"></param>
        /// <param name="driverOptionsFactory"></param>
        /// <param name="webDriverReSizer"></param>
        public InternetExplorerLocalWebDriverFactory(
            IDriverOptionsFactory driverOptionsFactory,
            string installedDriverPath,
            IWebDriverReSizer webDriverReSizer) : base(driverOptionsFactory, installedDriverPath, webDriverReSizer, false)
        {
        }

        /// <summary>
        /// Return a DriverFactory instance for use in .NET Core projects using Internet Explorer for IE testing.
        /// Try using driverPath = new DriverPath(Assembly.GetCallingAssembly()) when testing locally from .NET core projects.
        /// </summary>
        /// <param name="driverPath"></param>
        /// <param name="driverOptionsFactory"></param>
        /// <param name="webDriverReSizer"></param>
        public InternetExplorerLocalWebDriverFactory(IDriverOptionsFactory driverOptionsFactory,
                                            DriverPath driverPath,
                                            IWebDriverReSizer webDriverReSizer)
            : base(driverOptionsFactory, driverPath?.PathString, webDriverReSizer, false)
        {
        }
    }
}