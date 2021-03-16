using Ardalis.Specification;
using Ecomm.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ecomm.Domain.Abstractions
{
    public interface IGenericAsyncRepository<TEntity> where TEntity : BaseEntity, IAggregateRoot
    {
        Task<TEntity> FindAsync<TKey>(TKey id, CancellationToken cancellationToken = default);
        Task<TEntity> GetBySpecAsync<Spec>(Spec specification, CancellationToken cancellationToken = default)
            where Spec : ISpecification<TEntity>, ISingleResultSpecification;
        Task<TResult> GetBySpecAsync<TResult>(ISpecification<TEntity, TResult> specification, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<TEntity>> ListAsync(bool trackChanges = true, CancellationToken cancellationToken = default);
        Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
    }
}
