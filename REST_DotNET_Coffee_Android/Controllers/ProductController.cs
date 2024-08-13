using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace REST_DotNET_Coffee_Android.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        // Assign DbContext
        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: all products
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProduct()
        {
            var products = await _context.Products.ToListAsync();

            return Ok(products);
        }

        // GET: a single product
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product is null)
            {
                return BadRequest("Product not found!");
            }

            return Ok(product);
        }

        // POST: add a product
        [HttpPost]
        public async Task<ActionResult<List<Product>>> AddProduct(Product request)
        {
            if (request is null)
            {
                return BadRequest("Invalid POST request!");
            }

            _context.Products.Add(request);
            _context.SaveChanges();

            return Ok(await GetProduct());
        }

        // PUT: modify a product
        [HttpPut]
        public async Task<ActionResult<List<Product>>> UpdateProduct(Product request)
        {
            var product = await _context.Products.FindAsync(request.Id);

            if(product is null)
            {
                return BadRequest("Product not found!");
            }

            product.Name = request.Name;
            product.AvatarUrl = request.AvatarUrl;
            product.BasePrice = request.BasePrice;
            product.Active = request.Active;
            product.CategoryId = request.CategoryId;
            product.Type = request.Type;
            product.Quantities = request.Quantities;

            _context.SaveChanges();

            return Ok(await GetProduct());

        }

        // DELETE: a product
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Product>>> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if(product is null)
            {
                return BadRequest("Product not found!");
            }

            _context.Products.Remove(product);
            _context.SaveChanges();

            return Ok(await GetProduct());
        }
    }
}
