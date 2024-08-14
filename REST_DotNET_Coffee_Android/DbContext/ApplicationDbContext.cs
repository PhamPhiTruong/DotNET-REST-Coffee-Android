using Microsoft.EntityFrameworkCore;
using REST_DotNET_Coffee_Android.Entities;

public class ApplicationDbContext : DbContext
{

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }

    public DbSet<Product> Products { get; set; }

    public DbSet<UserDetail> UserDetails { get; set; }
}
