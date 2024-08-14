using Microsoft.EntityFrameworkCore;

public class DataInitializer
{

    private readonly IProductService _productService;
    private readonly IUserDetailService _userDetailService;
    private readonly IUserInfoService _userInfoService;
    private readonly IUserService _userService;

    public DataInitializer(IProductService productService,IUserService userService,IUserInfoService userInfoService, IUserDetailService userDetailService)
    {
        _productService = productService;

        _userDetailService = userDetailService;

        _userInfoService = userInfoService;

        _userService = userService;
    }



    public void Init()
    {
        // Generate default product
        _productService.Initialize();
        // Generate default user details
        _userDetailService.Initialize();
        // Add more data here
        _userInfoService.Initialize();
        
        _userService.Initialize();
    }

    public void Shutdown(ApplicationDbContext context)
    {
        // Delete all table after shutdown application
        Console.WriteLine("Deleting!");
        using (var scope = context.Database.BeginTransaction())
        {
            context.Database.ExecuteSqlRaw("DROP TABLE products");

            context.Database.ExecuteSqlRaw("DROP TABLE user_details");

            Console.WriteLine("Deleted!");

            context.Database.ExecuteSqlRaw("DROP TABLE __efmigrationshistory;");

            Console.WriteLine("Deleted!");

            scope.Commit();

            context.SaveChanges();
        }
    }
}