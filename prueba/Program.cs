// Importar los namespaces necesarios para acceder a los repositorios y modelos
using HomeBanking.Repository;
using HomeBanking.Repository.Implementations;
using Microsoft.EntityFrameworkCore;
using prueba.Models;
using prueba.Repository;
using prueba.Repository.Implementations;

// Crear una nueva instancia de WebApplication
var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor de dependencias
builder.Services.AddRazorPages();  // Agregar soporte para páginas Razor

// Configurar el contexto de Entity Framework Core para la base de datos
builder.Services.AddDbContext<HomeBankingContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("HomeBankingConexion")
));

// Registrar los repositorios en el contenedor de dependencias para la inyección de dependencias
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ILoanRepository, LoanRepository>();

// Construir la aplicación
var app = builder.Build();

// Crear un nuevo scope para gestionar los servicios
using (var scope = app.Services.CreateScope()) {
    try {
        // Obtener el proveedor de servicios
        var service = scope.ServiceProvider;
        // Obtener el contexto de la base de datos
        var context = service.GetRequiredService<HomeBankingContext>();
        // Inicializar la base de datos con datos predeterminados
        InitializerDB.Main(context);
    }
    catch (Exception ex) {
        // En caso de error, obtener el servicio de logging
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        // Registrar el error
        logger.LogError(ex, "An error occurred creating the DB.");
    }
}

// Configurar el pipeline de solicitud HTTP
if (!app.Environment.IsDevelopment()) {
    // Si no estamos en entorno de desarrollo, usar el manejador de excepciones
    app.UseExceptionHandler("/Error");
}

// Habilitar el uso de archivos estáticos (CSS, JS, imágenes)
app.UseStaticFiles();

// Habilitar el enrutamiento
app.UseRouting();

// Mapear los controladores para las API
app.MapControllers();

// Habilitar la autorización
app.UseAuthorization();

// Mapear las páginas Razor
app.MapRazorPages();

// Iniciar la aplicación
app.Run();