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
using System.ComponentModel;
using System.Drawing;
using Newtonsoft.Json;
using OpenQA.Selenium;

namespace AlexanderOnTest.NetCoreWebDriverFactory
{
    /// <summary>
    /// A WebDriver Configuration implementation.
    /// </summary>
    public class WebDriverConfiguration :IWebDriverConfiguration
    {
        private Size windowCustomSize;
        
        /// <summary>
        /// Browser type to request.
        /// </summary>
        [DefaultValue(Browser.Firefox)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Browser Browser { get; set; }

        /// <summary>
        /// Platform to request for a RemoteWebDriver
        /// </summary>
        [DefaultValue(PlatformType.Any)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public PlatformType PlatformType { get; set; }

        /// <summary>
        /// WindowSize to request
        /// </summary>
        [DefaultValue(WindowSize.Hd)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public WindowSize WindowSize { get; set; }

        /// <summary>
        /// Custom window size to request.
        /// </summary>
        public Size WindowCustomSize {
            get => WindowSize == WindowSize.Custom ? windowCustomSize : WindowSize.Size();
            set => windowCustomSize = value;
        }

        /// <summary>
        /// The Uri of the Selenium grid to use for remote calls.
        /// </summary>
        [DefaultValue("https://localhost:4400/wd/grid")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Uri GridUri { get; set; }

        /// <summary>
        /// Use a local WebDriver.
        /// </summary>
        [DefaultValue(true)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool IsLocal { get; set; }

        /// <summary>
        /// Run headless if available.
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public bool Headless { get; set; }
    }
}
