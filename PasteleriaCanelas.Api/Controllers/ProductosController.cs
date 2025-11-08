using Microsoft.AspNetCore.Mvc;
using PasteleriaCanelas.Services.Interfaces;
using PasteleriaCanelas.Api.Helpers;
using PasteleriaCanelas.Services.DTOs;
using System.Threading.Tasks;

namespace PasteleriaCanelas.Api.Controllers;

[ApiController]
[Route("api/[controller]")] //
public class ProductosController : ControllerBase
{
    //el controlador pide la interfaz de IProductoService
    // esto es un principio clave de la inyección de dependencias

    private readonly IProductoService _productoService;

    // el constructor recibe la interfaz de IProductoService, no la implementación directa
    // entonces .NET se encargade darle una instancia de la clase ProductoService
    public ProductosController(IProductoService productoService)
    {
        _productoService = productoService;
    }


    // Endpoint 1: mostrar todos los productos
    // GET /api/Productos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductoDetallesDto>>> GetAdminProductos()
    {
        //se retornan los datos y se envía respuesta
        var productos = await _productoService.ObtenerTodosProductos();
        return Ok(productos);
    }

    // Endpoint 2: mostrar producto por id
    // GET /api/Productos/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductoDetallesDto>> GetProducto(int id)
    {
        // se manda todo a la logica de negocio para validarlo
        var producto = await _productoService.ObtenerProductoPorId(id);
        // si no hay producto entonces retorna null
        if (producto == null)
        {
            return NotFound(ApiMensajes.ProductoNoEncontrado);
        }
        // si hay se retorna la respuesta
        return Ok(producto);

    }

    // Endpoint 3: agregar nuevo producto
    // POST /api/Productos
    [HttpPost]
    public async Task<IActionResult> PostProducto(ProductoCreacionDto productoDto)
    {
        // llama al servicio para manejar la lógica de creación del producto
        var productoCreado = await _productoService.CrearProducto(productoDto);

        // verificar que si se creó
        if (productoCreado == null)
        {
            // si no entonces devuelve error 400 de badquest
            return BadRequest(ApiMensajes.CategoriaParaProductoNoExiste);
        }
        // en caso de si crearse se retorna un 201 de creado con la info del producto
        return CreatedAtAction(nameof(GetProducto), new { id = productoCreado.ProductoId }, productoCreado);
    }


    // Endpoint 4: actualizar producto
    // PUT /api/Productos
    [HttpPut("{id}")]
    public async Task<IActionResult> PutProducto(int id, ProductoActualizacionDto productoDto)
    {
        //Validar que el id coincida con el Id de la información
        if (id != productoDto.ProductoId)
        {
            return BadRequest(ApiMensajes.IdInvalido);
        }

        // verificar que si se encontró el producto
        var resultado = await _productoService.ActualizarProducto(productoDto);

        // si retorna falso es porque no se encontró producto
        if (!resultado)
        {
            return NotFound(ApiMensajes.ProductoNoEncontrado);
        }

        // retornar respuesta de todo correcto si la actualización resultó
        return NoContent();
    }

    // Endpoint 5: eliminar producto
    // DELETE /api/Productos
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProducto(int id)
    {
        // se envían los datos a la lógica de negocio
        var resultado = await _productoService.EliminarProducto(id);

        // se verifica que todo salió bien
        if (!resultado)
        {
            return NotFound(ApiMensajes.ProductoNoEncontrado);
        }

        // no se retorna nada
        return NoContent();
    }

}
