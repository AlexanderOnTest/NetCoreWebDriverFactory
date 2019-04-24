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
using System.Runtime.InteropServices;
using AlexanderOnTest.NetCoreWebDriverFactory.DependencyInjection;
using AlexanderOnTest.NetCoreWebDriverFactory.WebDriverFactory;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using OpenQA.Selenium;

namespace AlexanderOnTest.NetCoreWebDriverFactory.Lib.Test
{
    [TestFixture]
    public abstract class RemoteWebDriverFactoryTestsBase
    {
        private readonly OSPlatform thisPlatform;
        private readonly Uri gridUrl;

        protected RemoteWebDriverFactoryTestsBase(OSPlatform thisPlatform, Uri gridUrl)
        {
            this.thisPlatform = thisPlatform;
            this.gridUrl = gridUrl;
        }

        private IRemoteWebDriverFactory RemoteWebDriverFactory { get; set; }
        private IWebDriver Driver { get; set; }

        [OneTimeSetUp]
        public void SetUp()
        {
            Assume.That(() => RuntimeInformation.IsOSPlatform(thisPlatform));

            IServiceProvider provider = ServiceCollectionFactory
                .GetDefaultServiceCollection(gridUrl)
                .BuildServiceProvider();
            RemoteWebDriverFactory = provider.GetService<IRemoteWebDriverFactory>();
        }
        
        public void RemoteWebDriverFactoryWorks(PlatformType platformType, Browser browser)
        {
            Driver = RemoteWebDriverFactory.GetWebDriver(browser, platformType);
            Assertions.AssertThatPageCanBeLoaded(Driver);
        }

        [TearDown]
        public void Teardown()
        {
            Driver?.Quit();
        }
    }
}
