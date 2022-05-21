# Selenium.WebDriver.NetCoreWebDriverFactory
### Version 4.x
An extensible library to ease the configuration and launching of Selenium WebDriver instances in .net / .net Core / .net Framework projects.
Supports browser instances run locally or on a Selenium grid v4.

Supported Browsers:
- Chrome
- Firefox
- Edge
- Internet Explorer 11 (Windows Platforms remotely and locally including Edge Internet Explorer mode)
- Safari (MacOS)

Platforms:
+ Windows (PlatformType.Windows)
+ Linux        (PlatformType.Linux)
+ MacOS     (PlatformType.Mac)

Tested and working on Windows 10/11, Linux (Ubuntu 22.04) and macOS Monterey

## NUnit3 auto configuration library available
[Selenium.WebDriver.WebDriverFactoryNunitConfig](https://www.nuget.org/packages/Selenium.WebDriver.WebDriverFactoryNunitConfig/)
provides additional code to pull the required configuration from *.runsettings files including the option to override with local configuration files.

This can be used to completely separate WebDriver configuration from your test code.

## Designed for Dependency Injection
[ServiceCollectionFactory.cs](https://github.com/AlexanderOnTest/NetCoreWebDriverFactory/blob/v4_0_0/src/Main/AlexanderOnTest.NetCoreWebDriverFactory/DependencyInjection/ServiceCollectionFactory.cs) 
includes static methods that provide a configured [Microsoft.Extensions.DependencyInjection.IServiceCollection](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection?msclkid=a366e91ec0f611ec91177292d9ba0435&view=dotnet-plat-ext-6.0)
for use in your project although you may prefer to use the code as inspiration for your own implementation / module.

e.g. NUnit3 example
```
    private IWebDriverFactory webDriverFactory;
    
    [OneTimeSetUp]
    public void Setup()
    {
        IServiceCollection serviceCollection = ServiceCollectionFactory.GetDefaultServiceCollection();
        IServiceProvider provider = serviceCollection.BuildServiceProvider();
        webDriverFactory = provider.GetRequiredService<IWebDriverFactory>();
    }
```

## Fluent Browser configuration including window size
[WebDriverConfigurationBuilder.cs](https://github.com/AlexanderOnTest/NetCoreWebDriverFactory/blob/v4_0_0/src/Main/AlexanderOnTest.NetCoreWebDriverFactory/Utils/Builders/WebDriverConfigurationBuilder.cs)
provides a fluent interface to generate an [IWebDriverConfiguration.cs](https://github.com/AlexanderOnTest/NetCoreWebDriverFactory/blob/v4_0_0/src/Main/AlexanderOnTest.NetCoreWebDriverFactory/Config/IWebDriverConfiguration.cs)
defining the characteristics of the WebDriver instance you desire.

e.g.

````
IWebDriverConfiguration configuration = WebDriverConfigurationBuilder.Start()
                             .WithBrowser(Browser.Firefox)
                             .RunHeadless()
                             .RunRemotelyOn(new Uri("http://localhost:4444"))
                             .WithPlatformType(PlatformType.Mac)
                             .WithLanguageCulture(new CultureInfo("es-Es"))
                             .WithWindowSize(WindowSize.Fhd)
                             .Build();
````

## WebDriver launching using the WebDriverFactory
```
IWebDriver driver = webDriverFactory.GetWebDriver(configuration);
```

## Optional WebDriver instance management
To generate and reuse one or more WebDriver instances of a single configuration use a [WebDriverManager.cs](https://github.com/AlexanderOnTest/NetCoreWebDriverFactory/blob/v4_0_0/src/Main/AlexanderOnTest.NetCoreWebDriverFactory/DriverManager/WebDriverManager.cs)
```
IWebDriverManager webDriverManager = new WebDriverManager(webDriverFactory, configuration)

IWebDriver driver = webDriverManager.Get();
```

Repeated calls to `webDriverManager.Get()` will return the same singleton IWebDriver instance until it is closed by calling `webDriverManager.Quit()`.

## Limited logging support available using [LibLog](https://github.com/damianh/LibLog/wiki)

## Guidance on unsupported configurations
The library attempts to provide informative exceptions where the requested WebDriver does not support the requested options.
As supported features do change, these are not guaranteed to remain correct.

## Extensible
All services are defined as interfaces so you can provide alternative or additional implementations if needed.
- The [Browser.cs](https://github.com/AlexanderOnTest/NetCoreWebDriverFactory/blob/v4_0_0/src/Main/AlexanderOnTest.NetCoreWebDriverFactory/Browser.cs) Enum in particular has been populated with many potential additional options that you can provide your own implementations to support.
- The configurations also support custom window sizes if the pre-defined options are not enough.