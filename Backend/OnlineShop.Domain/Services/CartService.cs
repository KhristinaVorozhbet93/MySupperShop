using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Interfaces;

namespace OnlineShop.Domain.Services
{
    public class CartService
    {
        private readonly ICartRepozitory _cartRepozitory;
        private readonly IProductRepozitory _productRepozitory; 

        public CartService(ICartRepozitory cartRepozitory, IProductRepozitory productRepozitory)
        {
            _cartRepozitory = cartRepozitory
                ?? throw new ArgumentNullException(nameof(cartRepozitory));
            _productRepozitory = productRepozitory
                ?? throw new ArgumentNullException(nameof(productRepozitory));
        }

        public virtual async Task<Cart> GetAccountCart(Guid accountId,
            CancellationToken cancellationToken)
        {
            var cart = await _cartRepozitory.GetCartByAccountId(accountId, cancellationToken);

            if (cart is null)
            {
                throw new CartNotFoundException(nameof(cart));
            }
            return cart;
        }

        public virtual async Task AddProduct(Guid accountId, Guid productId, 
             CancellationToken cancellationToken, double quantity = 1d)
        {
            var product = await _productRepozitory.GetById(productId, cancellationToken);
            if (product is null)
            {
                throw new ProductNotFoundException(nameof(product)); 
            }
            var cart = await _cartRepozitory.GetCartByAccountId(accountId, cancellationToken);

            if (cart is null)
            {
                throw new CartNotFoundException(nameof(cart));
            }

            var existedItem = cart.Items.FirstOrDefault(item => item.ProductId == product.Id);

            if (existedItem is null)
            {
                cart.Items.Add(new CartItem(Guid.Empty, product.Id, quantity));
            }
            else
            {
                existedItem.Quantity += quantity;
            }
            await _cartRepozitory.Update(cart, cancellationToken);
        }
    }
}
