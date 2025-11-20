using Microsoft.EntityFrameworkCore;
using SportsStore.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<StoreDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("SportsStoreConnection")));
builder.Services
    .AddScoped<IStoreRepository, EFStoreRepository>();  

var app = builder.Build();

app.UseStaticFiles();

app.MapControllerRoute(
    name: "pagination",
    pattern: "Products/Page{productPage:int}",
    defaults: new { Controller = "Home", action = "Index" }); 
app.MapDefaultControllerRoute();
SeedData.EnsurePopulated(app);
app.Run();
