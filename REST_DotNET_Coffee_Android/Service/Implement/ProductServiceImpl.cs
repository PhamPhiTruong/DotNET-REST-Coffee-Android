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
                _context.Products.AddRange(products);
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
            Console.WriteLine(json);
            return JsonConvert.DeserializeObject<List<Product>>(json);
        }
        catch(Exception ex) 
        {
            _logger.LogError(ex, "Error loading product from file.");

            return null;
        }
    }
}
