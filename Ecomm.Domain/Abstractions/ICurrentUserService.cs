
namespace Ecomm.Domain.Abstractions
{
    public interface ICurrentUserService
    {
        string Username { get; }
        string UserId { get; }
    }
}
