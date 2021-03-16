using Ecomm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecomm.Infrastructure.EntityConfigurations
{
    public class CartItemDiscountEntityTypeConfiguration : IEntityTypeConfiguration<CartItemDiscount>
    {
        public void Configure(EntityTypeBuilder<CartItemDiscount> builder)
        {
            builder.ToTable("CartItemDiscounts");
            builder.HasKey(cid => cid.Id);
            builder.Property(cid => cid.Code).IsRequired();
            builder.OwnsOne(cid => cid.Percentage, OwnedEntityNavigationBuilders.PercentageConfiguration<CartItemDiscount>());
        }
    }
}
