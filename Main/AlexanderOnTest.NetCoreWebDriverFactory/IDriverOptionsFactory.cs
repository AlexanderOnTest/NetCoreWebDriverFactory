using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Safari;

namespace AlexanderOnTest.NetCoreWebDriverFactory
{
    public interface IDriverOptionsFactory
    {
        /// <summary>
        /// Return a configured ChromeOptions instance.
        /// </summary>
        /// <param name="platformType"></param>
        /// <returns></returns>
        ChromeOptions GetChromeOptions(PlatformType platformType = PlatformType.Any);

        /// <summary>
        /// Return a configured ChromeOptions instance.
        /// </summary>
        /// <param name="headless"></param>
        /// <param name="platformType"></param>
        /// <returns></returns>
        ChromeOptions GetChromeOptions(bool headless = false, PlatformType platformType = PlatformType.Any);

        /// <summary>
        /// Return a configured FirefoxOptions instance.
        /// </summary>
        /// <param name="platformType"></param>
        /// <returns></returns>
        FirefoxOptions GetFirefoxOptions(PlatformType platformType = PlatformType.Any);

        /// <summary>
        /// Return a configured FirefoxOptions instance.
        /// </summary>
        /// <param name="headless"></param>
        /// <param name="platformType"></param>
        /// <returns></returns>
        FirefoxOptions GetFirefoxOptions(bool headless = false, PlatformType platformType = PlatformType.Any);

        /// <summary>
        /// Return a configured EdgeOptions instance.
        /// </summary>
        /// <param name="platformType"></param>
        /// <returns></returns>
        EdgeOptions GetEdgeOptions(PlatformType platformType = PlatformType.Any);

        /// <summary>
        /// Return a configured InternetExplorerOptions instance.
        /// </summary>
        /// <param name="platformType"></param>
        /// <returns></returns>
        InternetExplorerOptions GetInternetExplorerOptions(PlatformType platformType = PlatformType.Any);

        /// <summary>
        /// Return a configured SafariOptions instance.
        /// </summary>
        /// <param name="platformType"></param>
        /// <returns></returns>
        SafariOptions GetSafariOptions(PlatformType platformType = PlatformType.Any);

        /// <summary>
        /// Add the platform configuration to a DriverOptions instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="options"></param>
        /// <param name="platformType"></param>
        /// <returns></returns>
        T SetPlatform<T>(T options, PlatformType platformType) where T : DriverOptions;

    }
}
