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

using AlexanderOnTest.NetCoreWebDriverFactory.DriverOptionsFactory;
using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Safari;
using System;
using System.Globalization;
using static FluentAssertions.FluentActions;

namespace AlexanderOnTest.NetCoreWebDriverFactory.UnitTests.DriverOptionsFactory
{
    public class StaticDriverOptionsFactoryCultureTests
    {
        internal const string UnsupportedBrowserMessage =
            "The requested browser does not support requesting a given language culture.";

        internal const string HeadlessBrowserNotSupportedMessage =
            "Chromium based browsers do not support headless running when requesting a given language culture.";
        private static CultureInfo CultureInfo => new("en-GB");
        
        [Test]
        public void CallingWithNullCultureThrowsCorrectly()
        {
            Invoking(() => StaticDriverOptionsFactory.SetCulture(new ChromeOptions(), null, true))
                .Should().Throw<ArgumentNullException>();
        }
        
        [Test]
        public void RequestingHeadlessCulturedChromeOptionsCorrectlyThrows()
        {
            Invoking(() =>StaticDriverOptionsFactory.SetCulture(new ChromeOptions(), CultureInfo, true))
                .Should().Throw<NotSupportedException>()
                .WithMessage(HeadlessBrowserNotSupportedMessage);
        }

        [Test]
        public void RequestingOnScreenCulturedChromeOptionsDoesNotThrow()
        {
            Invoking(() => StaticDriverOptionsFactory.SetCulture(new ChromeOptions(), CultureInfo, false))
                .Should().NotThrow<NotSupportedException>();
        }

        [Test]
        public void RequestingHeadlessCulturedEdgeOptionsCorrectlyThrows()
        {
            Invoking(() => StaticDriverOptionsFactory.SetCulture(new EdgeOptions(), CultureInfo, true))
                .Should().Throw<NotSupportedException>()
                .WithMessage(HeadlessBrowserNotSupportedMessage);
        }

        [Test]
        public void RequestingOnScreenCulturedEdgeOptionsDoesNotThrow()
        {
            Invoking(() => StaticDriverOptionsFactory.SetCulture(new EdgeOptions(), CultureInfo, false))
                .Should().NotThrow<NotSupportedException>();
        }

        [Test]
        public void RequestingHeadlessCulturedFirefoxOptionsDoesNotThrow()
        {
            Invoking(() => StaticDriverOptionsFactory.SetCulture(new FirefoxOptions(), CultureInfo, true))
                .Should().NotThrow<NotSupportedException>();
        }

        [Test]
        public void RequestingOnScreenCulturedFirefoxOptionsDoesNotThrow()
        {
            Invoking(() => StaticDriverOptionsFactory.SetCulture(new FirefoxOptions(), CultureInfo, false))
                .Should().NotThrow<NotSupportedException>();
        }

        [Test]
        public void RequestingCulturedInternetExplorerOptionsThrowsCorrectly()
        {
            Invoking(() => StaticDriverOptionsFactory.SetCulture(new InternetExplorerOptions(), CultureInfo, true))
                .Should().Throw<NotSupportedException>()
                .WithMessage(UnsupportedBrowserMessage);
        }

        [Test]
        public void RequestingCulturedSafariOptionsThrowsCorrectly()
        {
            Invoking(() => StaticDriverOptionsFactory.SetCulture(new SafariOptions(), CultureInfo, true))
                .Should().Throw<NotSupportedException>()
                .WithMessage(UnsupportedBrowserMessage);
        }
    }
}
