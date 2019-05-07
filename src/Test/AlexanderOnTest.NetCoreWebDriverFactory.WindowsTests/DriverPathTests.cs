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

using System.Reflection;
using AlexanderOnTest.NetCoreWebDriverFactory.DependencyInjection;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace AlexanderOnTest.NetCoreWebDriverFactory.WindowsTests
{
    public class DriverPathTests
    {
        private static readonly DriverPath ExpectedDriverPath = new DriverPath(Assembly.GetExecutingAssembly());

        [Test]
        public void DriverPathLocatedByInstalledPackageIsTheSameAsLocatedByThisAssembly()
        {
            DriverPath implicitDriverPathLocatedByInstalledPackage = ServiceCollectionFactory
                .GetDefaultServiceCollection(true)
                .BuildServiceProvider()
                .GetService<DriverPath>();

            implicitDriverPathLocatedByInstalledPackage.Should().BeEquivalentTo(ExpectedDriverPath);
        }

        [Test]
        public void DriverPathReturnedShouldBeNullIfNoPathIsSpecified()
        {
            DriverPath driverPathReturnedWhenNoPathIsSpecified = ServiceCollectionFactory
                .GetDefaultServiceCollection()
                .BuildServiceProvider().GetService<DriverPath>();

            driverPathReturnedWhenNoPathIsSpecified.Should().BeNull();
        }

        [Test]
        public void DriverPathIsNullWithFalsePassedIn()
        {
            DriverPath driverPathWithFalsePassedIn = ServiceCollectionFactory
                .GetDefaultServiceCollection(false)
                .BuildServiceProvider().GetService<DriverPath>();

            driverPathWithFalsePassedIn.Should().BeNull();
        }

        [Test]
        public void DriverPathReturnedWhenPassedInIsAsExpected()
        {
            DriverPath driverPathReturned = ServiceCollectionFactory
                .GetDefaultServiceCollection(ExpectedDriverPath)
                .BuildServiceProvider()
                .GetService<DriverPath>();

            driverPathReturned.Should().BeEquivalentTo(ExpectedDriverPath);
        }
    }
}
