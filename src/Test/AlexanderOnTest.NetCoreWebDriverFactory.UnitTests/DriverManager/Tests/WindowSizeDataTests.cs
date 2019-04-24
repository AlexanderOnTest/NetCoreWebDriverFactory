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

using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;

namespace AlexanderOnTest.NetCoreWebDriverFactory.UnitTests.DriverManager.Tests
{
    [Category("CI")]
    public class WindowSizeDataTests
    {
        [Test]
        [TestCase(WindowSize.Maximise, 0, 0)]
        [TestCase(WindowSize.Maximize, 0, 0)]
        [TestCase(WindowSize.Unchanged, 0, 0)]
        [TestCase(WindowSize.Custom, 0, 0)]
        [TestCase(WindowSize.Defined, 0, 0)]
        [TestCase(WindowSize.Hd, 1366, 768)]
        [TestCase(WindowSize.Fhd, 1920, 1080)]
        [TestCase(WindowSize.Qhd, 2560, 1440)]
        [TestCase(WindowSize.Uhd, 3840, 2160)]
        public void WindowSizeEnumReturnsSizeWhereValid(WindowSize windowSize, int expectedWidth, int expectedHeight)
        {
            using (new AssertionScope())
            {
                windowSize.Size().Width.Should().Be(expectedWidth);
                windowSize.Size().Height.Should().Be(expectedHeight);
            }
        }

        [Test]
        [TestCase(WindowSize.Maximise, false)]
        [TestCase(WindowSize.Maximize, false)]
        [TestCase(WindowSize.Unchanged, false)]
        [TestCase(WindowSize.Custom, false)]
        [TestCase(WindowSize.Hd, true)]
        [TestCase(WindowSize.Fhd, true)]
        [TestCase(WindowSize.Qhd, true)]
        [TestCase(WindowSize.Uhd, true)]
        public void WindowSizeEnumHaveSizeReturnsCorrectValue(WindowSize windowSize, bool shouldHaveSize)
        {
            windowSize.HasDefinedSize().Should().Be(shouldHaveSize);
        }
    }
}
