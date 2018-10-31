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
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;

namespace AlexanderOnTest.NetCoreWebDriverFactory.WindowsTests
{
    [TestFixture]
    public class StaticWebDriverFactoryTests
    {
        private IWebDriver Driver { get; set; }
        private readonly OSPlatform thisPlatform = OSPlatform.Windows;
        private string DriverPath => Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
        private readonly Uri gridUrl = new Uri("http://192.168.0.200:4444/wd/hub");

        [OneTimeSetUp]
        public void CheckForValidPlatform()
        {
            Assume.That(() => RuntimeInformation.IsOSPlatform(thisPlatform)) ;
        }

        [Test]
        [TestCase(Browser.Firefox)]
        [TestCase(Browser.InternetExplorer)]
        [TestCase(Browser.Edge)]
        [TestCase(Browser.Chrome)]
        public void LocalWebDriverWorks(Browser browser)
        {
            Driver = StaticWebDriverFactory.GetLocalWebDriver(browser, browser == Browser.Edge? null : DriverPath);
            Driver.Url = "https://example.com/";
            Driver.Title.Should().Be("Example Domain");
        }

        [Test]
        [TestCase(Browser.Safari)]
        public void RequestingUnsupportedWebDriverThrowsInformativeException(Browser browser)
        {
            Action act = () => StaticWebDriverFactory.GetLocalWebDriver(browser);
            act.Should()
                .Throw<PlatformNotSupportedException>($"because {browser} is not supported on {thisPlatform}.")
                .WithMessage("*is only available on*");
        }

        [Test]
        [TestCase(Browser.Firefox)]
        [TestCase(Browser.Chrome)]
        public void HeadlessBrowsersWork(Browser browser)
        {
            Driver = StaticWebDriverFactory.GetLocalWebDriver(browser, DriverPath, true);
            Driver.Url = "https://example.com/";
            Driver.Title.Should().Be("Example Domain");
        }

        [Test]
        [TestCase(Browser.Edge)]
        [TestCase(Browser.InternetExplorer)]
        [TestCase(Browser.Safari)]
        public void RequestingUnsupportedHeadlessBrowserThrowsInformativeException(Browser browser)
        {
            Action act = () => StaticWebDriverFactory.GetLocalWebDriver(browser, DriverPath, true);
            act.Should()
                .ThrowExactly<ArgumentException>($"because headless mode is not supported on {browser}.")
                .WithMessage($"Headless mode is not currently supported for {browser}.");
        }

        [Test]
        public void HdBrowserIsOfRequestedSize()
        {
            Driver = StaticWebDriverFactory.GetLocalWebDriver(StaticDriverOptionsFactory.GetFirefoxOptions(true), DriverPath, WindowSize.Hd);

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
            Driver = StaticWebDriverFactory.GetLocalWebDriver(StaticDriverOptionsFactory.GetFirefoxOptions(true), DriverPath, WindowSize.Fhd);

            Assert.Multiple(() =>
            {
                Size size = Driver.Manage().Window.Size;
                size.Height.Should().Be(1080);
                size.Width.Should().Be(1920);
            });
        }

        [Test]
        [TestCase(Browser.Firefox)]
        [TestCase(Browser.InternetExplorer)]
        [TestCase(Browser.Edge)]
        [TestCase(Browser.Chrome)]
        public void RemoteWebDriverOnWindowsWorks(Browser browser)
        {
            Driver = StaticWebDriverFactory.GetRemoteWebDriver(browser, gridUrl, PlatformType.Windows);
            Driver.Url = "https://example.com/";
            Driver.Title.Should().Be("Example Domain");
        }

        [Test]
        [TestCase(Browser.Firefox)]
        [TestCase(Browser.Chrome)]
        [TestCase(Browser.Safari)]
        public void RemoteWebDriverOnMacOsWorks(Browser browser)
        {
            Driver = StaticWebDriverFactory.GetRemoteWebDriver(browser, gridUrl, PlatformType.Mac);
            Driver.Url = "https://example.com/";
            Driver.Title.Should().Be("Example Domain");
        }

        [Test]
        [TestCase(Browser.Firefox)]
        [TestCase(Browser.Chrome)]
        public void RemoteWebDriverOnLinuxWorks(Browser browser)
        {
            Driver = StaticWebDriverFactory.GetRemoteWebDriver(browser, gridUrl, PlatformType.Linux);
            StaticWebDriverFactory.SetWindowSize(Driver, WindowSize.Maximise);
            Driver.Url = "https://example.com/";
            Driver.Title.Should().Be("Example Domain");
        }


        [TearDown]
        public void Teardown()
        {
            Driver?.Quit();
        }
    }
}
