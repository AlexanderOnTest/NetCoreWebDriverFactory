using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace AlexanderOnTest.NetCoreWebDriverFactory.Utils
{
    /// <summary>
    /// Default implementation of the IWebDriverReSizer interface
    /// </summary>
    public class WebDriverReSizer : IWebDriverReSizer
    {
        /// <summary>
        /// Set the WebDriver to the requested Browser size.
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="windowSize"></param>
        /// <param name="windowCustomSize"></param>
        /// <returns></returns>
        public IWebDriver SetWindowSize(IWebDriver driver, WindowSize windowSize, Size windowCustomSize = new Size())
        {
            switch (windowSize)
            {
                case WindowSize.Unchanged:
                    return driver;

                case WindowSize.Maximise:
                    driver.Manage().Window.Maximize();
                    return driver;

                case WindowSize.Maximize:
                    driver.Manage().Window.Maximize();
                    return driver;

                case WindowSize.Defined:
                    if (!((RemoteWebDriver)driver).Capabilities.GetCapability("browserName").Equals("Safari"))
                    {
                        driver.Manage().Window.Position = Point.Empty;
                    }
                    driver.Manage().Window.Size = windowCustomSize;
                    return driver;

                default:
                    if (!((RemoteWebDriver)driver).Capabilities.GetCapability("browserName").Equals("Safari"))
                    {
                        driver.Manage().Window.Position = Point.Empty;
                    }
                    driver.Manage().Window.Size = windowSize.Size();
                    return driver;
            }
        }
    }
}
