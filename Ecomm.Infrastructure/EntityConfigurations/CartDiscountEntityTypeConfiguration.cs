using Ecomm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecomm.Infrastructure.EntityConfigurations
{
    public class CartDiscountEntityTypeConfiguration : IEntityTypeConfiguration<CartDiscount>
    {
        public void Configure(EntityTypeBuilder<CartDiscount> builder)
        {
            builder.ToTable("CartDiscounts");
            builder.HasKey(cd => cd.Id);
            builder.Property(cd => cd.Code).IsRequired();
            builder.OwnsOne(cd => cd.Percentage, OwnedEntityNavigationBuilders.PercentageConfiguration<CartDiscount>());
        }
    }
}
