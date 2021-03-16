using Ecomm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecomm.Infrastructure.EntityConfigurations
{
    internal class CartItemEntityTypeConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.ToTable("CartItems");
            builder.HasKey(ci => ci.Id);
            builder.Property(ci => ci.Description).HasMaxLength(255);
            builder.Property(ci => ci.Quantity).IsRequired();
            builder.OwnsOne(ci => ci.UnitPrice, OwnedEntityNavigationBuilders.MoneyConfiguration<CartItem>());
            builder.HasMany(ci => ci.Discounts).WithOne().HasForeignKey(cid => cid.CartItemId);
        }
    }
}
