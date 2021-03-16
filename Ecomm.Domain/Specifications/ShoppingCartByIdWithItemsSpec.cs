using Ardalis.Specification;
using Ecomm.Domain.Entities;
using System;
using System.Linq;

namespace Ecomm.Domain.Specifications
{
    public class ShoppingCartByIdWithItemsSpec :Specification<ShoppingCart>, ISingleResultSpecification
    {
        public ShoppingCartByIdWithItemsSpec(Guid id)
        {
            Query
                .Include(sc=>sc.Items)
                .Where(sc=>sc.Id == id);
        }
    }
}
