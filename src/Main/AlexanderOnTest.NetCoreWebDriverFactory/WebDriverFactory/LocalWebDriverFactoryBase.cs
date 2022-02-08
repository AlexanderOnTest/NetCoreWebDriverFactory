// <copyright>
// Copyright 2019 Alexander Dunn
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
using System.Runtime.InteropServices;
using AlexanderOnTest.NetCoreWebDriverFactory.Config;
using AlexanderOnTest.NetCoreWebDriverFactory.DriverOptionsFactory;
using AlexanderOnTest.NetCoreWebDriverFactory.Logging;
using AlexanderOnTest.NetCoreWebDriverFactory.Utils;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Safari;

namespace AlexanderOnTest.NetCoreWebDriverFactory.WebDriverFactory
{
    /// <summary>
    /// Default LocalWebDriverFactory implementation for .NET Core projects.
    /// </summary>
    public abstract class LocalWebDriverFactoryBase : ILocalWebDriverFactory
    {
        private static readonly ILog Logger = LogProvider.For<DefaultWebDriverFactory>();
        private static readonly bool IsDebugEnabled = Logger.IsDebugEnabled();
        private bool _useEdgeForInternetExplorer;
        private string _edgeExecutablePath = "C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\msedge.exe";

        /// <summary>
        /// Return a DriverFactory instance for use in .NET Core projects.
        /// Try using installedDriverPath = "Path.GetDirectoryName(Assembly.GetCallingAssembly().Location)" when running from .NET core projects.
        /// </summary>
        /// <param name="driverOptionsFactory"></param>
        /// <param name="installedDriverPath"></param>
        /// <param name="webDriverReSizer"></param>
        /// <param name="useEdgeForInternetExplorer"></param>
        protected LocalWebDriverFactoryBase(IDriverOptionsFactory driverOptionsFactory,
                                            string installedDriverPath,
                                            IWebDriverReSizer webDriverReSizer, 
                                            bool useEdgeForInternetExplorer = true)
        {
            InstalledDriverPath = installedDriverPath;
            DriverOptionsFactory = driverOptionsFactory ?? new DefaultDriverOptionsFactory();
            WebDriverReSizer = webDriverReSizer;
            _useEdgeForInternetExplorer = useEdgeForInternetExplorer;
        }

        /// <summary>
        /// Return a DriverFactory instance for use in .NET Core projects.
        /// Try using driverPath = new DriverPath(Assembly.GetCallingAssembly()) when testing locally from .NET core projects.
        /// </summary>
        /// <param name="driverOptionsFactory"></param>
        /// <param name="driverPath"></param>
        /// <param name="webDriverReSizer"></param>
        protected LocalWebDriverFactoryBase(IDriverOptionsFactory driverOptionsFactory,
                                            DriverPath driverPath,
                                            IWebDriverReSizer webDriverReSizer)
            : this(driverOptionsFactory, driverPath?.PathString, webDriverReSizer)
        {
        }

        /// <summary>
        /// The path of installed drivers.
        /// </summary>
        protected string InstalledDriverPath { get; }

        /// <summary>
        /// The IWebDriverReSizer implementation to use.
        /// </summary>
        protected IWebDriverReSizer WebDriverReSizer { get; }

        /// <summary>
        /// The DriverOptionsFactory to use.
        /// </summary>
        public IDriverOptionsFactory DriverOptionsFactory { get; set; }

        /// <summary>
        /// Path to the Microsoft Edge Executable (defaults to Windows default location)
        /// </summary>
        public string EdgeExecutablePath { get => _edgeExecutablePath; set => _edgeExecutablePath = value; }

