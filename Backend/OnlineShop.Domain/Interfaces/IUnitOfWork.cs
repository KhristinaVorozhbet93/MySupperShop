﻿namespace OnlineShop.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IAccountRepozitory AccountRepozitory { get; }
        ICartRepozitory CartRepozitory { get; }
        IProductRepozitory ProductRepozitory { get; }   
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
