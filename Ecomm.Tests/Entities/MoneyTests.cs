using Ecomm.Domain.Common.Enums;
using Ecomm.Domain.ValueObjects;
using FluentAssertions;
using Moq;
using System;
using Xunit;

namespace Ecomm.Tests.Entities
{
    public class MoneyTests
    {
        [Fact]
        public void Constructor_WithNegativeValue_ThrowsException()
        {
            //Arrange
            Action action = () => new Money(It.IsAny<Currency>(), decimal.MinusOne);

            //Act + Assert
            action.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void Constructor_WithZeroValue_ReturnsEntity()
        {
            //Arrange
            var money = new Money(It.IsAny<Currency>(), decimal.Zero);

            //Assert
            money.Value.Should().Be(decimal.Zero);
        }

        [Fact]
        public void Constructor_WithMaximumValue_ReturnsEntity()
        {
            //Arrange
            var money = new Money(It.IsAny<Currency>(), decimal.MaxValue);

            //Assert
            money.Value.Should().Be(decimal.MaxValue);
        }

        [Theory]
        [InlineData(Currency.EUR, 10.4)]
        [InlineData(Currency.RON, 9)]
        [InlineData(Currency.USD, 1.432534)]
        public void Equals_WithSameObject_ReturnsTrue(Currency currency, decimal value)
        {
            //Arrange
            var money = new Money(currency, value);
            var moneyDuplicate = new Money(currency, value);
           
            //Act
            bool isEqual = money.Equals(moneyDuplicate);

            //Assert
            isEqual.Should().Be(true);
        }

        [Fact]
        public void Equals_WithDifferentObject_ReturnsFalse()
        {
            //Arrange
            var money = new Money(Currency.USD, 10.55m);
            var moneyDuplicate = new Money(Currency.EUR, 10.55m);

            //Act
            bool isEqual = money.Equals(moneyDuplicate);

            //Assert
            isEqual.Should().Be(false);
        }
    }
}
