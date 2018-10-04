using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;

namespace AlexanderOnTest.NetCoreWebDriverFactory
{
    public interface IWebDriverFactory
    {
        string DriverPath
        {
            get;
            set;
        }

        Uri GridUri
        {
            get;
            set;
        }

        /// <summary>
        /// Return a local webdriver of the given browser type with default settings.
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="headless"></param>
        /// <returns></returns>
        IWebDriver GetLocalWebDriver(Browser browser, bool headless = false);


        /// <summary>
        /// Return a Local Chrome WebDriver instance.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        IWebDriver GetLocalWebDriver(ChromeOptions options, WindowSize windowSize = WindowSize.Hd);

        /// <summary>
        /// Return a local Firefox WebDriver instance.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        IWebDriver GetLocalWebDriver(FirefoxOptions options, WindowSize windowSize = WindowSize.Hd);

        /// <summary>
        /// Return a local Edge WebDriver instance. (Only supported on Microsoft Windows 10)
        /// </summary>
        /// <param name="options"></param>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        IWebDriver GetLocalWebDriver(EdgeOptions options, WindowSize windowSize = WindowSize.Hd);

        /// <summary>
        /// Return a local Internet Explorer WebDriver instance. (Only supported on Microsoft Windows)
        /// </summary>
        /// <param name="options"></param>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        IWebDriver GetLocalWebDriver(InternetExplorerOptions options, WindowSize windowSize = WindowSize.Hd);

        /// <summary>
        /// Return a local Safari WebDriver instance. (Only supported on Mac Os)
        /// </summary>
        /// <param name="options"></param>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        IWebDriver GetLocalWebDriver(SafariOptions options, WindowSize windowSize = WindowSize.Hd);

        /// <summary>
        /// Return a RemoteWebDriver of the given browser type with default settings.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="gridUrl"></param>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        IWebDriver GetRemoteWebDriver(DriverOptions options,
            Uri gridUrl = null,
            WindowSize windowSize = WindowSize.Hd);

        /// <summary>
        /// Return a configured RemoteWebDriver of the given browser type with default settings.
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="gridUrl"></param>
        /// <param name="platformType"></param>
        /// <returns></returns>
        IWebDriver GetRemoteWebDriver(Browser browser,
            Uri gridUrl = null,
            PlatformType platformType = PlatformType.Any);

        /// <summary>
        /// Convenience method for setting the Window Size of a WebDriver to common values. (768P, 1080P and fullscreen)
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        IWebDriver SetWindowSize(IWebDriver driver, WindowSize windowSize);
    }
}

