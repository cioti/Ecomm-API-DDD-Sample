using Ecomm.Domain.Abstractions;
using Ecomm.Domain.Entities;
using Ecomm.Infrastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ecomm.Infrastructure
{
    public class CartContext : DbContext
    {
        private readonly ICurrentUserService _currentUserService;

        public CartContext(DbContextOptions<CartContext> options, ICurrentUserService currentUserService = null) : base(options)
        {
            _currentUserService = currentUserService;
        }

        public DbSet<ShoppingCart> Carts { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<IAuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Audit.WasCreatedBy(_currentUserService?.UserId ?? "SYSTEM", DateTime.UtcNow);
                        break;
                    case EntityState.Modified:
                        entry.Entity.Audit.WasModifiedBy(_currentUserService?.UserId ?? "SYSTEM", DateTime.UtcNow);
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ShoppingCartEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CartItemEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CartDiscountEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CartItemDiscountEntityTypeConfiguration());
        }
    }
}