        /// <summary>
        /// Return a local WebDriver of the given browser type with default settings.
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="windowSize"></param>
        /// <param name="headless"></param>
        /// <param name="windowCustomSize"></param>
        /// <returns></returns>
        public virtual IWebDriver GetWebDriver(Browser browser, WindowSize windowSize = WindowSize.Hd, bool headless = false, Size windowCustomSize = new Size())
        {
            if (headless && 
                !(browser == Browser.Chrome || 
                  browser == Browser.Edge || 
                  browser == Browser.Firefox))
            {
                Exception ex = new ArgumentException($"Headless mode is not currently supported for {browser}.");
                Logger.Fatal("Invalid WebDriver Configuration requested.", ex);
                throw ex;
            }

            switch (browser)
            {
                case Browser.Firefox:
                    return GetWebDriver(DriverOptionsFactory.GetLocalDriverOptions<FirefoxOptions>(headless), windowSize, windowCustomSize);

                case Browser.Chrome:
                    return GetWebDriver(DriverOptionsFactory.GetLocalDriverOptions<ChromeOptions>(headless), windowSize, windowCustomSize);

                case Browser.InternetExplorer:
                    return GetWebDriver(DriverOptionsFactory.GetLocalDriverOptions<InternetExplorerOptions>(), windowSize, windowCustomSize);

                case Browser.Edge:
                    return GetWebDriver(DriverOptionsFactory.GetLocalDriverOptions<EdgeOptions>(headless), windowSize, windowCustomSize);

                case Browser.Safari:
                    return GetWebDriver(DriverOptionsFactory.GetLocalDriverOptions<SafariOptions>(), windowSize, windowCustomSize);

                default:
                    Exception ex = new PlatformNotSupportedException($"{browser} is not currently supported.");
                    Logger.Fatal("Invalid WebDriver Configuration requested.", ex);
                    throw ex;
            }
        }

        /// <summary>
        /// Return a local WebDriver instance of the given configuration.
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public IWebDriver GetWebDriver(IWebDriverConfiguration configuration)
        {
            Logger.Info($"Local WebDriver requested using configuration: {configuration}");

            if (!configuration.IsLocal)
            {
                Exception ex = new ArgumentException("A RemoteWebDriver Instance cannot be generated by this method.");
                Logger.Fatal("Invalid WebDriver Configuration requested.", ex);
                throw ex;
            }

            return GetWebDriver(
                configuration.Browser,
                configuration.WindowSize,
                configuration.Headless,
                configuration.WindowDefinedSize
            );
        }

        /// <summary>
        /// Return a Local Chrome WebDriver instance.
        /// Try using driverPath = "Path.GetDirectoryName(Assembly.GetCallingAssembly().Location)"
        /// </summary>
        /// <param name="options"></param>
        /// <param name="windowSize"></param>
        /// <param name="windowCustomSize"></param>
        /// <returns></returns>
        public virtual IWebDriver GetWebDriver(ChromeOptions options, WindowSize windowSize = WindowSize.Hd, Size windowCustomSize = new Size())
        {
            return GetLocalWebDriver(options, InstalledDriverPath, windowSize, windowCustomSize);
        }

        /// <summary>
        /// Return a local Firefox WebDriver instance.
        /// Try using driverPath = "Path.GetDirectoryName(Assembly.GetCallingAssembly().Location)"
        /// </summary>
        /// <param name="options"></param>
        /// <param name="windowSize"></param>
        /// <param name="windowCustomSize"></param>
        /// <returns></returns>
        public virtual IWebDriver GetWebDriver(FirefoxOptions options, WindowSize windowSize = WindowSize.Hd, Size windowCustomSize = new Size())
        {
            return GetLocalWebDriver(options, InstalledDriverPath, windowSize, windowCustomSize);
        }

        /// <summary>
        /// Return a local Edge WebDriver instance. (Only supported on Microsoft Windows 10 version 1809 or later)
        /// </summary>
        /// <param name="options"></param>
        /// <param name="windowSize"></param>
        /// <param name="windowCustomSize"></param>
        /// <returns></returns>
        public virtual IWebDriver GetWebDriver(EdgeOptions options, WindowSize windowSize = WindowSize.Hd, Size windowCustomSize = new Size())
        {
            return GetLocalWebDriver(options, InstalledDriverPath, windowSize, windowCustomSize);
        }

        /// <summary>
        /// Return a local Internet Explorer WebDriver instance. (Only supported on Microsoft Windows)
        /// </summary>
        /// <param name="options"></param>
        /// <param name="windowSize"></param>
        /// <param name="windowCustomSize"></param>
        /// <returns></returns>
        public virtual IWebDriver GetWebDriver(InternetExplorerOptions options, WindowSize windowSize = WindowSize.Hd, Size windowCustomSize = new Size())
        {
            if (_useEdgeForInternetExplorer)
            {
                options.AttachToEdgeChrome = true;
                options.EdgeExecutablePath = EdgeExecutablePath;
            }
            return GetLocalWebDriver(options, InstalledDriverPath, windowSize, windowCustomSize);
        }

