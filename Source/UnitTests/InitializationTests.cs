using FluentAssertions;
using MiniToggle.Core;
using NUnit.Framework;

namespace MiniToggle.UnitTests
{
    [TestFixture]
    public class InitializationTests
    {
        [Test]
        public void CTorCreatesAToggleRecordForEachClassThatImplementsIToggle()
        {
            // Arrange

            // Act

            // Assert
            Toggle.Toggles.Should().HaveCount(6);
        }
    }
}
