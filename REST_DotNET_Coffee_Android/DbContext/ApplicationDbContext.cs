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

    public DbSet<UserInfo> UserInfos { get; set; }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasOne(u => u.infoId)
            .WithMany() // Hoặc .WithMany(ui => ui.Users) nếu UserInfo có nhiều User
            .HasForeignKey("UserInfoId") // Tên cột khóa ngoại sẽ được Entity Framework tạo ra
            .OnDelete(DeleteBehavior.Restrict); // Hoặc DeleteBehavior.Cascade nếu bạn muốn tự động xóa các bản ghi liên quan

        modelBuilder.Entity<User>()
            .HasOne(u => u.detailId)
            .WithMany() // Hoặc .WithMany(ud => ud.Users) nếu UserDetail có nhiều User
            .HasForeignKey("UserDetailId") // Tên cột khóa ngoại sẽ được Entity Framework tạo ra
            .OnDelete(DeleteBehavior.Restrict); // Hoặc DeleteBehavior.Cascade nếu bạn muốn tự động xóa các bản ghi liên quan

        base.OnModelCreating(modelBuilder);
    }
}
