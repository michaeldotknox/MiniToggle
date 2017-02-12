using System;
using System.Threading.Tasks;
using FluentAssertions;
using MiniToggle.Core;
using MiniToggle.Core.Exceptions;
using NUnit.Framework;

namespace MiniToggle.UnitTests
{
    [TestFixture]
    public class ToggleExtensionTests
    {
        [Test]
        public void ToggleConfigurationCachedThrowsOnNullToggleProperty()
        {
            // Arrange
            var toggle = new ToggleConfiguration();

            // Act
            Func<Task<CachedToggle>> action = () => Task.Run(() => Toggle.Cached(toggle));

            // Assert
            action.ShouldThrow<ToggleCannotBeNullException>();
        }

        [Test]
        public void ToggleConfigurationCachedReturnsNonNullCachedToggle()
        {
            // Arrange
            var toggle = new ToggleConfiguration
            {
                Toggle = typeof(Toggles.AlwaysTrueToggle)
            };

            // Act
            var result = Toggle.Cached(toggle);

            // Assert
            result.Should().NotBeNull();
        }

        [Test]
        public void ToggleConfigurationCachedReturnsCachedToggleWithTogglePropertySet()
        {
            // Arrange
            var toggle = new ToggleConfiguration
            {
                Toggle = typeof(Toggles.AlwaysTrueToggle)
            };

            // Act
            var result = Toggle.Cached(toggle);

            // Assert
            result.Toggle.ShouldBeEquivalentTo(toggle.Toggle);
        }
    }
}
