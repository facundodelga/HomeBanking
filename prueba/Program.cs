using HomeBanking.Repository;
using HomeBanking.Repository.Implementations;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using prueba.Models;
using prueba.Repository;
using prueba.Repository.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<HomeBankingContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("HomeBankingConexion")
));


//Agrego servicios de autenticacion
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
      .AddCookie(options => {
          options.ExpireTimeSpan = TimeSpan.FromMinutes(1);
          options.LoginPath = new PathString("/index.html");
      });

//Agrego servicios de autorizaci?n
builder.Services.AddAuthorization(options => {
    options.AddPolicy("ClientOnly", policy => policy.RequireClaim("Client"));
});

//Agrego repositorios para usarlos en los controladores
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ILoanRepository, LoanRepository>();

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
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Error");
}else {
    app.UseSwagger();
    app.UseSwaggerUI();

}

app.UseStaticFiles();

app.UseRouting();

app.MapControllers();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
