using Ardalis.Specification;
using Ecomm.Domain.Entities;
using System;

namespace Ecomm.Domain.Specifications
{
    public class ShoppingCartWithItemsAndDiscountsSpec : Specification<ShoppingCart>, ISingleResultSpecification
    {
        public ShoppingCartWithItemsAndDiscountsSpec(Guid id)
        {
            Query
                .Include(sc => sc.Discounts)
                .Include(sc => sc.Items)
                    .ThenInclude(ci => ci.Discounts)
                .Where(sc => sc.Id == id);
        }
    }
}
