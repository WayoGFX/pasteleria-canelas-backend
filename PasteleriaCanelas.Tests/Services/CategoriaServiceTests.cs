using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PasteleriaCanelas.Data.Context;
using PasteleriaCanelas.Domain.Entities;
using PasteleriaCanelas.Services;
namespace PasteleriaCanelas.Tests.Services;
public class CategoriaServiceTests
{
    // Método helper para crear una base de datos limpia en memoria por cada test
    private PasteleriaDbContext CrearDbContextEnMemoria(string nombreBd)
    {
        var options = new DbContextOptionsBuilder<PasteleriaDbContext>()
            .UseInMemoryDatabase(databaseName: nombreBd)
            .Options;
        return new PasteleriaDbContext(options);
    }


    [Fact]
    public async Task
    EliminarCtegoria_CuandoTieneProductosAsociados_DebeRetornarFalseYNoEliminar()
    {
        // Arrange (Prepare the data of test)
        var dbContext = CrearDbContextEnMemoria(nameof(EliminarCtegoria_CuandoTieneProductosAsociados_DebeRetornarFalseYNoEliminar));

        var categoria = new Categoria
        {
            CategoriaId = 1,
            Nombre = "Postres",
            Slug = "postres",
            Activo = true
        };

        var producto = new Producto
        {
            ProductoId = 1,
            Nombre = "Tiramisú",
            Slug = "tiramisu",
            CategoriaId = 1,
            Activo = true
        };

        dbContext.Categorias.Add(categoria);
        dbContext.Productos.Add(producto);
        await dbContext.SaveChangesAsync();

        var service = new CategoriaService(dbContext);

        // Act actuar
        var resultado = await service.EliminarCategoria(1);

        // Assert verificar
        resultado.Should().BeFalse(); // es decir que debe rechazar la eliminacion
        var categoriaEndB = await dbContext.Categorias.FindAsync(1);
        categoriaEndB.Should().NotBeNull(); // la categoria debe seguir existiendo
    }
    
    [Fact]
    public async Task EliminarCategoria_CuandoNoTieneProductos_DebeRetornarTrueYEliminar()
    {
        // Arrange
        var dbContext = CrearDbContextEnMemoria(nameof(EliminarCategoria_CuandoNoTieneProductos_DebeRetornarTrueYEliminar));
        var categoria = new Categoria
        {
            CategoriaId = 2,
            Nombre = "Bebidas",
            Slug = "bebidas",
            Activo = true
        }; 

        dbContext.Categorias.Add(categoria);
        await dbContext.SaveChangesAsync();

        var service = new CategoriaService(dbContext);

        // Act
        var resultado = await service.EliminarCategoria(2);

        // Assert
        resultado.Should().BeTrue();
        var categoriaEnDb = await dbContext.Categorias.FindAsync(2);
        categoriaEnDb.Should().BeNull(); // ya no debe existir
    }
}