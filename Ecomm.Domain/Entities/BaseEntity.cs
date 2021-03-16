
namespace Ecomm.Domain.Entities
{
    public class BaseEntity<TKey> : BaseEntity
    {
        public TKey Id { get; protected set; }
    }

    /// <summary>
    /// Marker class for database entities
    /// </summary>
    public class BaseEntity { }
}
