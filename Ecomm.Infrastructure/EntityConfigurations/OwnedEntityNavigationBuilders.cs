using Ecomm.Domain.Entities;
using Ecomm.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Ecomm.Infrastructure.EntityConfigurations
{
    internal class OwnedEntityNavigationBuilders
    {
        internal static Action<OwnedNavigationBuilder<T, Audit>> AuditConfiguration<T>()
            where T : BaseEntity
        {
            return builder =>
            {
                builder.Property(a => a.CreatedBy).HasColumnName(nameof(Audit.CreatedBy)).IsRequired();
                builder.Property(a => a.CreatedDate).HasColumnName(nameof(Audit.CreatedDate)).IsRequired();
                builder.Property(a => a.ModifiedBy).HasColumnName(nameof(Audit.ModifiedBy));
                builder.Property(a => a.ModifiedDate).HasColumnName(nameof(Audit.ModifiedDate));
            };
        }

        internal static Action<OwnedNavigationBuilder<T, Money>> MoneyConfiguration<T>(string valueColumnName = "", string currencyColumnName = "")
            where T : BaseEntity
        {
            return builder =>
            {
                builder
                .Property(m => m.Value)
                .HasColumnName(string.IsNullOrWhiteSpace(valueColumnName) ? nameof(Money) : valueColumnName);
                builder
                .Property(m => m.Currency)
                .HasColumnName(string.IsNullOrWhiteSpace(currencyColumnName) ? nameof(Money.Currency) : currencyColumnName);
            };
        }

        internal static Action<OwnedNavigationBuilder<T, Amount>> AmountConfiguration<T>()
            where T : BaseEntity
        {
            return builder =>
            {
                builder.Property(a => a.Value).HasColumnName(nameof(Amount));
                builder.Property(a => a.Unit).HasColumnName(nameof(Amount.Unit));
            };
        }

        internal static Action<OwnedNavigationBuilder<T, Percentage>> PercentageConfiguration<T>()
        where T : BaseEntity
        {
            return builder =>
            {
                builder.Property(a => a.Value).HasColumnName(nameof(Percentage));
            };
        }

        internal static Action<OwnedNavigationBuilder<T, CartItemDiscount>> DiscountConfiguration<T>()
            where T : BaseEntity
        {
            return builder =>
            {
                builder.HasKey("Id");
                builder.Property("Id").ValueGeneratedOnAdd();
                builder.WithOwner().HasForeignKey("OwnerId");
                builder.Property(pd => pd.Percentage).HasColumnName(nameof(Discount) + nameof(Discount.Percentage));
            };
        }
    }
}
