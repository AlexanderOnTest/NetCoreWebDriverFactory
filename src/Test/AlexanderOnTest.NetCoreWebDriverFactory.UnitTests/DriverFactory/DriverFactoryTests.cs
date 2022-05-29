using System;
using AlexanderOnTest.NetCoreWebDriverFactory.DependencyInjection;
using AlexanderOnTest.NetCoreWebDriverFactory.Utils.Builders;
using AlexanderOnTest.NetCoreWebDriverFactory.WebDriverFactory;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using OpenQA.Selenium;

namespace AlexanderOnTest.NetCoreWebDriverFactory.UnitTests.DriverFactory;

[Category("CI")]
public class DriverFactoryTests
{
    private IServiceProvider _provider;
    
    [OneTimeSetUp]
    public void Setup()
    {
        IServiceCollection serviceCollection = ServiceCollectionFactory.GetDefaultServiceCollection();
        _provider = serviceCollection.BuildServiceProvider();
    }
    
    [TestCase(Browser.ChromeAlternate)]
    [TestCase(Browser.ChromeBeta)]
    [TestCase(Browser.ChromeDev)]
    [TestCase(Browser.EdgeAlternate)]
    [TestCase(Browser.EdgeBeta)]
    [TestCase(Browser.EdgeDev)]
    [TestCase(Browser.FirefoxAlternate)]
    [TestCase(Browser.FirefoxBeta)]
    [TestCase(Browser.FirefoxDev)]
    [TestCase(Browser.InternetExplorerAlternate)]
    [TestCase(Browser.SafariAlternate)]
    [TestCase(Browser.Custom1)]
    [TestCase(Browser.Custom2)]
    [TestCase(Browser.Custom3)]
    [TestCase(Browser.Custom4)]
    [TestCase(Browser.Custom5)]
    public void UnimplementedBrowserReturnsSuitableException(Browser browser)
    {
        var webDriverFactory = _provider.GetRequiredService<IWebDriverFactory>();
        var func = () => webDriverFactory.GetWebDriver(browser);
        func.Should().ThrowExactly<NotSupportedException>().WithMessage("* is not currently supported.");
    }

    [Test]
    public void ConfigUriOverridesFactoryUri()
    {
        var remoteWebDriverFactory = _provider.GetRequiredService<IRemoteWebDriverFactory>();

        string noGridUri = "http://192.168.1.1:44444";
        var configuration = WebDriverConfigurationBuilder.Start().RunRemotelyOn(new (noGridUri)).Build();
        
        var func = () => remoteWebDriverFactory.GetWebDriver(configuration);
        
        func.Should().ThrowExactly<WebDriverException>().WithMessage($"* {noGridUri}*");
    }

    [Test]
    public void RequestingBrowserUsesFactoryUri()
    {
        var remoteWebDriverFactory = _provider.GetRequiredService<IRemoteWebDriverFactory>();

        var func = () => remoteWebDriverFactory.GetWebDriver(Browser.Chrome);
        
        func.Should().ThrowExactly<WebDriverException>().WithMessage($"* http://localhost:4444/*");
    }
}