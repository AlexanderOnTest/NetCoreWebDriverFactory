﻿using System;
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

public class LocalDriverOptionsFactoryTests
{
    private IDriverOptionsFactory optionsFactory;

    public LocalDriverOptionsFactoryTests()
    {
        optionsFactory = ServiceCollectionFactory.GetDefaultServiceCollection()
            .BuildServiceProvider()
            .GetRequiredService<IDriverOptionsFactory>();
    }

    [TestCase]
    public void RequestingHeadlessModeThrowsCorrectlyForSafari()
    {
        optionsFactory
            .Invoking(of => of.GetLocalDriverOptions<SafariOptions>(true))
            .Should().Throw<ArgumentException>()
            .WithMessage("Only Chrome, Edge and Firefox support headless operation");
    }

    [TestCase]
    public void RequestingOnScreenModeDoesNotThrowForSafari()
    {
        optionsFactory
            .Invoking(of => of.GetLocalDriverOptions<SafariOptions>(false))
            .Should().NotThrow();
    }

    [TestCase]
    public void RequestingHeadlessModeThrowsCorrectlyForInternetExplorer()
    {
        optionsFactory
            .Invoking(of => of.GetLocalDriverOptions<InternetExplorerOptions>(true))
            .Should().Throw<ArgumentException>()
            .WithMessage("Only Chrome, Edge and Firefox support headless operation");
    }

    [TestCase]
    public void RequestingOnScreenModeThrowsCorrectlyForInternetExplorer()
    {
        optionsFactory
            .Invoking(of => of.GetLocalDriverOptions<InternetExplorerOptions>(false))
            .Should().NotThrow();
    }

    [TestCase]
    [TestCase]
    [TestCase]
    public void HeadlessModeIsSetCorrectlyForChrome()
    {
        ChromeOptions options = optionsFactory.GetLocalDriverOptions<ChromeOptions>(true);
        options?.Arguments.Should().Contain("headless");
    }

    [TestCase]
    [TestCase]
    [TestCase]
    public void HeadlessModeIsNotSetIncorrectlyForChrome()
    {
        ChromeOptions options = optionsFactory.GetLocalDriverOptions<ChromeOptions>(false);
        options?.Arguments.Should().NotContain("headless");
    }

    [TestCase]
    [TestCase]
    [TestCase]
    public void HeadlessModeIsSetCorrectlyForEdge()
    {
        EdgeOptions options = optionsFactory.GetLocalDriverOptions<EdgeOptions>(true);
        options?.Arguments.Should().Contain("headless");
    }

    [TestCase]
    [TestCase]
    [TestCase]
    public void HeadlessModeIsNotSetIncorrectlyForEdge()
    {
        EdgeOptions options = optionsFactory.GetLocalDriverOptions<EdgeOptions>(false);
        options?.Arguments.Should().NotContain("headless");
    }
    
    [TestCase]
    [TestCase]
    [TestCase]
    public void HeadlessModeIsSetCorrectlyForFirefox()
    {
        FirefoxOptions options = optionsFactory.GetLocalDriverOptions<FirefoxOptions>(true);
        GetFirefoxArguments(options).Should().Contain("--headless");
    }

    [TestCase]
    [TestCase]
    [TestCase]
    public void HeadlessModeIsNotSetIncorrectlyForFirefox()
    {
        FirefoxOptions options = optionsFactory.GetLocalDriverOptions<FirefoxOptions>(false);
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