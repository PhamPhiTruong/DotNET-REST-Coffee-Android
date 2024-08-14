using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

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
}