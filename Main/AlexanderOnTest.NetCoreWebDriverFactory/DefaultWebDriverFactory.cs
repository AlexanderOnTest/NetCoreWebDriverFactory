using System;
using System.IO;
using System.Reflection;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Safari;

namespace AlexanderOnTest.NetCoreWebDriverFactory
{
    public class DefaultWebDriverFactory : IWebDriverFactory
    {
        public DefaultWebDriverFactory(Uri gridUri = null, IDriverOptionsFactory driverOptionsFactory = null, string driverPath = null)
        {
            GridUri = gridUri?? new Uri("http://localhost:4444/wd/hub");
            DriverOptionsFactory = driverOptionsFactory ?? new DefaultDriverOptionsFactory();
            DriverPath = driverPath?? Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        public string DriverPath { get; set; }

        public Uri GridUri { get; set; }

        public IDriverOptionsFactory DriverOptionsFactory { get; set; }

        public virtual IWebDriver GetLocalWebDriver(Browser browser, bool headless = false)
        {
            if (headless && !(browser == Browser.Chrome || browser == Browser.Firefox))
            {
                throw new ArgumentException($"Headless mode is not currently supported for {browser}.");
            }
            switch (browser)
            {
                case Browser.Firefox:
                    return GetLocalWebDriver(StaticDriverOptionsFactory.GetFirefoxOptions(headless));

                case Browser.Chrome:
                    return GetLocalWebDriver(StaticDriverOptionsFactory.GetChromeOptions(headless));

                case Browser.InternetExplorer:
                    return GetLocalWebDriver(StaticDriverOptionsFactory.GetInternetExplorerOptions());

                case Browser.Edge:
                    return GetLocalWebDriver(StaticDriverOptionsFactory.GetEdgeOptions());

                case Browser.Safari:
                    return GetLocalWebDriver(StaticDriverOptionsFactory.GetSafariOptions());

                default:
                    throw new PlatformNotSupportedException($"{browser} is not currently supported.");
            }
        }

        public virtual IWebDriver GetLocalWebDriver(ChromeOptions options, WindowSize windowSize = WindowSize.Hd)
        {
            return StaticWebDriverFactory.GetLocalWebDriver(options, windowSize);
        }

        public virtual IWebDriver GetLocalWebDriver(FirefoxOptions options, WindowSize windowSize = WindowSize.Hd)
        {
            return StaticWebDriverFactory.GetLocalWebDriver(options, windowSize);
        }

        public virtual IWebDriver GetLocalWebDriver(EdgeOptions options, WindowSize windowSize = WindowSize.Hd)
        {
            return StaticWebDriverFactory.GetLocalWebDriver(options, windowSize);
        }

        public virtual IWebDriver GetLocalWebDriver(InternetExplorerOptions options, WindowSize windowSize = WindowSize.Hd)
        {
            return StaticWebDriverFactory.GetLocalWebDriver(options, windowSize);
        }

        public virtual IWebDriver GetLocalWebDriver(SafariOptions options, WindowSize windowSize = WindowSize.Hd)
        {
            return StaticWebDriverFactory.GetLocalWebDriver(options, windowSize);
        }

        public virtual IWebDriver GetRemoteWebDriver(DriverOptions options,
            Uri gridUrl,
            WindowSize windowSize = WindowSize.Hd)
        {
            return StaticWebDriverFactory.GetRemoteWebDriver(options, gridUrl, windowSize);
        }

        public virtual IWebDriver GetRemoteWebDriver(Browser browser,
            Uri gridUrl = null,
            PlatformType platformType = PlatformType.Any)
        {
            Uri actualGridUrl = gridUrl ?? GridUri;
            switch (browser)
            {
                case Browser.Firefox:
                    return GetRemoteWebDriver(DriverOptionsFactory.GetFirefoxOptions(platformType), actualGridUrl);

                case Browser.Chrome:
                    return GetRemoteWebDriver(DriverOptionsFactory.GetChromeOptions(platformType), actualGridUrl);

                case Browser.InternetExplorer:
                    return GetRemoteWebDriver(DriverOptionsFactory.GetInternetExplorerOptions(platformType), actualGridUrl);

                case Browser.Edge:
                    return GetRemoteWebDriver(DriverOptionsFactory.GetEdgeOptions(platformType), actualGridUrl);

                case Browser.Safari:
                    return GetRemoteWebDriver(DriverOptionsFactory.GetSafariOptions(platformType), actualGridUrl);

                default:
                    throw new PlatformNotSupportedException($"{browser} is not currently supported.");
            }
        }

        public virtual IWebDriver SetWindowSize(IWebDriver driver, WindowSize windowSize)
        {
            return StaticWebDriverFactory.SetWindowSize(driver, windowSize);
        }
    }
}
