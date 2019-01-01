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
using System.IO;
using System.Reflection;
using AlexanderOnTest.NetCoreWebDriverFactory;
using AlexanderOnTest.NetCoreWebDriverFactory.DriverOptionsFactory;
using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;

namespace AlexanderonTest.NetCoreWebDriverFactory.UnitTests.DriverManager.Tests
{
    public class WebDriverManagerTests
    {
        private IWebDriver DriverOne { get; set; }
        private IWebDriver DriverTwo { get; set; }
        private IWebDriverFactory WebDriverFactory { get; set; }
        private IDriverOptionsFactory DriverOptionsFactory { get; set; }
        private IWebDriverManager WebDriverManager { get; set; }

        [OneTimeSetUp]
        public void Prepare()
        {
            DriverOptionsFactory = new DefaultDriverOptionsFactory();
            WebDriverFactory = new DefaultWebDriverFactory(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));   // new FakeWebDriverFactory();
            WebDriverFactory = new FakeWebDriverFactory();
            WebDriverManager = new WebDriverManager(WebDriverFactory, Browser.Firefox);
        }

        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// Get() returns a new driver.
        /// </summary>
        [Test]
        public void GetCreatesNewDriver()
        {
            DriverOne = WebDriverManager.Get();
            DriverOne.Should().NotBeNull();
        }

        /// <summary>
        /// A second call to Get() returns the same instance.
        /// </summary>
        [Test]
        public void GetReturnsTheSameDriver()
        {
            DriverOne = WebDriverManager.Get();
            DriverTwo = WebDriverManager.Get();
            DriverTwo.Should().BeSameAs(DriverOne);
        }

        /// <summary>
        /// A call to GetAdditionalWebDriver() returns a different instance.
        /// </summary>
        [Test]
        public void GetAdditionalWebDriverReturnsAnotherInstance()
        {
            DriverOne = WebDriverManager.Get();
            DriverTwo = WebDriverManager.GetAdditionalWebDriver();
            DriverTwo.Should().NotBeSameAs(DriverOne);
        }

        /// <summary>
        /// Calling GetAdditionalWebDriver() first also returns different instances
        /// </summary>
        [Test]
        public void GetAdditionalWebDriverDoesNotCreateSingleton()
        {
            // Reverse of the above, check that get additional first does not create a singleton instance
            DriverTwo = WebDriverManager.GetAdditionalWebDriver();
            DriverOne = WebDriverManager.Get();
            DriverTwo.Should().NotBeSameAs(DriverOne);
        }

        /// <summary>
        /// Calling Quit() has quit the WebDriver instance
        /// </summary>
        [Test]
        public void QuitHasClosedTheWebDriver()
        {
            DriverOne = WebDriverManager.Get();
            WebDriverManager.Quit();
            Action act = () => DriverOne.Manage().Window.FullScreen();
            act.Should().ThrowExactly<WebDriverException>();
        }

        /// <summary>
        /// Calling Get() after Quit() returns a new instance
        /// </summary>
        [Test]
        public void AfterQuittingANewDriverIsCreated()
        {
            DriverOne = WebDriverManager.Get();
            WebDriverManager.Quit();
            DriverTwo = WebDriverManager.Get();
            DriverTwo.Should().NotBeSameAs(DriverOne);
        }

        /// <summary>
        /// Calling Quit() quits only the singleton driver.
        /// </summary>
        [Test]
        public void QuitOnlyQuitsTheSingletionWebDriver()
        {
            DriverOne = WebDriverManager.Get();
            DriverTwo = WebDriverManager.GetAdditionalWebDriver();
            
            WebDriverManager.Quit();

            Action actOne = () => DriverOne.Manage().Window.FullScreen();
            actOne.Should().ThrowExactly<WebDriverException>();

            Action actTwo = () => DriverTwo.Manage().Window.FullScreen();
            actTwo.Should().NotThrow<WebDriverException>();
        }

        [TearDown]
        public void Teardown()
        {
            DriverOne = WebDriverManager.Quit();
            DriverTwo?.Quit();
        }
    }
}
