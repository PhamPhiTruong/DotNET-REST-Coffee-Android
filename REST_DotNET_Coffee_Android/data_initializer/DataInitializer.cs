using Microsoft.EntityFrameworkCore;

public class DataInitializer
{
    private readonly IProductService _productService;

    private readonly IUserDetailService _userDetailService;

    private readonly IUserInfoService _userInfoService;

    private readonly IUserService _userService;

    private readonly ICategoryService _categoryService;

    private readonly IIngredientService _ingredientService;

    // Register service
    public DataInitializer(
        IUserService userService, 
        IProductService productService, 
        IUserInfoService userInfoService, 
        ICategoryService categoryService, 
        IUserDetailService userDetailService, 
        IIngredientService ingredientService)
    {
        _productService = productService;

        _userDetailService = userDetailService;

        _categoryService = categoryService;

        _ingredientService = ingredientService;

        _userInfoService = userInfoService;

        _userService = userService;
    }


    // Generate default data
    // Following template:
    // <SPACE>
    // <COMMENT> : Generate default <DATA_TYPE>
    // <CODE>    : <InterfaceService>.Initialize();
    // <SPACE>
    public void Init()
    {
        // Generate default categories
        _categoryService.Initialize();

        // Generate default product
        _productService.Initialize();

        // Generate default ingredients
        _ingredientService.Initialize();

        // Generate default user details
        _userDetailService.Initialize();

        // Generate default user infos
        _userInfoService.Initialize();

        // Generate default users
        _userService.Initialize();
        
        // Add more data here

    }

    // Drop database after Application closed, stopped.
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