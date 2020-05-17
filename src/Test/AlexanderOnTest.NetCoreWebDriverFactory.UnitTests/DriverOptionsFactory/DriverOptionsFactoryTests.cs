using AlexanderOnTest.NetCoreWebDriverFactory.DriverOptionsFactory;
using FluentAssertions;
using FluentAssertions.Execution;
//using Microsoft.Edge.SeleniumTools;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;

namespace AlexanderOnTest.NetCoreWebDriverFactory.UnitTests.DriverOptionsFactory
{
    public class DriverOptionsFactoryTests
    {
        private DefaultDriverOptionsFactory factory;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            factory = new DefaultDriverOptionsFactory();
        }

        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public void ChromiumEdgeIsConfiguredCorrectly()
        {
            DriverOptions options = factory.GetLocalDriverOptions<Microsoft.Edge.SeleniumTools.EdgeOptions>();
            
            options.Should().BeOfType<Microsoft.Edge.SeleniumTools.EdgeOptions>();
            if (options is Microsoft.Edge.SeleniumTools.EdgeOptions edgeOptions)
            {
                using (new AssertionScope())
                {
                    edgeOptions.Should().NotBeNull();
                    edgeOptions.UseChromium.Should().BeTrue();
                    edgeOptions.BrowserName.Should().Be("MicrosoftEdge");
                }
            }
        }

        [Test]
        public void ClassicEdgeIsConfiguredCorrectly()
        {
            DriverOptions options = factory.GetLocalDriverOptions<EdgeOptions>();
            
            options.Should().BeOfType<EdgeOptions>();
            if (options is EdgeOptions edgeOptions)
            {
                using (new AssertionScope())
                {
                    edgeOptions.Should().NotBeNull();
                    edgeOptions.BrowserName.Should().Be("MicrosoftEdge");
                }
            }
        }
    }
}
