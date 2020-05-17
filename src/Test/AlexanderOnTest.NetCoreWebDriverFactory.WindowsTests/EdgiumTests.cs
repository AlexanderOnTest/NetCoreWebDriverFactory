using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using AlexanderOnTest.NetCoreWebDriverFactory.DriverOptionsFactory;
using AlexanderOnTest.NetCoreWebDriverFactory.Utils;
using AlexanderOnTest.NetCoreWebDriverFactory.WebDriverFactory;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal;
using OpenQA.Selenium;

namespace AlexanderOnTest.NetCoreWebDriverFactory.WindowsTests
{
    public class EdgiumTests
    {
        private const string InstalledDriverPath = null;
        private static readonly WebDriverReSizer webDriverReSizer = new WebDriverReSizer();

        private readonly EdgiumLocalWebDriverFactory LocalEdgiumWebDriverFactory = new EdgiumLocalWebDriverFactory(
            new DefaultDriverOptionsFactory(), 
            InstalledDriverPath, 
            webDriverReSizer );

        private readonly EdgiumLocalWebDriverFactory culturedEdgiumLocalWebDriverFactory = new EdgiumLocalWebDriverFactory(
            new CulturedDriverOptionsFactory(new CultureInfo("nl")),
            InstalledDriverPath,
            webDriverReSizer);
            

        private IWebDriver driver;

        [Test]
        public void EdgiumLoadsWithDefaultDriverOptionsFactory()
        {
            driver = LocalEdgiumWebDriverFactory.GetWebDriver(Browser.Edge);
            driver.Should().NotBeNull();

            driver.Url = "https://developer.microsoft.com/en-us/microsoft-edge/tools/webdriver/";
            driver.Title.Should().BeEquivalentTo("WebDriver - Microsoft Edge Developer");
        }

        [Test]
        public void EdgiumCultureDriverLoads()
        {
            driver = culturedEdgiumLocalWebDriverFactory.GetWebDriver(Browser.Edge);
            driver.Should().NotBeNull();
            driver.Url = "https://manytools.org/http-html-text/browser-language/";
            driver.Title.Should().BeEquivalentTo("Browser language - display the list of languages your browser says you prefer");

            
            var executor = (IJavaScriptExecutor) driver;
            
            string language = executor.ExecuteScript("return window.navigator.userlanguage || window.navigator.language").ToString();
            language.Should().Be("nl");
        }

        [TearDown]
        public void TearDown()
        {
            driver?.Quit();
            driver?.Dispose();
        }
    }
}
