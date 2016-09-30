using FluentAssertions;
using MiniToggle.Core.Attributes;
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
            const string toggleName = "TestToggle";

            // Act
            var result = new SettingConfigurationAttribute(toggleName);

            // Assert
            result.SettingName.Should().Be(toggleName);
        }
    }
}
