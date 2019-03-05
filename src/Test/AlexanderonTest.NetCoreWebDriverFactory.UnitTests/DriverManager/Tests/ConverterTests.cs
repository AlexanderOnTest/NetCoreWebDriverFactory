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
                actualJson.Should().Contain("\"WindowSize\":6");
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

        public void BuilderGetJsonConfigStringProducesCorrectJson()
        {
            WebDriverConfigurationBuilder wdc= WebDriverConfigurationBuilder.Start()
                .WithBrowser(Browser.Firefox)
                .WithIsLocal(true);

            string generatedJson = wdc.GetJsonConfigString();
            WebDriverConfiguration expectedConfiguration = wdc.Build();
            WebDriverConfiguration configurationFromGeneratedJson = WebDriverConfiguration.DeserializeFromJson(generatedJson);

            configurationFromGeneratedJson.Should().BeEquivalentTo(expectedConfiguration);
        }
    }
}