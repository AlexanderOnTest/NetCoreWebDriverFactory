# NetCoreWebDriverFactory
A Utility library to ease the launching of Selenium WebDriver instances in .net Core projects

10/10/2018
Version 1 release

For usage ideas I recommend you to [look through the test source](https://github.com/AlexanderOnTesting/NetCoreWebDriverFactory/tree/master/Test)

Platforms supported:  
+Windows
+Linux
+MacOs

Browsers supported (Enum values):  
+Browser.Chrome  
+Browser.Edge  
+Browser.Firefox  
+Browser.InternetExplorer  
+Browser.Safari  

Note:  
Edge and Safari should have a null for the driverPath, its default value;  
Chrome, Firefox and Internet Explorer 11 should have the driverPath set to:
```
Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location
```


##The simplest local driver calls 
for a 1366 x 768 WebDriver instance:
```
using System.IO;
using System.Reflection;
using AlexanderOnTest.NetCoreWebDriverFactory;
using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;

namespace WebDriverLaunch
{
    [TestFixture]
    public class WebDriverLaunchTests
    {
        [Test]
        [TestCase(Browser.Edge)]
        [TestCase(Browser.Safari)]
        public void TestBrowserWithNoDriverPath(Browser browser)
        {
            IWebDriver driver = StaticWebDriverFactory.GetLocalWebDriver(browser);
            driver.Url = "http://www.example.com";
            driver.Title.Should().NotBeNullOrEmpty();
        }

        [Test]
        [TestCase(Browser.Chrome)]
        [TestCase(Browser.Firefox)]
        [TestCase(Browser.InternetExplorer)]
        public void TestBrowserWithSuggestedDriverPath(Browser browser)
        {
            IWebDriver driver = StaticWebDriverFactory.GetLocalWebDriver(
                browser,
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            driver.Url = "http://www.example.com";
            driver.Title.Should().NotBeNullOrEmpty();
            driver.Quit();
        }
    }
}
```

###Headless options for a local driver:  
Chrome and Firefox both support headless (i.e. off screen) testing mode. In both cases passing true as a third parameter will request a headless browser. This will throw an ArgumentException if set to true for any of the browsers that do not support this mode.

examples:
```
            IWebDriver driver = StaticWebDriverFactory.GetLocalWebDriver(
                Browser.Firefox,
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), 
                true);
```

### Passing in a DriverOptions instance instead of the Browser enum.
If you have unusual options requirements, or just prefer to create the options yourself:
```
        public static IWebDriver GetLocalWebDriver(
            ChromeOptions options,
            string driverPath = null,
            WindowSize windowSize = WindowSize.Hd)
```
The above signature is available for all supported browsers.