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
    private static List<ProductRespondeDTO> ToListDTO(List<Product> products)
    {

        if (products is null)
        {
            throw new ProductException("An product list should not be null here.");
        }

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

        if (request is null || product is null)
        {
            throw new ProductException("An product should not be null here.");
        }

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
        if (request is null)
        {
            throw new ProductException("An product should not be null here.");
        }

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

    // Summary:
    //     Finds all products. A query is made to the database for products, if found, is
    //     attached to the context and returned. If no product is found, then null is returned.
    //
    //
    // Returns:
    //     The product found, or null.
    //
    // Remarks:
    //     See Using GetAllProduct and GetProductById for more information and examples.
    public async Task<ActionResult<List<ProductRespondeDTO>>> GetAllProduct()
    {
        var products = await _context.Products.ToListAsync<Product>();

        if (products is null || products.Count == 0)
        {
            return null;
        }

        return ToListDTO(products);
    }

    //
    // Summary:
    //     Finds an product with the given primary key values. If an product with the given
    //     primary key values is being tracked by the context, then it is returned immediately
    //     without making a request to the database. Otherwise, a query is made to the database
    //     for a product with the given primary key values and this product, if found, is
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
            return null;
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

    //
    // Summary:
    //     Finds an product with the given type. If an product with the given
    //     type is being tracked by the context, then it is returned immediately
    //     without making a request to the database. Otherwise, a query is made to the database
    //     for an product with the given type and this product, if found, is
    //     attached to the context and returned. If no product is found, then null is returned.
    //
    //
    // Parameters:
    //   keyValues:
    //     The values of the enum type for the product to be found.
    //
    // Returns:
    //     The product found, or null.
    //
    // Remarks:
    //     See Using GetAllProduct and GetProductById for more information and examples.
    public async Task<ActionResult<List<ProductRespondeDTO>>> GetProductWithType(EProductType type)
    {

        if (type <= 0)
        {
            throw new ProductException($"Invalid input enum type '{type}'. An type enum should not be smaller than 0.");
        }

        string typeStr = ProductTypeExtension.ToString(type);

        var products = await _context.Products
                                .Where(p => p.Type == typeStr)
                                .ToListAsync();

        if (products is null || products.Count == 0)
            return null;

        return ToListDTO(products);
    }

    //
    // Summary:
    //     Finds an product with the given type. If an product with the given
    //     category is being tracked by the context, then it is returned immediately
    //     without making a request to the database. Otherwise, a query is made to the database
    //     for an product with the given category and this product, if found, is
    //     attached to the context and returned. If no product is found, then null is returned.
    //
    //
    // Parameters:
    //   keyValues:
    //     The values of the category for the product to be found.
    //
    // Returns:
    //     The product found, or null.
    //
    // Remarks:
    //     See Using GetAllProduct and GetProductById for more information and examples.
    public async Task<ActionResult<List<ProductRespondeDTO>>> GetProductByCategory(string category)
    {
        if (string.IsNullOrEmpty(category)) throw new CategoryException();

        var products = await _context.Products
                                .Where(p => p.CategoryId == Convert.ToInt32(category))
                                .ToListAsync();

        if (products is null || products.Count == 0) return null;

        return ToListDTO(products);
    }

    //
    // Summary:
    //     Add an product with the given product Data Transfer Oject (DTO). If an product with the given
    //     DTO's primary key already existed in database, then it is returned an exception and cancel request immediately.
    //     Otherwise, an insert command is made to the database for the product with the given DTO and this product,
    //     if success, is attached to the context and returned a full list of products.
    //
    //
    // Parameters:
    //   keyValues:
    //     The object of product's DTO for the product to be added.
    //
    // Returns:
    //     Full list of products.
    //
    // Remarks:
    //     See Using UpdateProduct for more information and examples.
    public async Task<ActionResult<List<ProductRespondeDTO>>> AddProduct(ProductRequestDTO request)
    {
        // If given product is null
        if (request is null)
        {
            throw new ProductNullException();
        }

        // If given product already exist
        if (await _context.Products.FindAsync(request.Id) is not null)
        {
            throw new ProductAlreadyExistedException();
        }

        var product = NewProduct(request);

        _context.Products.Add(product);
        _context.SaveChanges();

        return await GetAllProduct();
    }

    //
    // Summary:
    //     Update an product with the given product Data Transfer Oject (DTO). If an product with the given
    //     DTO's primary key is not already existed in database, then it is returned an exception and cancel 
    //     request immediately. Otherwise, an update is made to the database for the product with the 
    //     given DTO and this product, if success, returned a full list of products.
    //
    //
    // Parameters:
    //   keyValues:
    //     The object of product's DTO for the product to be updated.
    //
    // Returns:
    //     Full list of products.
    //
    // Remarks:
    //     See Using AddProduct for more information and examples.
    public async Task<ActionResult<List<ProductRespondeDTO>>> UpdateProduct(ProductRequestDTO request)
    {
        if (request is null)
        {
            throw new ProductNullException();
        }

        var product = await _context.Products.FindAsync(request.Id);

        // If product existed or not existed
        if (product is not null)
        {
            // Product exist, update product
            product = Update(product, request);
            _context.SaveChanges();
        }
        else
        {
            // Product does not exist
            throw new ProductException();
        }

        return await GetAllProduct();
    }

    // Delete a product by Id
    // IF product not exist return BadRequestResult
    // IF product exist delete it.
    //
    // Summary:
    //     Delete an product with the given primary key. If an product with the given
    //     primary key is not already existed in database, then it is returned null.
    //     Otherwise, an update is made to the database for the product with the 
    //     given primary key and this product, if success, returned a full list of products.
    //
    //
    // Parameters:
    //   keyValues:
    //     The values of primary key for the product to be deleted.
    //
    // Returns:
    //     Full list of products, or null.
    //
    // Remarks:
    //     See Using AddProduct for more information and examples.
    public async Task<ActionResult<List<ProductRespondeDTO>>> DeleteProduct(int id)
    {
        if (id < 0)
        {
            throw new InvalidIdException();
        }

        var product = await _context.Products.FindAsync(id);

        if (product is null)
        {
            return null;
        }

        _context.Remove(product);
        _context.SaveChanges();

        return await GetAllProduct();
    }
}