using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Services;
using OnlineShop.HttpModels.Requests;
using OnlineShop.HttpModels.Responses;

namespace OnlineShop.WebApi.Controllers
{
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly CatalogService _catalogService;

        public CatalogController(CatalogService catalogService)
        {
            _catalogService = catalogService ?? throw new ArgumentException(nameof(catalogService));
        }
        [Authorize]
        [HttpGet("get_product")]
        public async Task<ActionResult<ProductResponse>> GetProductById
           (Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var product = await _catalogService.GetProduct(id, cancellationToken);
                return Ok(new ProductResponse(product.Id, product.Name, product.Description,
                    product.Price, product.ProducedAt, product.ExpiredAt, product.Image));
            }
            catch (ProductNotFoundException)
            {
                return NotFound("Продукт с таким id не найден!");
            }
        }

        [Authorize]
        [HttpGet("get_products")]
        public async Task<ActionResult<List<ProductResponse>>> GetAllProducts
            (CancellationToken cancellationToken)
        {
            try
            {
                var products = await _catalogService.GetProducts(cancellationToken);
                List<ProductResponse> productsResponse = new();
                foreach (var product in products)
                {
                    productsResponse.Add(new ProductResponse(product.Id, product.Name, product.Description,
                    product.Price, product.ProducedAt, product.ExpiredAt, product.Image));
                }
                return Ok(productsResponse);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }

        [Authorize(Roles = nameof(Role.Admin))]
        [HttpPost("add_product")]
        public async Task<ActionResult> AddProduct(ProductRequest request,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            await _catalogService.AddProduct(request.Name, request.Description, request.Price,
                request.ProducedAt, request.ExpiredAt, request.Image, cancellationToken);
            return Ok();
        }

        [Authorize(Roles = nameof(Role.Admin))]
        [HttpPost("delete_product")]
        public async Task<ActionResult> DeleteProduct([FromBody]Guid id,
       CancellationToken cancellationToken)
        {
            try
            {
                await _catalogService.DeleteProduct(id, cancellationToken);
                return Ok();
            }
            catch (ProductNotFoundException)
            {
                return Conflict (new ErrorResponse("Продукт с таким id не найден!"));
            }
        }

        [Authorize(Roles = nameof(Role.Admin))]
        [HttpPost("update_product")]
        public async Task<ActionResult> UpdateProduct(
            ProductRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            await _catalogService.UpdateProduct(request.Id, request.Name,request.Description,
                request.Price, request.ProducedAt, request.ExpiredAt, 
                request.Image, cancellationToken);
            return Ok();
        }
    }
}
