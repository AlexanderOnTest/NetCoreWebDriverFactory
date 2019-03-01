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

using System.IO;
using System.Reflection;

namespace AlexanderOnTest.NetCoreWebDriverFactory
{
    /// <summary>
    /// DriverPath object containing the local WebDriver executable path as a string
    /// </summary>
    public class DriverPath
    {
        /// <summary>
        /// For convenience when local WebDriver testing is not required. Use 'new DriverPath(Assembly.GetExecutingAssembly());' for local testing
        /// </summary>
        public DriverPath()
        {
            PathString = "";
        }


        /// <summary>
        /// For convenience when local WebDriver testing is not required. Use 'new DriverPath(Assembly.GetExecutingAssembly());' for local testing
        /// </summary>
        public DriverPath(string driverPath)
        {
            PathString = driverPath;
        }

        /// <summary>
        /// For testing locally call as 'new DriverPath(Assembly.GetExecutingAssembly());' 
        /// </summary>
        /// <param name="assembly"></param>
        public DriverPath(Assembly assembly)
        {
            PathString = Path.GetDirectoryName(assembly.Location);
        }

        /// <summary>
        /// Return the pathString for locating local WebDriver executables.
        /// </summary>
        public string PathString { get; }
    }
}
