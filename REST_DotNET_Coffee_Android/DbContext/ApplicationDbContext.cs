using Microsoft.EntityFrameworkCore;
using REST_DotNET_Coffee_Android.Entities;

public class ApplicationDbContext : DbContext
{

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    public DbSet<HavingIngredient> HavingIngredients { get; set; }

    public DbSet<PaymentMethod> PaymentMethods { get; set; }

    public DbSet<AddIngredient> AddIngredients { get; set; }

    public DbSet<Ingredient> Ingredients { get; set; }

    public DbSet<UserDetail> UserDetails { get; set; }

    public DbSet<OrderItem> OrderItems { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<UserInfo> UserInfos { get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<Order> Orders { get; set; }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Assign Primary Key to HavingIngredients table
        modelBuilder.Entity<HavingIngredient>()
            .HasKey(hi => new {hi.ProductId, hi.IngredientId});

        //modelBuilder.Entity<HavingIngredient>()
        //    .HasOne(hi => hi.Product)
        //    .WithMany()
        //    .HasForeignKey("ProductId"); // Assign ForeignKey

        //modelBuilder.Entity<HavingIngredient>()
        //    .HasOne(hi => hi.Ingredient)
        //    .WithMany()
        //    .HasForeignKey("IngredientId"); // Assign ForeignKey

        /* ============================================================================================== */

        // Assign Primary Key to AddIngredient table
        modelBuilder.Entity<AddIngredient>()
            .HasKey(ai => new {ai.OrderItemId, ai.IngredientId});

        /* ============================================================================================== */

        // Assign Relation to User table
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
