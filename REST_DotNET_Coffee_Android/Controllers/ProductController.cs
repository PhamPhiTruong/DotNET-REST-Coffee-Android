using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace REST_DotNET_Coffee_Android.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class ProductController : ControllerBase
    {

        private readonly IProductService _productService;

        // Assign Service
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        //// GET: all products
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProduct()
        {
            return Ok(await _productService.GetAllProduct());
        }

        // GET: a single product
        [HttpGet("id/{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            return Ok(await _productService.GetProductById(id));
        }

        // GET: product with type
        [HttpGet("type/")]
        public async Task<ActionResult<List<Product>>> GetProduct(EProductType type)
        {
            return Ok(await _productService.GetProductWithType(type));
        }

        // GET: product with category
        [HttpGet("category/{category}")]
        public async Task<ActionResult<Product>> GetProduct(string category)
        {
            return Ok(await _productService.GetProductByCategory(category));
        }

        // POST: add a product
        [HttpPost]
        public async Task<ActionResult<List<Product>>> AddProduct([FromBody] ProductRequestDTO request)
        {
            return Ok(await _productService.AddProduct(request));
        }

        // PUT: modify a product
        [HttpPut]
        public async Task<ActionResult<List<Product>>> UpdateProduct(ProductRequestDTO request)
        {
            return Ok(await _productService.UpdateProduct(request));

        }

        // DELETE: a product
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Product>>> DeleteProduct(int id)
        {
            return Ok(await _productService.DeleteProduct(id));
        }
    }
}
