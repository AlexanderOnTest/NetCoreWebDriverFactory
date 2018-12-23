# NetCoreWebDriverFactory
A Utility library to ease the launching of Selenium WebDriver instances in .NET Core projects.

## 19/12/2018 - Version 2.0.0 release
### The following documentation is for version 2.x. 
Version 1.x documentation is available [here.](https://github.com/AlexanderOnTesting/NetCoreWebDriverFactory/wiki/Documentation-archive)

## Why an IWebDriverFactory for .NET core?
The standard WebDriver nuget packages install the binaries on the path when instantiating IWebDriver instances from .NET Framework projects allowing simple calls of the form
```
IWebDriver driver = new FirefoxDriver();
```

When attempting the same from a .NET Core project however, the call fails to locate the WebDriver binary for most browsers. (All except the most recent versions of Safari and Microsoft Edge)
The required call becomes
```
IWebDriver driver = new FirefoxDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

```
but only for browsers that require a separate WebDriver binary. 

This library provides a consistent interface for cross browser testing.

This is not really intended for production use, but is a quick starter for your .NET Core experiments. My hope is that it will ease the creation of WebDriver instances, particularly by providing helpful Exception messages to find the required driver path. 

The recommendation is to instantiate an appropriate IWebDriverFactory for generation of IWebDriver instances. The provided example implementations generate sessions with useful default DriverOptions set. To control the DriverOptions used you can  pass them in directly instead of the Browser enum or create your own IDriverOptionsFactory implementation. All public methods in the default implementations of both interfaces are overridable for simple changes, or the code here can be used as a base to write your own.

There is little benefit to using the static factories directly and I advise against it.

For usage ideas I recommend you to look through the test sources [e.g. for Windows](https://github.com/AlexanderOnTesting/NetCoreWebDriverFactory/blob/master/src/Test/AlexanderOnTest.NetCoreWebDriverFactory.WindowsTests/WebDriverFactoryInstanceTests.cs)

## Basic usage (Local testing only)
A .NET Core project referencing the following nuget packages:
Selenium.WebDriver.NetCoreWebDriverFactory
The appropriate WebDriver binary nuget packages for your OS
e.g. Selenium.WebDriver.GeckoDriver.Win64 by J Sakamoto

#### Instantiation:
IWebDriverFactory webDriverFactory = new NetCoreWebDriverFactory(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

#### Usage:
IWebDriver driver = webDriverFactory.GetLocalWebDriver(Browser.xxx);


## Basic usage (Using a Local Selenium grid)
A .NET Core project referencing the following nuget packages:
Selenium.WebDriver.NetCoreWebDriverFactory

#### Instantiation:
IWebDriverFactory webDriverFactory = new NetCoreWebDriverFactory(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), {GridUrl});

#### Usage:
IWebDriver driver = webDriverFactory.GetRemoteWebDriver(Browser.xxx, PlatformType.xxx);



## Platforms supported (Enum values):  
+ PlatformType.Windows
+ PlatformType.Linux
+ PlatformType.Mac

## Browsers supported (Enum values):  
+ Browser.Chrome  
+ Browser.Edge  
+ Browser.Firefox  
+ Browser.InternetExplorer  
+ Browser.Safari  

## Browser Window size options
+ Hd            (1366 x 768 - default)
+ Fhd           (1920 x 1080)
+ Maximise
+ Unchanged

## Local Visibility options
Chrome and Firefox both support headless browser instances. These can be requested from a local WebDriver using an optional bool. 
Requesting this for other Browsers will throw an Exception.


## Sample RemoteWebDriver Test class - Firefox, Linux 1920 x 1080
```
using System;
using AlexanderOnTest.NetCoreWebDriverFactory;
using NUnit.Framework;
using OpenQA.Selenium;

namespace QuickTests
{
    public class RemoteTests
    {
        private IWebDriverFactory DriverFactory { get ; set; }
        private IWebDriver Driver { get; set; }

        [OneTimeSetUp]
        public void Prepare()
        {
            DriverFactory = new DefaultWebDriverFactory(null, new Uri("http://192.168.0.200:4444/wd/hub"));
        }


        [SetUp]
        public void Setup()
        {
            Driver = DriverFactory.GetRemoteWebDriver(Browser.Firefox, PlatformType.Linux, WindowSize.Fhd);
        }

        [Test]
        public void Test1()
        {
            Driver.Url = "https://www.bbc.co.uk";
            Assert.IsTrue(true);
        }

        // Add more tests here

        [TearDown]
        public void Teardown()
        {
            Driver.Quit();
        }
    }
}
```

## Sample LocalWebDriver Test class - Chrome, Maximised browser window  / 1366 x 768 Headless
```
using System.IO;
using System.Reflection;
using AlexanderOnTest.NetCoreWebDriverFactory;
using NUnit.Framework;
using OpenQA.Selenium;

namespace QuickTests
{
    public class LocalTests
    {
        private IWebDriverFactory DriverFactory { get ; set; }
        private IWebDriver Driver { get; set; }

        [OneTimeSetUp]
        public void Prepare()
        {
            DriverFactory = new DefaultWebDriverFactory(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
        }

        [Test]
        public void OnScreenTest()
        {
            Driver = DriverFactory.GetLocalWebDriver(Browser.Chrome, WindowSize.Maximise);
            Driver.Url = "https://www.bbc.co.uk";
            Assert.IsTrue(true);
        }

        [Test]
        public void HeadlessTest()
        {
            Driver = DriverFactory.GetLocalWebDriver(Browser.Chrome, WindowSize.Hd, true);
            Driver.Url = "https://www.bbc.co.uk";
            Assert.IsTrue(true);
        }

        // Add more tests here

        [TearDown]
        public void Teardown()
        {
            Driver.Quit();
        }
    }
}
```

