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
        
        /// <summary>
        /// Asserts that the page loads with the correct title and expected Culture if requested
        /// To be a valid test, the requested culture if set must be different to that of the test OS
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="requestedCulture"></param>
        /// <param name="pageToBeLoaded"></param>
        /// <param name="expectedPageTitle"></param>
        public static void AssertThatPageCanBeLoadedInExpectedLanguage(
            IWebDriver driver,
            CultureInfo requestedCulture,
            string pageToBeLoaded = "https://manytools.org/http-html-text/browser-language/",
            string expectedPageTitle = "Browser language - display the list of languages your browser says you prefer")
        {
            driver.Url = pageToBeLoaded;
            var executor = (IJavaScriptExecutor) driver;
            string language = executor.ExecuteScript("return window.navigator.userlanguage || window.navigator.language").ToString();
            using (new AssertionScope())
            {
                driver.Title.Should().Be(expectedPageTitle);
                if (requestedCulture != null)
                {
                    language.Should().Be(requestedCulture.ToString());
                }
            }
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

        public static void AssertThatRequestingAnUnsupportedBrowserThrowsCorrectException(
            Action act, 
            Browser browser,
            PlatformType platformType)
        {
            act.Should()
                .Throw<PlatformNotSupportedException>($"because {browser} is not supported on {platformType}.")
                .WithMessage("*is not currently supported on*");
        }

        public static void AssertThatRequestingAnUnsupportedHeadlessBrowserThrowsCorrectException(
            Action act,
            Browser browser)
        {
            act.Should()
                .ThrowExactly<NotSupportedException>($"because headless mode is not supported on {browser}.")
                .WithMessage($"Headless mode is not currently supported for {browser}.");
        }

        public static void AssertThatRequestingAnUnsupportedCulturedBrowserThrowsCorrectException(
            Action act,
            Browser browser)
        {
            act.Should()
                .ThrowExactly<NotSupportedException>($"{browser} does not support requesting browsers with a specified culture.")
                .WithMessage("The requested browser does not support requesting a given language culture*");
        }

        public static void AssertThatRequestingAHeadlessCulturedChromiumBrowserThrowsCorrectException(
            Action act,
            Browser browser)
        {
            act.Should()
                .ThrowExactly<NotSupportedException>("Chromium based browsers do not support headless running when requesting a given language culture.")
                .WithMessage("Chromium based browsers do not support headless running when requesting a given language culture*");
        }
    }
}
