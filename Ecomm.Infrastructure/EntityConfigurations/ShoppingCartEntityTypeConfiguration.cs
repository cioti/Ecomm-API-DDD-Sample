using Ecomm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecomm.Infrastructure.EntityConfigurations
{
    internal class ShoppingCartEntityTypeConfiguration : IEntityTypeConfiguration<ShoppingCart>
    {
        public void Configure(EntityTypeBuilder<ShoppingCart> builder)
        {
            builder.ToTable("ShoppingCarts");
            builder.HasKey(sc => sc.Id);
            builder.Property(sc => sc.CustomerId).HasField("_customerId");
            builder.OwnsOne(sc => sc.Audit, OwnedEntityNavigationBuilders.AuditConfiguration<ShoppingCart>());
            builder.HasMany(sc => sc.Items).WithOne().HasForeignKey(ci => ci.CartId);
            builder.HasMany(sc => sc.Discounts).WithOne().HasForeignKey(cd => cd.CartId);
        }
    }
}
