using HomeBanking.Repository;
using HomeBanking.Repository.Implementations;
using HomeBanking.Services;
using HomeBanking.Services.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using prueba.Models;
using prueba.Repository;
using prueba.Repository.Implementations;
using System.Text;

var jwtSecretKey = "9Ks3bnBGx8fGJdN7VFnTY8jCDYmq/fR/4V5yVWGzs7Y=";


//Patron Builder
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
//builder.Services.AddAuthentication()
//      .AddCookie(options => {
//          options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
//          options.LoginPath = new PathString("/index.html");
//      });

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt => {
    var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey));
    var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature);

    opt.RequireHttpsMetadata = false;
    
    opt.TokenValidationParameters = new TokenValidationParameters(){
        IssuerSigningKey = signingKey,
        ValidateAudience = false,
        ValidateIssuer = false,
    };

});

//Agrego servicios de autorizacion
builder.Services.AddAuthorization(options => {
    options.AddPolicy("ClientOnly", policy => policy.RequireClaim("Client"));
    options.AddPolicy("AdminOnly", policy => policy.RequireClaim("Admin"));
});

//Agrego repositorios para usarlos en los controladores
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ILoanRepository, LoanRepository>();
builder.Services.AddScoped<ICardRepository, CardRepository>();
builder.Services.AddScoped<IClientLoanRepository, ClientLoanRepository>();

builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ICardService, CardService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IClientLoanService, ClientLoanService>();
builder.Services.AddScoped<ILoanService, LoanService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope()) {
    try {
        var service = scope.ServiceProvider;
        var context = service.GetRequiredService<HomeBankingContext>();
        //InitializerDB.Main(context);
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
