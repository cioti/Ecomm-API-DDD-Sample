using Ecomm.Application.Common.DTOs;
using System.Threading;
using System.Threading.Tasks;

namespace Ecomm.Application.Common.Interfaces
{
    public interface IProductProxy
    {
        Task<ProductDto> GetProductWithDiscountsAsync(int productId, CancellationToken cancellationToken);
    }
}
