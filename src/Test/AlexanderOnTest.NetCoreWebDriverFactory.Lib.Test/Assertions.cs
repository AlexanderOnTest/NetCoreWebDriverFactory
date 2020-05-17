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
using System.Globalization;
using System.Runtime.InteropServices;
using FluentAssertions;
using FluentAssertions.Execution;
using OpenQA.Selenium;

namespace AlexanderOnTest.NetCoreWebDriverFactory.Lib.Test
{
    public static class Assertions
    {
        public static void AssertThatPageCanBeLoaded(
            IWebDriver driver,
            string pageToBeLoaded = "https://example.com/",
            string expectedPageTitle = "Example Domain")
        {
            driver.Url = pageToBeLoaded;
            driver.Title.Should().Be(expectedPageTitle);
        }

        public static void AssertThatBrowserReturnsTheExpectedCulture(
            IWebDriver driver,
            CultureInfo cultureInfo)
        {
            driver.Url = "https://manytools.org/http-html-text/browser-language/";
            driver.Title.Should().Be("Browser language - display the list of languages your browser says you prefer");
        }

        public static void AssertThatBrowserWindowSizeIsCorrect(
            IWebDriver driver,
            int expectedWidth,
            int expectedHeight
        )
        {
            using (new AssertionScope())
            {
                Size size = driver.Manage().Window.Size;
                size.Width.Should().Be(expectedWidth);
                size.Height.Should().Be(expectedHeight);
            }
        }

        public static void AssertThatRequestingAnUnsupportedBrowserThrowsCorrectException(
            Action act, 
            Browser browser,
            OSPlatform thisPlatform)
        {
            act.Should()
                .Throw<PlatformNotSupportedException>($"because {browser} is not supported on {thisPlatform}.")
                .WithMessage("*is only available on*");
        }

        public static void AssertThatRequestingAnUnsupportedHeadlessBrowserThrowsCorrectException(
            Action act,
            Browser browser)
        {
            act.Should()
                .ThrowExactly<ArgumentException>($"because headless mode is not supported on {browser}.")
                .WithMessage($"Headless mode is not currently supported for {browser}.");
        }
    }
}
