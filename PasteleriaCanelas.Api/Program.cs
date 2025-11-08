// capas necesarias
using Microsoft.EntityFrameworkCore;
using PasteleriaCanelas.Data.Context;
using PasteleriaCanelas.Services;
using Microsoft.AspNetCore.Mvc; // Necesario para ApiBehaviorOptions | osea manejo de errores
using PasteleriaCanelas.Services.Interfaces;
using PasteleriaCanelas.Services.Services;
using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);

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


// inyección de dependencias para el DbContext
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL") ?? builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<PasteleriaDbContext>(options =>
    options.UseSqlServer(connectionString));

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

app.UseHttpsRedirection();

// Habilitar compresión de respuestas
app.UseResponseCompression();

// Habilitar CORS
app.UseCors(myAllowSpecificOrigins);

// permite hacer peticiones http y procesarlas
app.MapControllers();

app.Run();
    