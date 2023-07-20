using Microsoft.AspNetCore.Mvc;
using MyShopBackend.Interfaces;
using MyShopBackend.Models;

namespace MyShopBackend.Controllers
{
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IRepozitory<Product> _repozitory; 

        public CatalogController(IRepozitory<Product> repozitory)
        {
            _repozitory = repozitory ?? throw new ArgumentException(nameof(repozitory));
        }
        [HttpPost("add_product")]
        public async Task<IResult> AddProduct(Product product, CancellationToken cancellationToken)
        {
            try
            {
                await _repozitory.Add(product, cancellationToken);
                return Results.Ok();
            }
            catch (InvalidOperationException)
            {
                return Results.NotFound();
            }
        }
        [HttpGet("get_product")]
        public async Task<IResult> GetProductById(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var product = await _repozitory.GetById(id, cancellationToken);
                return Results.Ok(product);
            }
            catch (InvalidOperationException)
            {
                return Results.NotFound();
            }
        }
        [HttpGet("get_products")]
        public async Task<IResult> GetAllProducts(CancellationToken cancellationToken)
        {
            try
            {
                var products = await _repozitory.GetAll(cancellationToken);
                return Results.Ok(products);
            }
            catch (ArgumentNullException)
            {
                return Results.NotFound();
            }
        }
        [HttpPost("update_product")]
        public async Task UpdateProduct(Product product, CancellationToken cancellationToken)
        {
            await _repozitory.Update(product, cancellationToken);
        }
        [HttpPost("delete_product")]
        public async Task DeleteProduct([FromBody] Product product, CancellationToken cancellationToken)
        {
            await _repozitory.Delete(product, cancellationToken);
        }
    }
}
