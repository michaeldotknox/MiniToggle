using System;
using System.Data.SqlTypes;
using System.Threading.Tasks;
using FluentAssertions;
using MiniToggle.Core;
using MiniToggle.UnitTests.Toggles;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace MiniToggle.UnitTests
{
    [TestFixture]
    public class ToggleTests
    {
        private Fixture _fixture;

        [OneTimeSetUp]
        public void Setup()
        {
            _fixture = new Fixture();
        }

        [Test]
        public void ExecuteReturnsEnabledResultIfToggleIsEnabled()
        {
            // Arrange
            var trueResult = _fixture.Create<TestResult>();
            var falseResult = _fixture.Create<TestResult>();

            // Act
            var result = Toggle<AlwaysTrueToggle>.Execute(() => trueResult, () => falseResult);

            // Assert
            result.Should().Be(trueResult);
        }

        [Test]
        public void ExecuteReturnsDisabledResultIfToggleIsDisabled()
        {
            // Arrange
            var trueResult = _fixture.Create<TestResult>();
            var falseResult = _fixture.Create<TestResult>();

            // Act
            var result = Toggle<AlwaysFalseToggle>.Execute(() => trueResult, () => falseResult);

            // Assert
            result.Should().Be(falseResult);
        }

        [Test]
        public void ExecuteUsesEnabledDelegateIfToggleIsEnabled()
        {
            // Arrange
            var trueExecuted = false;
            var falseExecuted = false;

            // Act
            Toggle<AlwaysTrueToggle>.Execute(() => { trueExecuted = true; }, () => { falseExecuted = true; });

            // Assert
            trueExecuted.Should().BeTrue();
            falseExecuted.Should().BeFalse();
        }

        [Test]
        public void ExecuteUsesDisabledDelegateIfToggleIsDisabled()
        {
            // Arrange
            var trueExecuted = false;
            var falseExecuted = false;

            // Act
            Toggle<AlwaysFalseToggle>.Execute(() => { trueExecuted = true; }, () => { falseExecuted = true; });

            // Assert
            trueExecuted.Should().BeFalse();
            falseExecuted.Should().BeTrue();
        }

        [Test]
        public async Task ExecuteAsyncReturnsEnabledResultIfToggleIsEnabled()
        {
            // Arrange
            var trueResult = _fixture.Create<TestResult>();
            var falseResult = _fixture.Create<TestResult>();

            // Act
            var result = await Toggle<AlwaysTrueToggle>.ExecuteAsync(() => Task.FromResult(trueResult), () => Task.FromResult(falseResult));

            // Assert
            result.Should().Be(trueResult);
        }

        [Test]
        public async Task ExecuteAsyncReturnsDisabledResultIfToggleIsDisabled()
        {
            // Arrange
            var trueResult = _fixture.Create<TestResult>();
            var falseResult = _fixture.Create<TestResult>();

            // Act
            var result = await Toggle<AlwaysFalseToggle>.ExecuteAsync(() => Task.FromResult(trueResult), () => Task.FromResult(falseResult));

            // Assert
            result.Should().Be(falseResult);
        }

        [Test]
        public async Task ExecuteAsyncUsesEnabledDelegateIfToggleIsEnabled()
        {
            // Arrange
            var trueExecuted = false;
            var falseExecuted = false;

            Func<Task> trueBranch = async () =>
            {
                trueExecuted = true;
            };
            Func<Task> falseBranch = async () =>
            {
                falseExecuted = true;
            };

            // Act
            await Toggle<AlwaysTrueToggle>.ExecuteAsync(trueBranch, falseBranch);

            // Assert
            trueExecuted.Should().BeTrue();
            falseExecuted.Should().BeFalse();
        }

        [Test]
        public async Task ExecuteAsyncUsesDisabledDelegateIfToggleIsDisabled()
        {
            // Arrange
            var trueExecuted = false;
            var falseExecuted = false;

            Func<Task> trueBranch = async () =>
            {
                trueExecuted = true;
            };
            Func<Task> falseBranch = async () =>
            {
                falseExecuted = true;
            };

                // Act
            await Toggle<AlwaysFalseToggle>.ExecuteAsync(trueBranch, falseBranch);

            // Assert
            trueExecuted.Should().BeFalse();
            falseExecuted.Should().BeTrue();
        }
    }
}
