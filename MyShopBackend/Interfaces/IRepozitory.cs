﻿namespace MyShopBackend.Interfaces
{
    public interface IRepozitory<TEntity>
    {
        Task Add(TEntity entity, CancellationToken cancellationToken);
        Task<TEntity> GetById(Guid id, CancellationToken cancellationToken);
        Task<List<TEntity>> GetAll(CancellationToken cancellationToken);
        Task Update(TEntity entity, CancellationToken cancellationToken);
        Task Delete(TEntity entity, CancellationToken cancellationToken);
    }
}
