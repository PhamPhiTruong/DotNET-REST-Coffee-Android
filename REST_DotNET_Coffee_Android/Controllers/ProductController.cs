using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace REST_DotNET_Coffee_Android.Controllers
{
    [ApiController]
    [Route("product/")]
    public class ProductController : ControllerBase
    {

        private readonly IProductService _productService;

        // Assign Service
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        //// GET: all products
        [HttpGet("getAllProduct/")]
        public async Task<List<ProductRespondeDTO>> GetProduct()
        {
            return await _productService.GetAllProduct();
        }

        // GET: a single product
        [HttpGet("getProductById/")]
        public async Task<ProductRespondeDTO> GetProduct( int id)
        {
            return await _productService.GetProductById(id);
        }

        // GET: product with type
        [HttpGet("getProductByType/")]
        public async Task<List<ProductRespondeDTO>> GetProduct(EProductType type)
        {
            return await _productService.GetProductWithType(type);
        }

        // GET: product with category
        [HttpGet("getProductByCategory/")]
        public async Task<List<ProductRespondeDTO>> GetProduct(string category)
        {
            return await _productService.GetProductByCategory(category);
        }

        // POST: add a product
        [HttpPost("addProduct/")]
        public async Task<List<ProductRespondeDTO>> AddProduct([FromBody] ProductRequestDTO request)
        {
            return await _productService.AddProduct(request);
        }

        // PUT: modify a product
        [HttpPut("updateProduct/")]
        public async Task<List<ProductRespondeDTO>> UpdateProduct(ProductRequestDTO request)
        {
            return await _productService.UpdateProduct(request);

        }

        // DELETE: a product
        [HttpDelete("deleteProduct/")]
        public async Task<List<ProductRespondeDTO>> DeleteProduct(int id)
        {
            return await _productService.DeleteProduct(id);
        }
    }
}
