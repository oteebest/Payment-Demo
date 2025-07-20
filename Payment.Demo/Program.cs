using Microsoft.EntityFrameworkCore;
using Payment.Demo.AppDependencies;
using Payment.Demo.Infrastructure.EntityFramework;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var services = builder.Services;
var configuration = builder.Configuration;

services.AddApplication(configuration)
    .AddInfrastructure(configuration);


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var seeder = scope.ServiceProvider.GetRequiredService<ProductSeeder>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    try
    {
        logger.LogInformation("Starting database migration and seeding...");

        await context.Database.MigrateAsync();
        logger.LogInformation("Database migrations completed successfully");

        await seeder.SeedProductsAsync();
        logger.LogInformation("Product seeding completed successfully");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred during database migration or seeding");
        throw; // Re-throw to prevent app from starting with incomplete setup
    }
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Products/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Products}/{action=Index}/{id?}");

app.Run();
