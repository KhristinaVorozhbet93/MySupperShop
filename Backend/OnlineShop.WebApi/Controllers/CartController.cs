using Microsoft.AspNetCore.Mvc;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Services;
using OnlineShop.HttpModels.Responses;

namespace OnlineShop.WebApi.Controllers
{
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly CartService _cartService;
        public CartController(CartService cartService)
        {
            _cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));
        }

        [HttpPost("cart/add_product")]
        public async Task<ActionResult> AddProductInCart(Guid accountId, Guid productId,
             CancellationToken cancellationToken, double quantity)
        {
            try
            {
                await _cartService.AddProduct(accountId, productId, cancellationToken, quantity);
                return Ok();
            }
            catch (ProductNotFoundException)
            {
                return Conflict(new ErrorResponse("Не найден продукт с данным id!"));
            }
            catch (CartNotFoundException)
            {
                return Conflict(new ErrorResponse("Корзины по таком id не сущестует!")); 
            }
        }

        [HttpGet("cart")]
        public async Task<ActionResult> GetAccountCart(Guid accountId,
             CancellationToken cancellationToken)
        {
            try
            {
                await _cartService.GetAccountCart(accountId, cancellationToken);
                return Ok();
            }
            catch (CartNotFoundException)
            {
                return Conflict(new ErrorResponse("Корзины по таком id не сущестует!")); 
            }
        }
    }
}
