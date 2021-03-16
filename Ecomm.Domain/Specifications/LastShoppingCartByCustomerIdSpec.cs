using Ardalis.Specification;
using Ecomm.Domain.Entities;
using System;

namespace Ecomm.Domain.Specifications
{
    public class LastShoppingCartByCustomerIdSpec : Specification<ShoppingCart>, ISingleResultSpecification
    {
        public LastShoppingCartByCustomerIdSpec(Guid customerId)
        {
            Query.Where(sc => sc.CustomerId == customerId)
                .OrderByDescending(sc => sc.Audit.CreatedDate);
        }
    }
}
