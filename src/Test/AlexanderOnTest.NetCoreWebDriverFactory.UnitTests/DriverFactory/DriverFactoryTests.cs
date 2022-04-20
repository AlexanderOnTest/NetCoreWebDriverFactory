using System;
using AlexanderOnTest.NetCoreWebDriverFactory.DependencyInjection;
using AlexanderOnTest.NetCoreWebDriverFactory.WebDriverFactory;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace AlexanderOnTest.NetCoreWebDriverFactory.UnitTests.DriverFactory;

[Category("CI")]
public class DriverFactoryTests
{
    private IWebDriverFactory webDriverFactory;
    
    [OneTimeSetUp]
    public void Setup()
    {
        IServiceCollection serviceCollection = ServiceCollectionFactory.GetDefaultServiceCollection();
        IServiceProvider provider = serviceCollection.BuildServiceProvider();
        webDriverFactory = provider.GetRequiredService<IWebDriverFactory>();
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
        var func = () => webDriverFactory.GetWebDriver(browser);
        func.Should().ThrowExactly<NotSupportedException>().WithMessage("* is not currently supported.");
    }
}