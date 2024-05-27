using Microsoft.EntityFrameworkCore;
using prueba.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<HomeBankingContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("HomeBankingConexion")
));

var app = builder.Build();

using (var scope = app.Services.CreateScope()) {
    try {
        var service = scope.ServiceProvider;
        var context = service.GetRequiredService<HomeBankingContext>();
        InitializerDB.Main(context);
    }
    catch (Exception ex) {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating the DB.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
