using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Ecomm.Domain.Abstractions;
using Ecomm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ecomm.Infrastructure
{
    public class GenericAsyncRepository<TEntity> : IGenericAsyncRepository<TEntity>
        where TEntity : BaseEntity, IAggregateRoot
    {
        private readonly CartContext _dbContext;
        private readonly ISpecificationEvaluator _specificationEvaluator;
        public GenericAsyncRepository(CartContext dbContext) : this(dbContext, SpecificationEvaluator.Default) { }

        public GenericAsyncRepository(CartContext dbContext, ISpecificationEvaluator specificationEvaluator)
        {
            _dbContext = dbContext;
            _specificationEvaluator = specificationEvaluator;
        }

        public async Task<TEntity> FindAsync<TKey>(TKey id, CancellationToken cancellationToken = default)
        {
            return await SetContextEntity().FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<TEntity> GetBySpecAsync<Spec>(Spec specification, CancellationToken cancellationToken = default)
            where Spec : ISpecification<TEntity>, ISingleResultSpecification
        {
            return await ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<TResult> GetBySpecAsync<TResult>(ISpecification<TEntity, TResult> specification, CancellationToken cancellationToken = default)
        {
            return await ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<TEntity>> ListAsync(bool trackChanges = true, CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = SetContextEntity();
            if (!trackChanges)
            {
                query = query.AsNoTracking();
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await SetContextEntity().AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return entity;
        }

        public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            _dbContext.Update(entity);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            SetContextEntity().Remove(entity);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        private DbSet<TEntity> SetContextEntity()
        {
            return _dbContext.Set<TEntity>();
        }

        private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> specification, bool evaluateCriteriaOnly = false)
        {
            return _specificationEvaluator.GetQuery(SetContextEntity().AsQueryable(), specification, evaluateCriteriaOnly);
        }

        private IQueryable<TResult> ApplySpecification<TResult>(ISpecification<TEntity, TResult> specification)
        {
            if (specification is null) throw new ArgumentNullException("Specification is required");
            if (specification.Selector is null) throw new SelectorNotFoundException();

            return _specificationEvaluator.GetQuery(SetContextEntity().AsQueryable(), specification);
        }
    }
}