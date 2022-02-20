using System;
using System.Collections.Generic;
using System.Linq;
using AlexanderOnTest.NetCoreWebDriverFactory.DependencyInjection;
using AlexanderOnTest.NetCoreWebDriverFactory.DriverOptionsFactory;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Safari;

namespace AlexanderOnTest.NetCoreWebDriverFactory.UnitTests.DriverOptionsFactory;

public class RemoteDriverOptionsFactoryTests
{
    private IDriverOptionsFactory optionsFactory;

    public RemoteDriverOptionsFactoryTests()
    {
        optionsFactory = ServiceCollectionFactory.GetDefaultServiceCollection()
            .BuildServiceProvider()
            .GetRequiredService<IDriverOptionsFactory>();
    }
    
    [TestCase(PlatformType.Windows)]
    [TestCase(PlatformType.Linux)]
    [TestCase(PlatformType.Mac)]
    public void PlatformIsSetCorrectlyForChrome(PlatformType platformType)
    {
        ChromeOptions options = optionsFactory.GetRemoteDriverOptions<ChromeOptions>(platformType);
        options?.PlatformName?.ToLower().Should().Be(platformType.ToString().ToLower());
    }

    [TestCase]
    public void RequestingHeadlessModeThrowsCorrectlyForSafari()
    {
        optionsFactory
            .Invoking(of => of.GetRemoteDriverOptions<SafariOptions>(PlatformType.Mac, true))
            .Should().Throw<ArgumentException>()
            .WithMessage("Only Chrome, Edge and Firefox support headless operation");
    }

    [TestCase]
    public void RequestingOnScreenModeDoesNotThrowForSafari()
    {
        optionsFactory
            .Invoking(of => of.GetRemoteDriverOptions<SafariOptions>(PlatformType.Mac, false))
            .Should().NotThrow();
    }

    [TestCase]
    public void RequestingHeadlessModeThrowsCorrectlyForInternetExplorer()
    {
        optionsFactory
            .Invoking(of => of.GetRemoteDriverOptions<InternetExplorerOptions>(PlatformType.Windows, true))
            .Should().Throw<ArgumentException>()
            .WithMessage("Only Chrome, Edge and Firefox support headless operation");
    }

    [TestCase]
    public void RequestingOnScreenModeThrowsCorrectlyForInternetExplorer()
    {
        optionsFactory
            .Invoking(of => of.GetRemoteDriverOptions<InternetExplorerOptions>(PlatformType.Windows, false))
            .Should().NotThrow();
    }

    [TestCase(PlatformType.Windows)]
    [TestCase(PlatformType.Linux)]
    [TestCase(PlatformType.Mac)]
    public void HeadlessModeIsSetCorrectlyForChrome(PlatformType platformType)
    {
        ChromeOptions options = optionsFactory.GetRemoteDriverOptions<ChromeOptions>(platformType, true);
        options?.Arguments.Should().Contain("headless");
    }

    [TestCase(PlatformType.Windows)]
    [TestCase(PlatformType.Linux)]
    [TestCase(PlatformType.Mac)]
    public void HeadlessModeIsNotSetIncorrectlyForChrome(PlatformType platformType)
    {
        ChromeOptions options = optionsFactory.GetRemoteDriverOptions<ChromeOptions>(platformType, false);
        options?.Arguments.Should().NotContain("headless");
    }

    [TestCase(PlatformType.Windows)]
    [TestCase(PlatformType.Linux)]
    [TestCase(PlatformType.Mac)]
    public void HeadlessModeIsSetCorrectlyForEdge(PlatformType platformType)
    {
        EdgeOptions options = optionsFactory.GetRemoteDriverOptions<EdgeOptions>(platformType, true);
        options?.Arguments.Should().Contain("headless");
    }

    [TestCase(PlatformType.Windows)]
    [TestCase(PlatformType.Linux)]
    [TestCase(PlatformType.Mac)]
    public void HeadlessModeIsNotSetIncorrectlyForEdge(PlatformType platformType)
    {
        EdgeOptions options = optionsFactory.GetRemoteDriverOptions<EdgeOptions>(platformType, false);
        options?.Arguments.Should().NotContain("headless");
    }
    
    [TestCase(PlatformType.Windows)]
    [TestCase(PlatformType.Linux)]
    [TestCase(PlatformType.Mac)]
    public void HeadlessModeIsSetCorrectlyForFirefox(PlatformType platformType)
    {
        FirefoxOptions options = optionsFactory.GetRemoteDriverOptions<FirefoxOptions>(platformType, true);
        GetFirefoxArguments(options).Should().Contain("--headless");
    }

    [TestCase(PlatformType.Windows)]
    [TestCase(PlatformType.Linux)]
    [TestCase(PlatformType.Mac)]
    public void HeadlessModeIsNotSetIncorrectlyForFirefox(PlatformType platformType)
    {
        FirefoxOptions options = optionsFactory.GetRemoteDriverOptions<FirefoxOptions>(platformType,false);
        GetFirefoxArguments(options).Should().NotContain("--headless");
    }

    private List<string> GetFirefoxArguments(FirefoxOptions firefoxOptions)
    {
        List<string> argumentStringsList = new List<string>();
        ICapabilities capabilities = firefoxOptions?.ToCapabilities();
        if (capabilities != null && capabilities.HasCapability("moz:firefoxOptions"))
        {
            Dictionary<string, object> dictionaryOfOptions = capabilities.GetCapability("moz:firefoxOptions") as Dictionary<string, object>;
            object argumentObject = null;
            dictionaryOfOptions?.TryGetValue("args", out argumentObject);
            List<object> arguments = argumentObject as List<object>;
            if (arguments != null)
            {
                argumentStringsList = arguments?.Select(obj => obj as string).ToList();
            }
        }
        
        return argumentStringsList;
    }
}