using PasteleriaCanelas.Services.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace PasteleriaCanelas.Services.Interfaces;

// la interfaz define el contrato. define los métodos que existen pero no como se implementan
public interface IProductoService
{
    // Administrador

    // CRUD Productos 
    Task<ProductoDetallesDto?> ObtenerProductoPorId(int productoId);
    Task<IEnumerable<ProductoDetallesDto>?> ObtenerTodosProductos();
    Task<ProductoDetallesDto?> CrearProducto(ProductoCreacionDto productoDto);
    Task<bool> ActualizarProducto(ProductoActualizacionDto productoDto);
    Task<bool> EliminarProducto(int productoId);

    // CRUD Precio Producto
    Task<ProductoPrecioDto?> CrearPrecio(ProductoPrecioCreacionDto precioDto);
    Task<bool> ActualizarPrecio(ProductoPrecioActualizacionDto precioDto);
    Task<bool> EliminarPrecio(int precioId);

    // Usuario normal

    //Catálogo inicial optimizado (categorías + productos + temporada en una sola petición)
    Task<CatalogoInicialDto> ObtenerCatalogoInicial();
}