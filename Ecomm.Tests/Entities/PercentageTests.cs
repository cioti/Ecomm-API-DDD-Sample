using Ecomm.Domain.ValueObjects;
using FluentAssertions;
using System;
using Xunit;

namespace Ecomm.Tests.Entities
{
    public class PercentageTests
    {
        [Fact]
        public void Constructor_WithNegativeValue_ThrowsException()
        {
            //Arrange
            Action action = () => new Percentage(-1);

            //Act + Assert
            action.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void Constructor_WithValueGreaterThan100_ThrowsException()
        {
            //Arrange
            Action action = () => new Percentage(100.3414d);

            //Act + Assert
            action.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void Constructor_WithValidValue_ReturnsEntity()
        {
            //Arrange
            var percentage = new Percentage(23.75d);

            //Assert
            percentage.Value.Should().Be(23.75d);
        }

        [Fact]
        public void Equals_WithSameObject_ReturnsTrue()
        {
            //Arrange
            var percentage = new Percentage(23.75d);
            var percentageDuplicate = new Percentage(23.75d);

            //Act
            bool isEqual = percentage.Equals(percentageDuplicate);

            //Assert
            isEqual.Should().Be(true);
        }

        [Fact]
        public void Equals_WithDifferentObject_ReturnsFalse()
        {
            //Arrange
            var percentage = new Percentage(23.75d);
            var percentageDuplciate = new Percentage(23.76d);

            //Act
            bool isEqual = percentage.Equals(percentageDuplciate);

            //Assert
            isEqual.Should().Be(false);
        }
    }
}
