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
        public async Task<List<ProductRespondeDTO>> GetProduct()
        {
            return await _productService.GetAllProduct();
        }

        // GET: a single product
        [HttpGet("id/{id}")]
        public async Task<ProductRespondeDTO> GetProduct(int id)
        {
            return await _productService.GetProductById(id);
        }

        // GET: product with type
        [HttpGet("type/")]
        public async Task<List<ProductRespondeDTO>> GetProduct(EProductType type)
        {
            return await _productService.GetProductWithType(type);
        }

        // GET: product with category
        [HttpGet("category/{category}")]
        public async Task<List<ProductRespondeDTO>> GetProduct(string category)
        {
            return await _productService.GetProductByCategory(category);
        }

        // POST: add a product
        [HttpPost]
        public async Task<List<ProductRespondeDTO>> AddProduct([FromBody] ProductRequestDTO request)
        {
            return await _productService.AddProduct(request);
        }

        // PUT: modify a product
        [HttpPut]
        public async Task<List<ProductRespondeDTO>> UpdateProduct(ProductRequestDTO request)
        {
            return await _productService.UpdateProduct(request);

        }

        // DELETE: a product
        [HttpDelete("{id}")]
        public async Task<List<ProductRespondeDTO>> DeleteProduct(int id)
        {
            return await _productService.DeleteProduct(id);
        }
    }
}
