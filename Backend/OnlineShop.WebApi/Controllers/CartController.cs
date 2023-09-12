using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Services;
using OnlineShop.HttpModels.Requests;
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
        [Authorize]
        [HttpPost("cart/add_product")]
        public async Task<ActionResult> AddProductInCart(CartRequest request, 
             CancellationToken cancellationToken)
        {
            try
            {
                await _cartService.AddProduct
                    (request.AccountId, request.Product.Id, cancellationToken, request.Quantity);
                return Ok();
            }
            catch (ProductNotFoundException)
            {
                return Conflict(new ErrorResponse("Не найден продукт с данным id!"));
            }
            catch (CartNotFoundException)
            {
                return Conflict(new ErrorResponse("Корзины по такому id не сущестует!")); 
            }
        }
        [Authorize]
        [HttpGet("cart")]
        public async Task<ActionResult<CartResponse>> GetAccountCart(Guid accountId,
             CancellationToken cancellationToken)
        { 
            try
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
            catch (CartNotFoundException)
            {
                return Conflict(new ErrorResponse("Корзины по таком id не сущестует!")); 
            }
        }
    }
}
