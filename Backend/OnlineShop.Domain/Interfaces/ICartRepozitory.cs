using OnlineShop.Domain.Entities;

namespace OnlineShop.Domain.Interfaces
{
    public interface ICartRepozitory : IRepozitory<Cart>
    {
        Task<Cart> GetCartByAccountId(Guid id, CancellationToken cancellationToken);
    }
}
