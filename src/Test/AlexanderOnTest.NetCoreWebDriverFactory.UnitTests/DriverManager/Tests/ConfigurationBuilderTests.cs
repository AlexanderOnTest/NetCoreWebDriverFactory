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
using System.Globalization;
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
        [TestCase(Browser.Safari, WindowSize.Maximise, false, false, "http://localhost:4444", PlatformType.Mac, "nl")]
        [TestCase(Browser.Edge, WindowSize.Unchanged, false, false, "http://localhost:4444", PlatformType.Windows, "en-GB")]
        [TestCase(Browser.InternetExplorer, WindowSize.Hd, false, false, "http://localhost:4444", PlatformType.Windows, "es-US")]
        [TestCase(Browser.Firefox, WindowSize.Maximise, false, false, "http://localhost:4444", PlatformType.Linux, "fr")]
        [TestCase(Browser.Chrome, WindowSize.Uhd, true, true, "http://localhost:4444", PlatformType.Any, "en-US")]
        public void BuilderJsonConfigStringProducesCorrectSerialisation(
            Browser browser, 
            WindowSize windowSize, 
            bool headless,
            bool isLocal,
            string gridUri,
            PlatformType platformType,
            string language)
        {
            // Arrange
            var webDriverConfigurationBuilder = WebDriverConfigurationBuilder.Start()
                .WithBrowser(browser)
                .WithHeadless(headless)
                .WithWindowSize(windowSize)
                .WithIsLocal(isLocal)
                .WithGridUri(new Uri(gridUri))
                .WithPlatformType(platformType)
                .WithLanguageCulture(language != null ? new CultureInfo(language) : null);
            WebDriverConfiguration expectedConfig = webDriverConfigurationBuilder.Build();
            string requestedJson = expectedConfig.SerializeToJson();


            string jsonConfigString = webDriverConfigurationBuilder.GetJsonConfigString();

            // Act
            WebDriverConfiguration returnedConfig = WebDriverConfiguration.DeserializeFromJson(requestedJson);
            WebDriverConfiguration configFromJsonConfigString = WebDriverConfiguration.DeserializeFromJson(jsonConfigString);

            // Assert
            configFromJsonConfigString.Should().BeEquivalentTo(returnedConfig);
        }

        [TestCase(Browser.Safari, WindowSize.Maximise, false, false, "http://localhost:4444", PlatformType.Mac, "nl")]
        [TestCase(Browser.Edge, WindowSize.Unchanged, false, false, "http://localhost:4444", PlatformType.Windows, "en-GB")]
        [TestCase(Browser.InternetExplorer, WindowSize.Hd, false, false, "http://localhost:4444", PlatformType.Windows, "es-US")]
        [TestCase(Browser.Firefox, WindowSize.Maximise, false, false, "http://localhost:4444", PlatformType.Linux, "fr")]
        [TestCase(Browser.Chrome, WindowSize.Uhd, true, true, "http://localhost:4444", PlatformType.Any, "en-US")]
        public void JsonNetSerialisationIsEquivalentToBuilderGetJsonConfigString(
            Browser browser,
            WindowSize windowSize,
            bool headless,
            bool isLocal,
            string gridUri,
            PlatformType platformType,
            string language)
        {
            CompareJsonEquivalence(WebDriverConfigurationBuilder.Start()
                .WithBrowser(browser)
                .WithHeadless(headless)
                .WithWindowSize(windowSize)
                .WithIsLocal(isLocal)
                .WithGridUri(new Uri(gridUri))
                .WithPlatformType(platformType)
                .WithLanguageCulture(language != null ? new CultureInfo(language) : null));
        }
        
        
        [TestCase(1280, 1024)]
        [TestCase(1920, 1200)]
        [TestCase(320, 240)]
        [TestCase(240, 320)]
        public void JsonNetSerialisationIsEquivalentToBuilderGetJsonConfigStringCustomSizeConfiguration(int width, int height)
        {
            CompareJsonEquivalence(WebDriverConfigurationBuilder.Start().WithCustomSize(new Size(width, height)));
        }
        
        [TestCase(WindowSize.Unchanged)]
        [TestCase(WindowSize.Maximise)]
        [TestCase(WindowSize.Maximize)]
        public void JsonNetSerialisationIsEquivalentToBuilderGetJsonConfigStringUndefinedSizeConfiguration(WindowSize windowSize)
        {
            CompareJsonEquivalence(WebDriverConfigurationBuilder.Start().WithWindowSize(windowSize));
        }

        [TestCase(WindowSize.Hd)]
        [TestCase(WindowSize.Fhd)]
        [TestCase(WindowSize.Qhd)]
        [TestCase(WindowSize.Uhd)]
        public void JsonNetSerialisationIsEquivalentToBuilderGetJsonConfigStringStandardSizeConfiguration(WindowSize windowSize)
        {
            CompareJsonEquivalence(WebDriverConfigurationBuilder.Start().WithWindowSize(windowSize));
        }

        private void CompareJsonEquivalence(WebDriverConfigurationBuilder webDriverConfigurationBuilder)
        {
            // Arrange
            string formattedString = webDriverConfigurationBuilder.GetJsonConfigString();
            WebDriverConfiguration expectedConfig = webDriverConfigurationBuilder.Build();
            string requestedJson = expectedConfig.SerializeToJson();

            // Act
            WebDriverConfiguration returnedConfig = WebDriverConfiguration.DeserializeFromJson(requestedJson);
            WebDriverConfiguration configFromFormattedString =
                WebDriverConfiguration.DeserializeFromJson(formattedString);
            
            // Assert
            returnedConfig.Should().BeEquivalentTo(configFromFormattedString);
        }
    }
}
