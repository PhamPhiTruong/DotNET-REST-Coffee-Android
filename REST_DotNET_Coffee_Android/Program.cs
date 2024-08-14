using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(o =>
{
#pragma warning disable CS8604 // Possible null reference argument.
    o.UseMySQL(
    builder.Configuration.GetConnectionString("DefaultConnection")
    );
#pragma warning restore CS8604 // Possible null reference argument.
});

// More service
builder.Services.AddTransient<IProductService, ProductServiceImpl>();
builder.Services.AddTransient<IUserDetailService, UserDetailServiceImpl>();
builder.Services.AddTransient<IUserInfoService, UserInfoServiceImpl>();
builder.Services.AddTransient<IUserService, UserServiceImpl>();

builder.Services.AddTransient<DataInitializer>();

var app = builder.Build();

// Setup database service
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    var dataInitializer = scope.ServiceProvider.GetRequiredService<DataInitializer>();

    dbContext.Database.EnsureCreated();

    dataInitializer.Init();

}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
