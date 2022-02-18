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
using System.Drawing;

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
        Unchanged,
        /// <summary>
        /// Maximize the browser to the full screen.
        /// </summary>
        Maximize,
        /// <summary>
        /// Customise browser size according to the windowCustomSize configuration.
        /// </summary>
        Custom,
        /// <summary>
        /// Quad HD (aka 1440p) - 2560 x 1440
        /// </summary>
        Qhd,
        /// <summary>
        /// Ultra HD-1 (aka 4k) - 3840 x 2160
        /// </summary>
        Uhd,
        /// <summary>
        /// Implemented form for all defined size browsers
        /// </summary>
        Defined
    }

    /// <summary>
    /// Extension methods for the WindowSize Enum.
    /// </summary>
    public static class WindowSizeExtension
    {
        private static readonly Dictionary<WindowSize, Size> Sizes = new Dictionary<WindowSize, Size>();


        static WindowSizeExtension()
        {
            Sizes.Add(WindowSize.Hd, new Size(1366, 768));
            Sizes.Add(WindowSize.Fhd, new Size(1920, 1080));
            Sizes.Add(WindowSize.Qhd, new Size(2560, 1440));
            Sizes.Add(WindowSize.Uhd, new Size(3840, 2160));
        }

        /// <summary>
        /// Return the requested browser window size for the Enum value, or (0,0) if size is not specified
        /// </summary>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        public static Size Size(this WindowSize windowSize)
        {
            return windowSize.HasDefinedSize() ? Sizes[windowSize] : default;
        }

        /// <summary>
        /// Does this WindowSize value request a given size for the generated WebDriver?
        /// </summary>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        public static bool HasDefinedSize(this WindowSize windowSize)
        {
            return Sizes.ContainsKey(windowSize);
        }
    }
}