        /// <summary>
        /// Return a local Safari WebDriver instance. (Only supported on Mac Os)
        /// </summary>
        /// <param name="options"></param>
        /// <param name="windowSize"></param>
        /// <param name="windowCustomSize"></param>
        /// <returns></returns>
        public virtual IWebDriver GetWebDriver(SafariOptions options, WindowSize windowSize = WindowSize.Hd, Size windowCustomSize = new Size())
        {
            return GetLocalWebDriver(options, null, windowSize, windowCustomSize);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                DriverOptionsFactory?.Dispose();
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Return a Local Chrome WebDriver instance.
        /// Try using driverPath = "Path.GetDirectoryName(Assembly.GetCallingAssembly().Location)" 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="driverPath"></param>
        /// <param name="windowSize"></param>
        /// <param name="windowCustomSize"></param>
        /// <returns></returns>
        protected IWebDriver GetLocalWebDriver(
            ChromeOptions options,
            string driverPath = null,
            WindowSize windowSize = WindowSize.Hd,
            Size windowCustomSize = new Size())
        {
            IWebDriver driver = null;

            driver = driverPath == null
                    ? new ChromeDriver(options)
                    : new ChromeDriver(driverPath, options);

            return WebDriverReSizer.SetWindowSize(driver, windowSize, windowCustomSize);
        }

        /// <summary>
        /// Return a local Firefox WebDriver instance.
        /// Try using driverPath = "Path.GetDirectoryName(Assembly.GetCallingAssembly().Location)" 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="driverPath"></param>
        /// <param name="windowSize"></param>
        /// <param name="windowCustomSize"></param>
        /// <returns></returns>
        protected IWebDriver GetLocalWebDriver(
            FirefoxOptions options,
            string driverPath = null,
            WindowSize windowSize = WindowSize.Hd,
            Size windowCustomSize = new Size())
        {
            IWebDriver driver = null;

            driver = driverPath == null
                ? new FirefoxDriver(options)
                : new FirefoxDriver(driverPath, options);

            return WebDriverReSizer.SetWindowSize(driver, windowSize, windowCustomSize);
        }

        /// <summary>
        /// Return a local Edge WebDriver instance. 
        /// Try using driverPath = "Path.GetDirectoryName(Assembly.GetCallingAssembly().Location)"
        /// </summary>
        /// <param name="options"></param>
        /// <param name="driverPath"></param>
        /// <param name="windowSize"></param>
        /// <param name="windowCustomSize"></param>
        /// <returns></returns>
        protected IWebDriver GetLocalWebDriver(
            EdgeOptions options,
            string driverPath = null,
            WindowSize windowSize = WindowSize.Hd,
            Size windowCustomSize = new Size())
        {
            IWebDriver driver = null;

            driver = driverPath == null
                ? new EdgeDriver(options)
                : new EdgeDriver(driverPath, options);

            return WebDriverReSizer.SetWindowSize(driver, windowSize, windowCustomSize);
        }

        /// <summary>
        /// Return a local Internet Explorer WebDriver instance. (Only supported on Microsoft Windows)
        /// Try using driverPath = "Path.GetDirectoryName(Assembly.GetCallingAssembly().Location)"
        /// </summary>
        /// <param name="options"></param>
        /// <param name="driverPath"></param>
        /// <param name="windowSize"></param>
        /// <param name="windowCustomSize"></param>
        /// <returns></returns>
        protected IWebDriver GetLocalWebDriver(
                InternetExplorerOptions options,
                string driverPath = null,
                WindowSize windowSize = WindowSize.Hd,
                Size windowCustomSize = new Size())
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                throw new PlatformNotSupportedException("Microsoft Internet Explorer is only available on Microsoft Windows.");
            }

            IWebDriver driver = null;

            driver = driverPath == null
                ? new InternetExplorerDriver(options)
                : new InternetExplorerDriver(driverPath, options);

            return WebDriverReSizer.SetWindowSize(driver, windowSize, windowCustomSize);
        }

        /// <summary>
        /// Return a local Safari WebDriver instance. (Only supported on Mac Os)
        /// Try using driverPath = null (default)
        /// </summary>
        /// <param name="options"></param>
        /// <param name="driverPath"></param>
        /// <param name="windowSize"></param>
        /// <param name="windowCustomSize"></param>
        /// <returns></returns>
        protected IWebDriver GetLocalWebDriver(
            SafariOptions options,
            string driverPath = null,
            WindowSize windowSize = WindowSize.Hd,
            Size windowCustomSize = new Size())
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                throw new PlatformNotSupportedException("Safari is only available on Mac Os.");
            }

            IWebDriver driver = null;

            driver = driverPath == null
                ? new SafariDriver(options)
                : new SafariDriver(driverPath, options);

            return WebDriverReSizer.SetWindowSize(driver, windowSize, windowCustomSize);
        }
    }
}
