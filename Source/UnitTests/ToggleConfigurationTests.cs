using FluentAssertions;
using MiniToggle.Core;
using MiniToggle.UnitTests.Toggles;
using NUnit.Framework;

namespace MiniToggle.UnitTests
{
    [TestFixture]
    public class ToggleConfigurationTests
    {
        [Test]
        public void AlwaysTrueSetsEvaluationPropertyOfToggle()
        {
            // Arrange
            var toggleType = typeof(AlwaysTrueToggle);
            var sut = new ToggleConfiguration
            {
                Toggle = toggleType
            };

            // Act
            sut.AlwaysTrue();

            // Assert
            Toggle.GetToggle(toggleType).Evaluation.Should().NotBeNull();
        }

        [Test]
        public void AlwaysTrueSetsEvaluationThatReturnsTrue()
        {
            // Arrange
            var toggleType = typeof (AlwaysTrueToggle);
            var sut = new ToggleConfiguration
            {
                Toggle = toggleType
            };

            // Act
            sut.AlwaysTrue();

            // Assert
            Toggle.GetToggle(toggleType).Evaluation().Should().BeTrue();
        }

        [Test]
        public void AlwaysFalseToggleSetsEvaluationPropertyOfToggle()
        {
            // Arrange
            var sut = new ToggleConfiguration
            {
                Toggle = typeof(AlwaysFalseToggle)
            };

            // Act
            sut.AlwaysFalse();

            // Assert
            Toggle.GetToggle(typeof(AlwaysFalseToggle)).Evaluation.Should().NotBeNull();
        }

        [Test]
        public void ConfiguredReturnsConfigurableToggleWithTogglePropertySet()
        {
            // Arrange
            var toggleType = typeof (ConfigurationFileToggle);
            var sut = new ToggleConfiguration
            {
                Toggle = toggleType
            };

            // Act
            var result = sut.Configured();

            // Assert
            result.Toggle.Should().Be(toggleType);
        }
    }
}
