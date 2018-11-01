using System;
using System.Drawing;
using System.Runtime.InteropServices;
using AlexanderOnTest.NetCoreWebDriverFactory;
using FluentAssertions;
using FluentAssertions.Execution;
using OpenQA.Selenium;

namespace AlexanderonTest.NetCoreWebDriverFactory.Lib.Test
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
