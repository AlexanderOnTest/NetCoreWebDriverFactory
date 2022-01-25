# NetCoreWebDriverFactory
A Utility library to ease the launching of Selenium WebDriver instances in .NET Core projects.

### Documentation for each major version is available on the wiki
https://github.com/AlexanderOnTesting/NetCoreWebDriverFactory/wiki


## Why an IWebDriverFactory for .NET core?
The standard WebDriver nuget packages install the binaries on the path when instantiating IWebDriver instances from .NET Framework projects allowing simple calls of the form
```
IWebDriver driver = new FirefoxDriver();
```

When attempting the same from a .NET Core project however, the call fails to locate the WebDriver binary for most browsers. (All except recent versions of Safari on MacOS)
The required call becomes
```
IWebDriver driver = new FirefoxDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

```
but only for browsers that require a separate WebDriver binary. 

This library provides a consistent interface for cross browser testing.
