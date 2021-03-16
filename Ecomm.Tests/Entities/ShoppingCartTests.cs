using Ecomm.Domain.Common.Enums;
using Ecomm.Domain.Entities;
using Ecomm.Domain.ValueObjects;
using Ecomm.Shared.Exceptions;
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
    public class ShoppingCartTests
    {
        private readonly CartItem _cartItem;
        public ShoppingCartTests()
        {
            _cartItem = new CartItem(1,
                "test",
                1,
                new Money(Currency.EUR, 1),
                null,
                new List<CartItemDiscount> { new CartItemDiscount("code", 10) });
        }

        [Fact]
        public void AddItem_ItemDoesNotExist_ItemIsAddedToTheList()
        {
            //Arrange
            var shoppingCart = new ShoppingCart(It.IsAny<Guid>());

            //Act
            shoppingCart.AddItem(1, "test", 1, 1, Currency.EUR, It.IsAny<byte[]>(), null);

            //Assert
            shoppingCart.Items.Should().ContainSingle()
                .And.Contain(x => x.ProductId == 1);
        }

        [Fact]
        public void AddItem_ItemAlreadyExists_ShouldIncrementItemQuantity()
        {
            //Arrange
            var shoppingCart = new ShoppingCart(It.IsAny<Guid>());

            //Act
            shoppingCart.AddItem(1, "test", 1, 1, Currency.EUR, It.IsAny<byte[]>(), null);
            shoppingCart.AddItem(1, "test", 1, 1, Currency.EUR, It.IsAny<byte[]>(), null);


            //Assert
            shoppingCart.Items.Should().ContainSingle()
                .And.Contain(x => x.Quantity == 2);
        }

        [Fact]
        public void RemoveItem_ItemNotFound_ThrowsException()
        {
            //Arrange
            var shoppingCart = new ShoppingCart(It.IsAny<Guid>());

            Action act = () => shoppingCart.RemoveItem(1);

            //Act + Assert
            act.Should().ThrowExactly<NotFoundException>();
        }

        [Fact]
        public void RemoveItem_ItemFound_ItemIsRemovedFromList()
        {
            //Arrange
            var shoppingCart = new ShoppingCart(It.IsAny<Guid>());
            shoppingCart.AddItem(1, "test", 1, 1, Currency.EUR, It.IsAny<byte[]>(), null);

            //Act
            shoppingCart.RemoveItem(1);

            //Act + Assert
            shoppingCart.Items.Should().BeEmpty();
        }

        [Fact]
        public void AddDiscount_NullDiscounts_ThrowsException()
        {
            //Arrange
            var shoppingCart = new ShoppingCart(It.IsAny<Guid>());

            Action act = () => shoppingCart.AddDiscounts(null);

            //Act + Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void AddDiscount_DoesNotExist_DiscountAddedToTheList()
        {
            //Arrange
            var shoppingCart = new ShoppingCart(It.IsAny<Guid>());
            var discount = new CartDiscount("test", 5);
            var discounts = new List<CartDiscount> { discount };

            //Act
            shoppingCart.AddDiscounts(discounts);

            //Assert
            shoppingCart.Discounts
                .Should().Contain(d => d.Code == discount.Code &&
                d.Percentage == discount.Percentage);
        }

        [Fact]
        public void AddDiscount_DiscountExists_DiscountPercentageUpdated()
        {
            //Arrange
            var shoppingCart = new ShoppingCart(It.IsAny<Guid>());
            var discount = new CartDiscount("test", 5);
            var discounts = new List<CartDiscount> { discount };
            var discountUpdate = new CartDiscount("test", 30);
            var discountsUpdate = new List<CartDiscount> { discountUpdate };

            //Act
            shoppingCart.AddDiscounts(discounts);
            shoppingCart.AddDiscounts(discountsUpdate);

            //Assert
            shoppingCart.Discounts.Should().HaveCount(1)
                .And.Contain(d => d.Code == discountUpdate.Code &&
                d.Percentage == discountUpdate.Percentage);
        }


    }
}
