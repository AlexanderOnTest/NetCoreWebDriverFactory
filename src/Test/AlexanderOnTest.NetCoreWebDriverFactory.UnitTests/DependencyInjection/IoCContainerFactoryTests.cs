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
using System.Drawing;
using System.Reflection;
using AlexanderOnTest.NetCoreWebDriverFactory.DependencyInjection;
using AlexanderOnTest.NetCoreWebDriverFactory.DriverManager;
using AlexanderOnTest.NetCoreWebDriverFactory.Utils.Builders;
using AlexanderOnTest.NetCoreWebDriverFactory.WebDriverFactory;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using OpenQA.Selenium;

namespace AlexanderOnTest.NetCoreWebDriverFactory.UnitTests.DependencyInjection
{
    [Category("CI")]
    public class IoCContainerFactoryTests
    {
        private static readonly DriverPath DriverPath = new DriverPath(Assembly.GetExecutingAssembly());
        private static readonly Uri GridUri = new Uri("http://192.168.0.200:4444/wd/hub");

        private static readonly IWebDriverConfiguration TestConfiguration = WebDriverConfigurationBuilder
            .Start()
            .WithBrowser(Browser.Firefox)
            .RunHeadless()
            .WithCustomSize(new Size(1920, 1080))
            .RunRemotelyOn(GridUri)
            .WithPlatformType(PlatformType.Linux)
            .Build();

        private static readonly IWebDriverConfiguration DefaultConfigurationWithGridUri = WebDriverConfigurationBuilder
            .Start()
            .WithGridUri(GridUri)
            .Build();

        [Test]
        [TestCase(PathDefinition.Defined, typeof(DefaultLocalWebDriverFactory))]
        [TestCase(PathDefinition.Implicit, typeof(FrameworkLocalWebDriverFactory))]
        public void CanGetCorrectLocalFactoryFromServiceCollection(PathDefinition driverPathType, Type expectedType)
        {
            IServiceCollection serviceCollection;

            switch (driverPathType)
            {
                case PathDefinition.Defined:
                    serviceCollection = ServiceCollectionFactory
                        .GetDefaultServiceCollection(DriverPath, GridUri);
                    break;

                default:
                    serviceCollection = ServiceCollectionFactory
                        .GetDefaultServiceCollection(GridUri);
                    break;
            }

            IServiceProvider provider = serviceCollection.BuildServiceProvider();

            using (new AssertionScope())
            {
                provider.Should().NotBe(null);
                provider.GetService<ILocalWebDriverFactory>()
                    .Should().BeOfType(expectedType);
            }
        }

        [Test]
        public void CanGetRemoteIoCContainer()
        {
            IServiceProvider provider = ServiceCollectionFactory
                .GetDefaultServiceCollection(GridUri)
                .BuildServiceProvider();

            using (new AssertionScope())
            {
                provider.Should().NotBe(null);
                IRemoteWebDriverFactory remoteWebDriverFactory = provider.GetService<IRemoteWebDriverFactory>();
                remoteWebDriverFactory.Should().BeOfType(typeof(DefaultRemoteWebDriverFactory));
                remoteWebDriverFactory.GridUri.Should().BeEquivalentTo(GridUri);
            }
        }

        [Test]
        public void CanGetFullIoCContainer()
        {
            IServiceProvider provider = ServiceCollectionFactory
                .GetDefaultServiceCollection(DriverPath, GridUri)
                .BuildServiceProvider();

            IWebDriverFactory webDriverFactory = provider.GetService<IWebDriverFactory>();

            webDriverFactory.Should().BeOfType<DefaultWebDriverFactory>();
        }

        [Test]
        public void PathDefinedServiceCollectionProvidesCorrectImplementations()
        {
            IServiceProvider provider = ServiceCollectionFactory
                .GetDefaultServiceCollection(DriverPath, GridUri)
                .BuildServiceProvider();

            IWebDriverFactory webDriverFactory = provider.GetService<IWebDriverFactory>();
            DefaultWebDriverFactory factory = (DefaultWebDriverFactory) webDriverFactory;

            using (new AssertionScope())
            {
                factory.LocalWebDriverFactory.Should().BeOfType<DefaultLocalWebDriverFactory>();
                factory.RemoteWebDriverFactory.Should().BeOfType<DefaultRemoteWebDriverFactory>();
            }
        }

        [Test]
        public void PathUndefinedServiceCollectionProvidesCorrectImplementations()
        {
            IServiceProvider provider = ServiceCollectionFactory
                .GetDefaultServiceCollection(GridUri)
                .BuildServiceProvider();

            IWebDriverFactory webDriverFactory = provider.GetService<IWebDriverFactory>();
            DefaultWebDriverFactory factory = (DefaultWebDriverFactory) webDriverFactory;

            using (new AssertionScope())
            {
                factory.LocalWebDriverFactory.Should().BeOfType<FrameworkLocalWebDriverFactory>();
                factory.RemoteWebDriverFactory.Should().BeOfType<DefaultRemoteWebDriverFactory>();
            }
        }

        [Test]
        public void ServiceCollectionProvidesCorrectlyConfiguredIWebDriverManager()
        {

            IServiceProvider provider = ServiceCollectionFactory
                .GetDefaultServiceCollection(DriverPath, TestConfiguration)
                .BuildServiceProvider();

            using (new AssertionScope())
            {
                object webDriverManager = provider.GetService(typeof(IWebDriverManager));
                FieldInfo storedDriverConfigField = webDriverManager
                    .GetType()
                    .GetField("driverConfig", BindingFlags.NonPublic | BindingFlags.Instance);
                IWebDriverConfiguration storedDriverConfig =
                    (IWebDriverConfiguration) storedDriverConfigField.GetValue(webDriverManager);
                storedDriverConfig.Should().BeEquivalentTo(TestConfiguration);
            }
        }

        [Test]
        public void ServiceCollectionProvidesCorrectlyConfiguredIWebDriverManagerfromUri()
        {

            IServiceProvider provider = ServiceCollectionFactory
                .GetDefaultServiceCollection(DriverPath, GridUri)
                .BuildServiceProvider();

            using (new AssertionScope())
            {
                object webDriverManager = provider.GetService(typeof(IWebDriverManager));
                FieldInfo storedDriverConfigField = webDriverManager
                    .GetType()
                    .GetField("driverConfig", BindingFlags.NonPublic | BindingFlags.Instance);
                IWebDriverConfiguration storedDriverConfig =
                    (IWebDriverConfiguration) storedDriverConfigField.GetValue(webDriverManager);
                storedDriverConfig.Should().BeEquivalentTo(DefaultConfigurationWithGridUri);
            }
        }
    }


    public enum PathDefinition
    {
        Defined,
        Implicit,
        NetCoreNugetPath
    }

    public enum GridUriDefinition
    {
        Config,
        Uri,
        Implicit
    }
}