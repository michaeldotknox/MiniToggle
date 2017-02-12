using System.Configuration;
using FluentAssertions;
using MiniToggle.Core.Attributes;
using MiniToggle.UnitTests.Toggles;
using NUnit.Framework;

namespace MiniToggle.UnitTests
{
    [TestFixture]
    public class AttributeTests
    {
        [Test]
        public void CTorWithNameSetsSettingNameProperty()
        {
            // Arrange
            const string toggleName = "onToggle";

            // Act
            var result = new SettingConfigurationAttribute(toggleName);

            // Assert
            result.SettingName.Should().Be(toggleName);
        }

        [Test]
        public void AlwaysFalseAttributeGetDefinitionReturnsNonNullToggleDefinition()
        {
            // Arrange
            var sut = new AlwaysFalseAttribute();

            // Act
            var result = sut.GetDefinition(typeof(AlwaysFalseToggle));

            // Assert
            result.Should().NotBeNull();
        }

        [Test]
        public void AlwaysFalseAttributeGetDefinitionReturnsEvaluationThatReturnsFalse()
        {
            // Arrange
            var sut = new AlwaysFalseAttribute();
            var definition = sut.GetDefinition(typeof(AlwaysFalseToggle));

            // Act
            var result = definition.Evaluation();

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public void AlwaysTrueAttributeGetDefinitionReturnsNonNullToggleDefinition()
        {
            // Arrange
            var sut = new AlwaysTrueAttribute();

            // Act
            var result = sut.GetDefinition(typeof(AlwaysTrueToggle));

            // Assert
            result.Should().NotBeNull();
        }

        [Test]
        public void AlwaysTrueAttributeGetDefinitionReturnsEvaluationThatReturnsTrue()
        {
            // Arrange
            var sut = new AlwaysTrueAttribute();
            var definition = sut.GetDefinition(typeof(AlwaysTrueToggle));

            // Act
            var result = definition.Evaluation();

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void SettingConfigurationAttributeGetDefinitionReturnsNonNullToggleDefinition()
        {
            // Arrange
            var sut = new SettingConfigurationAttribute("onToggle");

            // Act
            var result = sut.GetDefinition(typeof(ConfigurationFileToggle));

            // Assert
            result.Should().NotBeNull();
        }

        [Test]
        public void SettingConfigurationAttributeGetDefinitionReturnsEvaluationThatMatchesSettingValue()
        {
            // Arrange
            var sut = new SettingConfigurationAttribute("onToggle");
            var settingValue = bool.Parse(ConfigurationManager.AppSettings["onToggle"]);
            var definition = sut.GetDefinition(typeof(ConfigurationFileToggle));

            // Act
            var result = definition.Evaluation();

            // Assert
            result.Should().Be(settingValue);
        }

        [Test]
        public void SettingConfigurationAttributeGetDefinitionReturnsEvaluationThatReturnsTrueWhenSettingIsNotPresent()
        {
            // Arrange
            var sut = new SettingConfigurationAttribute("NotPresentSetting");
            var definition = sut.GetDefinition(typeof(ConfigurationFileToggle));

            // Act
            var result = definition.Evaluation();

            // Assert
            result.Should().BeTrue();

        }
    }
}
