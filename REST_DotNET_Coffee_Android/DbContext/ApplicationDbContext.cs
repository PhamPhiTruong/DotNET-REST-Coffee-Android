using Microsoft.EntityFrameworkCore;
using REST_DotNET_Coffee_Android.Entities;

public class ApplicationDbContext : DbContext
{

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }

    public DbSet<Product> Products { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<Ingredient> Ingredients { get; set; }

    public DbSet<UserDetail> UserDetails { get; set; }
}
