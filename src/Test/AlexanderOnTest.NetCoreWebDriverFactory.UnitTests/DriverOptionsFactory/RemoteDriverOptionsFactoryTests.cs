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

[Category("CI")]
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

    [Test]
    public void RequestingHeadlessModeThrowsCorrectlyForSafari()
    {
        optionsFactory
            .Invoking(of => of.GetRemoteDriverOptions<SafariOptions>(PlatformType.Mac, true))
            .Should().Throw<ArgumentException>()
            .WithMessage("Only Chrome, Edge and Firefox support headless operation");
    }

    [Test]
    public void RequestingHeadlessModeThrowsCorrectlyForInternetExplorer()
    {
        optionsFactory
            .Invoking(of => of.GetRemoteDriverOptions<InternetExplorerOptions>(PlatformType.Windows, true))
            .Should().Throw<ArgumentException>()
            .WithMessage("Only Chrome, Edge and Firefox support headless operation");
    }

    [Test]
    public void RequestingOnScreenModeDoesNotThrowForSafari()
    {
        optionsFactory
            .Invoking(of => of.GetRemoteDriverOptions<SafariOptions>(PlatformType.Mac, false))
            .Should().NotThrow();
    }

    [Test]
    public void RequestingOnScreenModeDoesNotThrowForInternetExplorer()
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