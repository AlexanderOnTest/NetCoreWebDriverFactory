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
using System.Drawing;
using AlexanderOnTest.NetCoreWebDriverFactory.Config;
using AlexanderOnTest.NetCoreWebDriverFactory.Utils.Builders;
using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;

namespace AlexanderOnTest.NetCoreWebDriverFactory.UnitTests.DriverManager.Tests
{
    [Category("CI")]
    public class ConfigurationBuilderTests
    {
        [TestCase(Browser.Safari, WindowSize.Maximise, 1280, 1024, false, false, "http://localhost:4444/wd/hub", PlatformType.Mac)]
        [TestCase(Browser.Edge, WindowSize.Unchanged, 1280, 1024, false, false, "http://localhost:4444/wd/hub", PlatformType.Windows)]
        [TestCase(Browser.InternetExplorer, WindowSize.Hd, 1280, 1024, false, false, "http://localhost:4444/wd/hub", PlatformType.Windows)]
        [TestCase(Browser.Firefox, WindowSize.Maximise, 1280, 1024, false, false, "http://localhost:4444/wd/hub", PlatformType.Linux)]
        [TestCase(Browser.Chrome, WindowSize.Uhd, 1280, 1024, true, true, "http://localhost:4444/wd/hub", PlatformType.Any)]
        public void BuilderJsonConfigStringProducesCorrectSerialisation(
            Browser browser, 
            WindowSize windowSize, 
            int width, 
            int height, 
            bool headless,
            bool isLocal,
            string gridUri,
            PlatformType platformType)
        {
            // Arrange
            var webDriverConfigurationBuilder = WebDriverConfigurationBuilder.Start()
                .WithBrowser(browser)
                .WithHeadless(headless)
                .WithWindowSize(windowSize)
                .WithWindowDefinedSize(new Size(width, height))
                .WithIsLocal(isLocal)
                .WithGridUri(new Uri(gridUri))
                .WithPlatformType(platformType);
            WebDriverConfiguration expectedConfig = webDriverConfigurationBuilder.Build();
            string requestedJson = expectedConfig.SerializeToJson();


            string jsonConfigString = webDriverConfigurationBuilder.GetJsonConfigString();

            // Act
            WebDriverConfiguration returnedConfig = WebDriverConfiguration.DeserializeFromJson(requestedJson);
            WebDriverConfiguration configFromJsonConfigString = WebDriverConfiguration.DeserializeFromJson(jsonConfigString);

            // Assert
            configFromJsonConfigString.Should().BeEquivalentTo(returnedConfig);
        }

    }
}
