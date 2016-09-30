using System;
using FluentAssertions;
using MiniToggle.Core;
using MiniToggle.UnitTests.Toggles;
using NUnit.Framework;

namespace MiniToggle.UnitTests
{
    [TestFixture]
    public class ToggleStatusTests
    {
        [Test]
        public void UnregisteredToggleThrowsException()
        {
            // Arrange

            // Act
            Action action = () => Toggle<UninitializedToggle>.IsEnabled();

            // Assert
            action.ShouldThrow<Core.Exceptions.ToggleNotConfiguredException>();
        }

        [Test]
        public void AlwaysTrueToggleConfiguredWithAttributeReturnsTrue()
        {
            // Arrange

            // Act
            var result = Toggle<AlwaysTrueToggleWithAttribute>.IsEnabled();

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void AlwaysTrueToggleConfiguredManuallyReturnsTrue()
        {
            // Arrange

            // Act
            var result = Toggle<AlwaysTrueToggle>.IsEnabled();

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void AlwaysFalseToggleConfiguredWIthAttributeReturnsFalse()
        {
            // Arrange

            // Act
            var result = Toggle<AlwaysFalseWithAttribute>.IsEnabled();

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public void AlwaysFalseToggleConfiguredManuallyReturnsFalse()
        {
            // Arrange

            // Act
            var result = Toggle<AlwaysFalseToggle>.IsEnabled();

            // Assert
            result.Should().BeFalse();
        }
    }
}
