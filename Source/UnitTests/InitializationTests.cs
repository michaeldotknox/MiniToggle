using System;
using System.Linq;
using System.Reflection;
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
            var toggles =
                Assembly
                    .GetExecutingAssembly()
                    .GetTypes()
                    .Count(_ => _.GetInterfaces().Contains(typeof (IToggle)));

            // Act

            // Assert
            Toggle.Toggles.Should().HaveCount(toggles);
        }
    }
}
