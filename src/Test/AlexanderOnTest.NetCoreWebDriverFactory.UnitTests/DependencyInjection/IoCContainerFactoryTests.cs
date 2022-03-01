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
using System.Globalization;
using System.Reflection;
using AlexanderOnTest.NetCoreWebDriverFactory.Config;
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
        private static readonly DriverPath DriverPath = 
            new (Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
        private static readonly Uri GridUri = new ("http://localhost:4444");

        private static readonly IWebDriverConfiguration TestConfiguration = WebDriverConfigurationBuilder
            .Start()
            .WithBrowser(Browser.Firefox)
            .RunHeadless()
            .WithCustomSize(new Size(1920, 1080))
            .RunRemotelyOn(GridUri)
            .WithPlatformType(PlatformType.Linux)
            .WithlanguageCulture(new CultureInfo("en-GB"))
            .Build();

        private static readonly IWebDriverConfiguration DefaultConfigurationWithGridUri = WebDriverConfigurationBuilder
            .Start()
            .WithGridUri(GridUri)
            .Build();

        [Test]
        public void CanGetCorrectLocalFactoryFromServiceCollection()
        {
            IServiceCollection serviceCollection = ServiceCollectionFactory
                        .GetDefaultServiceCollection(GridUri);

            IServiceProvider provider = serviceCollection.BuildServiceProvider();

            using (new AssertionScope())
            {
                provider.Should().NotBe(null);
                provider.GetService<ILocalWebDriverFactory>()
                    .Should().BeOfType(typeof(DefaultLocalWebDriverFactory));
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
                remoteWebDriverFactory?.GridUri.Should().BeEquivalentTo(GridUri);
            }
        }

        [Test]
        public void CanGetFullIoCContainer()
        {
            IServiceProvider provider = ServiceCollectionFactory
                .GetDefaultServiceCollection(GridUri, DriverPath)
                .BuildServiceProvider();

            IWebDriverFactory webDriverFactory = provider.GetService<IWebDriverFactory>();

            webDriverFactory.Should().BeOfType<DefaultWebDriverFactory>();
            provider.GetService<DriverPath>().Should().NotBeNull();
        }

        [Test]
        public void PathDefinedServiceCollectionProvidesCorrectImplementations()
        {
            IServiceProvider provider = ServiceCollectionFactory
                .GetDefaultServiceCollection(GridUri, DriverPath)
                .BuildServiceProvider();

            IWebDriverFactory webDriverFactory = provider.GetService<IWebDriverFactory>();
            var defaultWebDriverFactory = webDriverFactory as DefaultWebDriverFactory;

            using (new AssertionScope())
            {
                defaultWebDriverFactory.LocalWebDriverFactory.Should().BeOfType<DefaultLocalWebDriverFactory>();
                defaultWebDriverFactory.RemoteWebDriverFactory.Should().BeOfType<DefaultRemoteWebDriverFactory>();
            }

            var localWebDriverFactory = defaultWebDriverFactory.LocalWebDriverFactory as DefaultLocalWebDriverFactory;
            localWebDriverFactory.InstalledDriverPath.Should().Be(DriverPath.PathString);
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
                factory.LocalWebDriverFactory.Should().BeOfType<DefaultLocalWebDriverFactory>();
                factory.RemoteWebDriverFactory.Should().BeOfType<DefaultRemoteWebDriverFactory>();
            }
        }

        [Test]
        public void ServiceCollectionProvidesCorrectlyConfiguredIWebDriverManager()
        {

            IServiceProvider provider = ServiceCollectionFactory
                .GetDefaultServiceCollection(TestConfiguration, DriverPath)
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
        public void ServiceCollectionProvidesCorrectlyConfiguredIWebDriverManagerFromUri()
        {

            IServiceProvider provider = ServiceCollectionFactory
                .GetDefaultServiceCollection(GridUri, DriverPath)
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