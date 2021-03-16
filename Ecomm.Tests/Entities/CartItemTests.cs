using Ecomm.Domain.Common.Enums;
using Ecomm.Domain.Entities;
using Ecomm.Domain.ValueObjects;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ecomm.Tests.Entities
{
    public class CartItemTests
    {

        [Fact]
        public void Controller_NegativeProductId_ThrowsException()
        {
            //Act + assert
            Action act = () => GetCartItem(-1);
            act.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void Controller_ZeroValueProductId_ThrowsException()
        {
            //Act + assert
            Action act = () => GetCartItem(0);
            act.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void Controller_NegativeQuantity_ThrowsException()
        {
            //Act + assert
            Action act = () => GetCartItem(-1);
            act.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void Controller_ZeroValueQuantity_ThrowsException()
        {
            //Act + assert
            Action act = () => GetCartItem(0);
            act.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void Controller_EmptyDescription_ThrowsException()
        {
            //Act + assert
            Action act = () => GetCartItem(description: "");
            act.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void Controller_WhitespaceDescription_ThrowsException()
        {
            //Act + assert
            Action act = () => GetCartItem(description: "  ");
            act.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void Controller_NullPrice_ThrowsException()
        {
            //Act + assert
            Action act = () => GetCartItem(1, "test", 1, null);
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Controller_NullDiscounts_ThenDiscountsIsEmptyList()
        {
            //Arrange + act
            CartItem cartItem = GetCartItem(1, "test", 1, new Money(Currency.EUR, 1), null, null);

            //Assert
            cartItem.Discounts.Should().NotBeNull();
            cartItem.Discounts.Should().BeEmpty();
        }

        [Fact]
        public void IncreaseQuantity_NegativeQuantity_ThrowsException()
        {
            //Arrange
            var cartItem = GetCartItem(1, "test", 1, new Money(Currency.EUR, 1));

            //Act + assert
            Action act = () => cartItem.IncreaseQuantity(-1);
            act.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void IncreaseQuantity_ZeroValueQuantity_ThrowsException()
        {
            //Arrange
            var cartItem = GetCartItem(1, "test", 1, new Money(Currency.EUR, 1));

            //Act + assert
            Action act = () => cartItem.IncreaseQuantity(0);
            act.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void CalculateTotalPriceWithDiscount_NoDiscountsAvailable_ReturnsTotalPrice()
        {
            //Arrange
            var cartItem = GetCartItem(1, "test", 1, new Money(Currency.EUR, 1));

            //Act
            var totalPriceWithDiscount = cartItem.CalculateTotalPriceWithDiscount();

            //Assert
            totalPriceWithDiscount.Should().Be(cartItem.TotalPrice);
        }

        [Fact]
        public void CalculateTotalPriceWithDiscount_MaxDiscountIsZero_ReturnsTotalPrice()
        {
            //Arrange
            var maxDiscount = new CartItemDiscount("test", 0);
            var discounts = new List<CartItemDiscount>
             {
                 maxDiscount
             };
            var cartItem = GetCartItem(1, "test", 1, new Money(Currency.EUR, 1), null, discounts);

            //Act
            var totalPriceWithDiscount = cartItem.CalculateTotalPriceWithDiscount();

            //Assert
            var expectedValue = cartItem.TotalPrice - cartItem.TotalPrice * (decimal)maxDiscount.Percentage.Value / 100;
            totalPriceWithDiscount.Should().Be(expectedValue);
        }

        [Fact]
        public void CalculateTotalPriceWithDiscount_DiscountsAvailable_ReturnsTotalPriceWithDiscount()
        {
            //Arrange
            var maxDiscount = new CartItemDiscount("test", 30);
            var discounts = new List<CartItemDiscount>
             {
                 new CartItemDiscount("test",20),
                 maxDiscount
             };
            var quantity = 1;
            var unitPrice = new Money(Currency.EUR, 1);
            var cartItem = GetCartItem(1, "test", quantity, unitPrice, null, discounts);

            //Act
            var totalPriceWithDiscount = cartItem.CalculateTotalPriceWithDiscount();

            //Assert
            var expectedValue = cartItem.TotalPrice - cartItem.TotalPrice * (decimal)maxDiscount.Percentage.Value / 100;
            totalPriceWithDiscount.Should().Be(expectedValue);
        }


        private CartItem GetCartItem(int productId = default, string description = "",
            int quantity = default, Money price = null, byte[] image = null,
            List<CartItemDiscount> discounts = null)
        {
            return new CartItem(productId, description, quantity, price, image, discounts);
        }
    }

}
