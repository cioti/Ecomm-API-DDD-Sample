using Ecomm.Application.Common.DTOs;
using System.Threading.Tasks;

namespace Ecomm.Application.Common.Interfaces
{
    public interface ICustomerProxy
    {
        Task<CustomerDto> GetCustomerWithDiscountsAsync(string userId);
    }
}
