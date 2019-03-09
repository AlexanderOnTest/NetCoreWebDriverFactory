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

using System.Drawing;
using AlexanderOnTest.NetCoreWebDriverFactory.Utils.Builders;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;

namespace AlexanderOnTest.NetCoreWebDriverFactory.UnitTests.DriverManager.Tests
{
    [Category("CI")]
    public class ConverterTests
    {
        [TestCase(240, 320)]
        public void ConfigurationCorrectlySerialisesAndDeserialises(int width, int height)
        {
            // Arrange
            WebDriverConfiguration testConfig = WebDriverConfigurationBuilder.Start().WithCustomSize(new Size(width, height)).Build();

            // Act
            string serialisedConfig = testConfig.SerializeToJson();
            WebDriverConfiguration deserialisedConfig = WebDriverConfiguration.DeserializeFromJson(serialisedConfig);

            // Assert
            deserialisedConfig.Should().BeEquivalentTo(testConfig);
        }

        [TestCase(1280, 1024)]
        [TestCase(1920, 1200)]
        [TestCase(320, 240)]
        [TestCase(240, 320)]
        public void CustomSizeConfigurationSerialisesWithCorrectWidthAndHeight(int width, int height)
        {
            // Arrange
            WebDriverConfiguration testedConfig = WebDriverConfigurationBuilder.Start().WithCustomSize(new Size(width, height)).Build();

            // Act
            string actualJson = testedConfig.SerializeToJson();

            //Assert
            using (new AssertionScope())
            {
                actualJson.Should().Contain("\"PlatformType\":1");
                actualJson.Should().Contain("\"WindowSize\":5");
                actualJson.Should().Contain($"\"WindowCustomSize\":{{\"width\":{width},\"height\":{height}}}");
                actualJson.Should().Contain("\"GridUri\":\"http://localhost:4444/wd/hub\"");
            }
        }

        [TestCase(1280, 1024)]
        [TestCase(1920, 1200)]
        [TestCase(320, 240)]
        [TestCase(240, 320)]
        public void CustomSizeConfigurationDeSerialisesWithCorrectWidthAndHeight(int width, int height)
        {
            // Arrange
            var webDriverConfigurationBuilder = WebDriverConfigurationBuilder.Start().WithCustomSize(new Size(width, height));
            WebDriverConfiguration expectedConfig = webDriverConfigurationBuilder.Build();
            string requestedJson = expectedConfig.SerializeToJson();

            // Act
            WebDriverConfiguration returnedConfig = WebDriverConfiguration.DeserializeFromJson(requestedJson);

            // Assert
            returnedConfig.Should().BeEquivalentTo(expectedConfig);
        }
    }
}