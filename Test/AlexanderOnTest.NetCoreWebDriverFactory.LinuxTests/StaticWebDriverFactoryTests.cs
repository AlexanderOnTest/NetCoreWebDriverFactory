using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace AlexanderOnTest.NetCoreWebDriverFactory.LinuxTests
{
    [TestFixture]
    public class StaticWebDriverFactoryTests
    {
        private IWebDriver Driver { get; set; }
        private readonly PlatformType thisPlatformType = PlatformType.Linux;

        [OneTimeSetUp]
        public void CheckForValidPlatform()
        {
            Assume.That(() => Platform.CurrentPlatform.IsPlatformType(thisPlatformType));
        }

        [Test]
        [TestCase(Browser.Chrome)]
        [TestCase(Browser.Firefox)]
        public void LocalWebDriverCanBeLaunchedAndLoadExampleDotCom(Browser browser)
        {
            Driver = StaticWebDriverFactory.GetLocalWebDriver(browser);
            Driver.Url = "https://example.com/";
            Driver.Title.Should().Be("Example Domain");
        }

        [Test]
        [TestCase(Browser.Edge)]
        [TestCase(Browser.InternetExplorer)]
        [TestCase(Browser.Safari)]
        public void RequestingUnsupportedWebDriverThrowsInformativeException(Browser browser)
        {
            Action act = () => StaticWebDriverFactory.GetLocalWebDriver(browser);
            act.Should()
                .Throw<PlatformNotSupportedException>($"because {browser} is not supported on {thisPlatformType}.")
                .WithMessage("*is only available on*");
        }

        [Test]
        [TestCase(Browser.Firefox)]
        [TestCase(Browser.Chrome)]
        public void HeadlessBrowserCanBeLaunched(Browser browser)
        {
            Driver = StaticWebDriverFactory.GetLocalWebDriver(browser, true);
            Driver.Url = "https://example.com/";
            Driver.Title.Should().Be("Example Domain");
        }

        [Test]
        [TestCase(Browser.Edge)]
        [TestCase(Browser.InternetExplorer)]
        [TestCase(Browser.Safari)]
        public void RequestingUnsupportedHeadlessBrowserThrowsInformativeException(Browser browser)
        {
            Action act = () => StaticWebDriverFactory.GetLocalWebDriver(browser, true);
            act.Should()
                .ThrowExactly<ArgumentException>($"because headless mode is not supported on {browser}.")
                .WithMessage($"Headless mode is not currently supported for {browser}.");
        }

        [Test]
        [TestCase(Browser.Firefox)]
        [TestCase(Browser.InternetExplorer)]
        [TestCase(Browser.Edge)]
        [TestCase(Browser.Chrome)]
        public void RemoteWebDriverCanBeLaunchedAndLoadExampleDotCom(Browser browser)
        {
            Driver = StaticWebDriverFactory.GetRemoteWebDriver(browser, new Uri("http://localhost:4444/wd/hub"), PlatformType.Any);
            Driver.Url = "https://example.com/";
            Driver.Title.Should().Be("Example Domain");
        }

        [Test]
        public void HdBrowserIsOfRequestedSize()
        {
            Driver = StaticWebDriverFactory.GetLocalWebDriver(StaticDriverOptionsFactory.GetFirefoxOptions(true), WindowSize.Hd);

            Assert.Multiple(() =>
            {
                Size size = Driver.Manage().Window.Size;
                size.Width.Should().Be(1366);
                size.Height.Should().Be(768);
            });
        }

        [Test]
        public void FhdBrowserIsOfRequestedSize()
        {
            Driver = StaticWebDriverFactory.GetLocalWebDriver(StaticDriverOptionsFactory.GetFirefoxOptions(true), WindowSize.Fhd);

            Assert.Multiple(() =>
            {
                Size size = Driver.Manage().Window.Size;
                size.Height.Should().Be(1080);
                size.Width.Should().Be(1920);
            });
        }

        [TearDown]
        public void Teardown()
        {
            Driver?.Quit();
        }
    }
}
