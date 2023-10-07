using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Interfaces;

namespace OnlineShop.Domain.Services
{
    public class CartService
    {
        private readonly IUnitOfWork _uow;

        public CartService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        public virtual async Task<Cart> GetAccountCart(Guid accountId,
            CancellationToken cancellationToken)
        {
            var cart = await _uow.CartRepozitory.GetCartByAccountId(accountId, cancellationToken);

            if (cart is null)
            {
                throw new CartNotFoundException(nameof(cart));
            }
            return cart;
        }

        public virtual async Task AddProduct(Guid accountId, Guid productId,
             CancellationToken cancellationToken, double quantity = 1d)
        {
            if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity));
            var product = await _uow.ProductRepozitory.GetById(productId, cancellationToken);
            if (product is null)
            {
                throw new ProductNotFoundException(nameof(product));
            }
            var cart = await _uow.CartRepozitory.GetCartByAccountId(accountId, cancellationToken);
            if (cart is null)
            {
                throw new CartNotFoundException(nameof(cart));
            }
            cart.AddItem(product, quantity);
            await _uow.CartRepozitory.Update(cart, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
        }
    }
}
