// <copyright>
// Copyright 2018 Alexander Dunn
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
using System.Globalization;
using AlexanderOnTest.NetCoreWebDriverFactory.DependencyInjection;
using AlexanderOnTest.NetCoreWebDriverFactory.DriverOptionsFactory;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Safari;

using static AlexanderOnTest.NetCoreWebDriverFactory.UnitTests.DriverOptionsFactory.StaticDriverOptionsFactoryCultureTests;


namespace AlexanderOnTest.NetCoreWebDriverFactory.UnitTests.DriverOptionsFactory;

[Category("CI")]
public class LocalDriverOptionsCultureTests
{
    private IDriverOptionsFactory optionsFactory;

    public LocalDriverOptionsCultureTests()
    {
        optionsFactory = ServiceCollectionFactory.GetDefaultServiceCollection()
            .BuildServiceProvider()
            .GetRequiredService<IDriverOptionsFactory>();
    }
    
    private static CultureInfo CultureInfo => new("en-GB");
    
    [Test]
    public void RequestingHeadlessModeThrowsCorrectlyForChrome()
    {
        optionsFactory
            .Invoking(of => of.GetLocalDriverOptions<ChromeOptions>(true, CultureInfo))
            .Should().Throw<NotSupportedException>()
            .WithMessage(HeadlessBrowserNotSupportedMessage);
    }
    
    [Test]
    public void RequestingOnScreenModeReturnsOptionsForChrome()
    {
        optionsFactory
            .Invoking(of => of.GetLocalDriverOptions<ChromeOptions>(false, CultureInfo))
            .Should().NotThrow();
        
        var firefoxOptions = optionsFactory.GetLocalDriverOptions<ChromeOptions>(false, CultureInfo);

        using (new AssertionScope())
        {
            firefoxOptions.Should().BeOfType<ChromeOptions>();
            firefoxOptions.Should().NotBeNull();
        }
    }
    
    [Test]
    public void RequestingHeadlessModeThrowsCorrectlyForEdge()
    {
        optionsFactory
            .Invoking(of => of.GetLocalDriverOptions<EdgeOptions>(true, CultureInfo))
            .Should().Throw<NotSupportedException>()
            .WithMessage(HeadlessBrowserNotSupportedMessage);
    }
    
    [Test]
    public void RequestingOnScreenModeReturnsOptionsForEdge()
    {
        optionsFactory
            .Invoking(of => of.GetLocalDriverOptions<EdgeOptions>(false, CultureInfo))
            .Should().NotThrow();
        
        var firefoxOptions = optionsFactory.GetLocalDriverOptions<EdgeOptions>(false, CultureInfo);

        using (new AssertionScope())
        {
            firefoxOptions.Should().BeOfType<EdgeOptions>();
            firefoxOptions.Should().NotBeNull();
        }
    }
    
    [Test]
    public void RequestingHeadlessModeReturnsOptionsForFirefox()
    {
        optionsFactory
            .Invoking(of => of.GetLocalDriverOptions<FirefoxOptions>(true, CultureInfo))
            .Should().NotThrow();
        
        var firefoxOptions = optionsFactory.GetLocalDriverOptions<FirefoxOptions>(true, CultureInfo);

        using (new AssertionScope())
        {
            firefoxOptions.Should().BeOfType<FirefoxOptions>();
            firefoxOptions.Should().NotBeNull();
        }
    }
    
    [Test]
    public void RequestingOnScreenModeReturnsOptionsForFirefox()
    {
        optionsFactory
            .Invoking(of => of.GetLocalDriverOptions<FirefoxOptions>(false, CultureInfo))
            .Should().NotThrow();
        
        var firefoxOptions = optionsFactory.GetLocalDriverOptions<FirefoxOptions>(false, CultureInfo);

        using (new AssertionScope())
        {
            firefoxOptions.Should().BeOfType<FirefoxOptions>();
            firefoxOptions.Should().NotBeNull();
        }
        
    }
    
    [Test]
    public void RequestingHeadlessModeThrowsCorrectlyForInternetExplorer()
    {
        optionsFactory
            .Invoking(of => of.GetLocalDriverOptions<InternetExplorerOptions>(true, CultureInfo))
            .Should().Throw<NotSupportedException>()
            .WithMessage(UnsupportedBrowserMessage);
    }
    
    [Test]
    public void RequestingOnScreenModeThrowsCorrectlyForInternetExplorer()
    {
        optionsFactory
            .Invoking(of => of.GetLocalDriverOptions<InternetExplorerOptions>(false, CultureInfo))
            .Should().Throw<NotSupportedException>()
            .WithMessage(UnsupportedBrowserMessage);
    }
    
    [Test]
    public void RequestingHeadlessModeThrowsCorrectlyForSafari()
    {
        optionsFactory
            .Invoking(of => of.GetLocalDriverOptions<SafariOptions>(true, CultureInfo))
            .Should().Throw<NotSupportedException>()
            .WithMessage(UnsupportedBrowserMessage);
    }
    
    [Test]
    public void RequestingOnScreenModeThrowsCorrectlyForSafari()
    {
        optionsFactory
            .Invoking(of => of.GetLocalDriverOptions<SafariOptions>(false, CultureInfo))
            .Should().Throw<NotSupportedException>()
            .WithMessage(UnsupportedBrowserMessage);
    }
}