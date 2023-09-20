using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Interfaces;

namespace OnlineShop.Data.EntityFramework.Data
{
    public class EfRepozitory<TEntity> : IRepozitory<TEntity> where TEntity : class, IEntity
    {
        protected readonly AppDbContext _dbContext;

        public EfRepozitory(AppDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentException(nameof(dbContext));
        }
        protected DbSet<TEntity> _entities => _dbContext.Set<TEntity>();

        public virtual async Task Add(TEntity entity, CancellationToken cancellationToken)
        {
            await _entities.AddAsync(entity, cancellationToken);
        }
        public virtual async Task<TEntity> GetById(Guid id, CancellationToken cancellationToken)
        {
            var entity = await _entities.SingleAsync(it => it.Id == id, cancellationToken);
            return entity;
        }
        public virtual async Task<List<TEntity>> GetAll(CancellationToken cancellationToken)
        {
            return await _entities.ToListAsync(cancellationToken);
        }
        public virtual async Task Update(TEntity entity,
            CancellationToken cancellationToken)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }
        public virtual async Task Delete(TEntity entity, CancellationToken cancellationToken)
        {
            _entities.Remove(entity);
        }
    }
}
