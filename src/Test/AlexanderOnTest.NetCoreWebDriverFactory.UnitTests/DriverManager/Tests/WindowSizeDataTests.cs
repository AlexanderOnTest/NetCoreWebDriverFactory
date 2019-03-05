using AlexanderOnTest.NetCoreWebDriverFactory;
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
        [TestCase(WindowSize.Unchanged, 0, 0)]
        [TestCase(WindowSize.Custom, 0, 0)]
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
