using Microsoft.EntityFrameworkCore;

public class DataInitializer
{

    private readonly IProductService _productService;

    public DataInitializer(IProductService productService)
    {
        _productService = productService;
    }

    public void Init()
    {
        // Generate default product
        _productService.Initialize();

        // Add more data here

    }

    public void Shutdown(ApplicationDbContext context)
    {
        // Delete all table after shutdown application
        Console.WriteLine("Deleting!");
        using (var scope = context.Database.BeginTransaction())
        {
            context.Database.ExecuteSqlRaw("DROP TABLE products;");

            Console.WriteLine("Deleted!");

            context.Database.ExecuteSqlRaw("DROP TABLE __efmigrationshistory;");

            Console.WriteLine("Deleted!");

            scope.Commit();

            context.SaveChanges();
        }
    }
}