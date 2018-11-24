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

namespace AlexanderOnTest.NetCoreWebDriverFactory
{
    /// <summary>
    /// Enum of Browser window size configurations.
    /// </summary>
    public enum WindowSize
    {
        /// <summary>
        /// HD - 1366 x 768 (cheap laptop) screen size. Most common 'PC' screen size.
        /// </summary>
        Hd,
        /// <summary>
        /// Full HD - 1920 x 1080 screen size. 2nd most common 'PC' screen size.
        /// </summary>
        Fhd,
        /// <summary>
        /// Maximise the browser to the full screen.
        /// </summary>
        Maximise,
        /// <summary>
        /// Continue without setting screen size, this may not be consistent.
        /// </summary>
        Unchanged
    }
}
