using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

#nullable disable

public class ProductServiceImpl : AService<Product>, IProductService
{
    public ProductServiceImpl(ApplicationDbContext context, ILogger<ProductServiceImpl> logger) : base(context, logger) { }

    // Import data
    public void Initialize()
    {
        if (!_context.Products.Any())
        {
            var products = LoadProductsFromFile();

            if (products != null)
            {
                _context.AddRange(products);
                _context.SaveChanges();
                _logger.LogInformation("Inserted products from file.");
            }
        }
    }

    private List<Product> LoadProductsFromFile()
    {
        try
        {
            var json = File.ReadAllText("resources\\products.json");

            var data = JsonConvert.DeserializeObject<Dictionary<string, object>[]>(json);

            var products = new List<Product>();

            foreach (var item in data)
            {
                var product = new Product
                {
                    Id = Convert.ToInt32(item["id"]),
                    Name = item["name"].ToString(),
                    Type = item["type"].ToString(),
                    BasePrice = Convert.ToDouble(item["base_price"].ToString()),
                    Quantities = Convert.ToInt32(item["quantities"]),
                    Active = Convert.ToBoolean(item["active"]),
                    CategoryId = Convert.ToInt32(item["category_id"].ToString()),
                    AvatarUrl = item["avatar"].ToString()
                };

                products.Add(product);
            }

            return products;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading product from file.");

            return null;
        }
    }
        
    // From list of Product to list of DTO
    private static List<ProductRespondeDTO> ToDTO(List<Product> products)
    {
        if (products == null || products.Count == 0) return null;

        List<ProductRespondeDTO> productJSONs = new List<ProductRespondeDTO>();

        products.ForEach((Action<Product>)(p =>
        {
            productJSONs.Add(new ProductRespondeDTO
            {
                Id = p.Id,
                Name = p.Name,
                Type = p.Type,
                Active = Convert.ToBoolean(p.Active),
                AvatarUrl = p.AvatarUrl,
                BasePrice = Convert.ToDouble(p.BasePrice),
                CategoryId = Convert.ToInt32(p.CategoryId),
                Quantities = Convert.ToInt32(p.Quantities)
            });
        }));

        return productJSONs;
    }

    // Update product
    private static Product Update(Product product, ProductRequestDTO request)
    {
        product.Name = request.Name;
        product.AvatarUrl = request.AvatarUrl;
        product.BasePrice = request.BasePrice;
        product.Active = request.Active;
        product.CategoryId = request.CategoryId;
        product.Type = ProductTypeExtension.ToString(request.Type);
        product.Quantities = request.Quantities;

        return product;
    }

    private static Product NewProduct(ProductRequestDTO request)
    {
        var product = new Product
        {
            Id = request.Id,
            Name = request.Name,
            Active = Convert.ToBoolean(request.Active),
            AvatarUrl = request.AvatarUrl,
            BasePrice = Convert.ToDouble(request.BasePrice),
            CategoryId = request.CategoryId,
            Quantities = Convert.ToInt32(request.Quantities),
            Type = ProductTypeExtension.ToString(request.Type),
        };

        return product;
    }

    // Return all Products from database and turn them to DTO
    public async Task<ActionResult<List<ProductRespondeDTO>>> GetAllProduct()
    {
        var products = await _context.Products.ToListAsync<Product>();

        if (products is null || products.Count == 0)
        {
            return new BadRequestResult();
        }

        return ToDTO(products);
    }

    // Summary:
    //     Finds an product with the given primary key values. If an product with the given
    //     primary key values is being tracked by the context, then it is returned immediately
    //     without making a request to the database. Otherwise, a query is made to the database
    //     for an product with the given primary key values and this product, if found, is
    //     attached to the context and returned. If no product is found, then null is returned.
    //
    //
    // Parameters:
    //   keyValues:
    //     The values of the primary key for the product to be found.
    //
    // Returns:
    //     The product found, or null.
    //
    // Remarks:
    //     See Using GetAllProduct and GetProductById for more information and examples.
    public async Task<ActionResult<ProductRespondeDTO>> GetProductById(int id)
    {
        if (id <= 0)
        {
            throw new InvalidIdException($"Invalid id {id}, an id should not be smaller than 0.");
        }

        var product = await _context.Products.FindAsync(id);

        if (product is null)
        {
            return new BadRequestResult();
        }

        return new ProductRespondeDTO
        {
            Id = id,
            Name = product.Name,
            Type = product.Type,
            Active = Convert.ToBoolean(product.Active),
            AvatarUrl = product.AvatarUrl,
            BasePrice = Convert.ToDouble(product.BasePrice),
            CategoryId = Convert.ToInt32(product.CategoryId),
            Quantities = Convert.ToInt32(product.Quantities)
        };
    }

    // Get product by type and turns it to DTO
    // IF SUCCEED return full set of Products
    // IF UNSUCCEED Return BadRequestResult
    public async Task<ActionResult<List<ProductRespondeDTO>>> GetProductWithType(EProductType type)
    {

        string typeStr = ProductTypeExtension.ToString(type);

        var products = await _context.Products
                                .Where(p => p.Type == typeStr)
                                .ToListAsync();

        return ToDTO(products);
    }

    // Get product by category and turns it to DTO
    // IF SUCCEED return full set of Products
    // IF UNSUCCEED Return BadRequestResult
    public async Task<ActionResult<List<ProductRespondeDTO>>> GetProductByCategory(string category)
    {
        if (string.IsNullOrEmpty(category)) return new BadRequestResult();

        var products = await _context.Products
                                .Where(p => p.CategoryId == Convert.ToInt32(category))
                                .ToListAsync();

        return ToDTO(products);
    }

    // Turn a product DTO into a Product and add it to database, return full set of Products
    // IF UNSUCCEED Return BadRequestResult
    // IF product already exist, cancel request Return BadRequestResult
    // IF SUCCEED return full set of Products
    public async Task<ActionResult<List<ProductRespondeDTO>>> AddProduct(ProductRequestDTO request)
    {
        if (request is null)
        {
            Console.Error.WriteLine("Product is null!");
            return new BadRequestResult();
        }

        // If product already exist, cancel request
        if (await _context.Products.FindAsync(request.Id) is not null)
        {
            Console.Error.WriteLine("Product already exist!");
            return new BadRequestResult();
        }

        var product = NewProduct(request);

        _context.Products.Add(product);
        _context.SaveChanges();

        return await GetAllProduct();
    }

    // Turn a product DTO into a Product and update it to database
    // IF SUCCEED return full set of Products
    // IF UNSUCCEED Return BadRequestResult
    public async Task<ActionResult<List<ProductRespondeDTO>>> UpdateProduct(ProductRequestDTO request)
    {
        if (request is null)
        {
            return new BadRequestResult();
        }

        var product = await _context.Products.FindAsync(request.Id);

        // If product does not exist, add a new product
        if (product is not null)
        {
            // Product exist, update product
            product = Update(product, request);
            _context.SaveChanges();
        }
        else
        {
            // Product does not exist, add new product
            await AddProduct(request);
        }

        return await GetAllProduct();
    }

    // Delete a product by Id
    // IF product not exist return BadRequestResult
    // IF product exist delete it.
    public async Task<ActionResult<List<ProductRespondeDTO>>> DeleteProduct(int id)
    {
        if (id < 0)
        {
            return new BadRequestResult();
        }

        var product = await _context.Products.FindAsync(id);

        if (product is null)
        {
            return new BadRequestResult();
        }

        _context.Remove(product);
        _context.SaveChanges();

        return await GetAllProduct();
    }
}