using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Domain.Services;
using OnlineShop.HttpModels.Requests;
using OnlineShop.HttpModels.Responses;
using OnlineShop.WebApi.Filtres;

namespace OnlineShop.WebApi.Controllers
{
    [CentralizedExceptionHandlingFilter]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly CartService _cartService;
        public CartController(CartService cartService)
        {
            _cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));
        }
        [Authorize]
        [HttpPost("cart/add_product")]
        public async Task<ActionResult> AddProductInCart(CartRequest request,
             CancellationToken cancellationToken)
        {
            await _cartService.AddProduct
                (request.AccountId, request.Product.Id, cancellationToken, request.Quantity);
            return Ok();
        }
        [Authorize]
        [HttpGet("cart")]
        public async Task<ActionResult<CartResponse>> GetAccountCart(Guid accountId,
             CancellationToken cancellationToken)
        {
            var cart = await _cartService.GetAccountCart(accountId, cancellationToken);
            List<ProductCartResponse> products = new();
            foreach (var item in cart.Items)
            {
                products.Add(new ProductCartResponse(item.Product.Id, item.Product.Name,
                    item.Product.Price, item.Product.Image, item.Quantity));
            }
            return new CartResponse(products);
        }
    }
}
