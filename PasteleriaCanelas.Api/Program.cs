// capas necesarias
using Microsoft.EntityFrameworkCore;
using PasteleriaCanelas.Data.Context;
using PasteleriaCanelas.Services;
using Microsoft.AspNetCore.Mvc; // Necesario para ApiBehaviorOptions | osea manejo de errores
using PasteleriaCanelas.Services.Interfaces;
using PasteleriaCanelas.Services.Services;
using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);

// ============================================================================
// CONFIGURAR PUERTO DINÁMICO (DEBE IR ANTES DE builder.Build())
// ============================================================================
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(int.Parse(port));
});

var myAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin() // solo rutas permitiras (Angular, React, etc.)
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Habilitar compresión de respuestas (Brotli y Gzip)
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});


// ============================================================================
// CONFIGURAR CONEXIÓN A POSTGRESQL (CON CONVERSIÓN DE URL DE RAILWAY)
// ============================================================================
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");

// Si existe DATABASE_URL de Railway, convertirla
if (!string.IsNullOrEmpty(connectionString))
{
    connectionString = ConvertPostgresUrl(connectionString);
}
else
{
    // Fallback a appsettings.json (para desarrollo local)
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
}

builder.Services.AddDbContext<PasteleriaDbContext>(options =>
    options.UseNpgsql(connectionString));

// servicio de producto al contenedor de inyección de dependencias.
// Esto enlaza la interfaz (IProductoService) con su implementación (ProductoService).
// AddScoped significa que se crea una nueva instancia por cada petición HTTP.
builder.Services.AddScoped<IProductoService, ProductoService>();
// service de categorias
builder.Services.AddScoped<ICategoriaService, CategoriaService>();

// Habilitar caché en memoria
builder.Services.AddMemoryCache();


// añadir los servicios al controlador.
builder.Services.AddControllers();
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        // Esta función se ejecuta cuando el model state no es válido.
        options.InvalidModelStateResponseFactory = context =>
        {
            // Extraemos los errores de validación del ModelState.
            var errors = context.ModelState
                .Where(e => e.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            // Objeto de respuesta de error.
            var problemDetails = new ValidationProblemDetails(context.ModelState);
            
            return new BadRequestObjectResult(problemDetails);
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Deshabilitar redirección HTTPS en producción (Railway maneja SSL)
if (!app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}

// Habilitar compresión de respuestas
app.UseResponseCompression();

// Habilitar CORS
app.UseCors(myAllowSpecificOrigins);

// permite hacer peticiones http y procesarlas
app.MapControllers();

app.Run();


// ============================================================================
// MÉTODO HELPER: Convertir DATABASE_URL de Railway al formato de .NET
// ============================================================================
static string ConvertPostgresUrl(string url)
{
    try
    {
        // Railway da: postgres://user:pass@host:port/dbname
        // .NET necesita: Host=host;Port=port;Database=dbname;Username=user;Password=pass
        
        var uri = new Uri(url.Replace("postgres://", "postgresql://"));
        var db = uri.AbsolutePath.Trim('/');
        var userInfo = uri.UserInfo.Split(':');
        var user = userInfo[0];
        var password = userInfo.Length > 1 ? userInfo[1] : "";
        var host = uri.Host;
        var port = uri.Port > 0 ? uri.Port : 5432;
        
        return $"Host={host};Port={port};Database={db};Username={user};Password={password};SSL Mode=Require;Trust Server Certificate=true";
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error convirtiendo DATABASE_URL: {ex.Message}");
        return url; // Si falla, devuelve el original
    }
}