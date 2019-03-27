using System;
using System.Runtime.InteropServices;
using AlexanderOnTest.NetCoreWebDriverFactory.Lib.Test;
using NUnit.Framework;
using OpenQA.Selenium;

namespace AlexanderOnTest.NetCoreWebDriverFactory.MacOsTests
{
    [TestFixture]
    public class RemoteWebDriverFactoryTests : RemoteWebDriverFactoryTestsBase
    {
        private static readonly OSPlatform ThisPlatform = OSPlatform.OSX;
        private static readonly Uri GridUrl = new Uri("http://192.168.0.200:4444/wd/hub");

        public RemoteWebDriverFactoryTests() : base(ThisPlatform, GridUrl) { }

        [Test]
        [TestCase(PlatformType.Linux, Browser.Chrome)]
        [TestCase(PlatformType.Linux, Browser.Firefox)]
        [TestCase(PlatformType.Mac, Browser.Chrome)]
        [TestCase(PlatformType.Mac, Browser.Firefox)]
        [TestCase(PlatformType.Mac, Browser.Safari)]
        [TestCase(PlatformType.Windows, Browser.Chrome)]
        [TestCase(PlatformType.Windows, Browser.Edge)]
        [TestCase(PlatformType.Windows, Browser.Firefox)]
        [TestCase(PlatformType.Windows, Browser.InternetExplorer)]
        public new void RemoteWebDriverFactoryWorks(PlatformType platformType, Browser browser)
        {
            base.RemoteWebDriverFactoryWorks(platformType, browser);
        }
    }
}