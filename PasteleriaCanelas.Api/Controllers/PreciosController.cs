using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PasteleriaCanelas.Services.Interfaces;
using PasteleriaCanelas.Services.DTOs;
using System.Collections.Generic;
using PasteleriaCanelas.Api.Helpers;
using System.Threading.Tasks;
using PasteleriaCanelas.Services.Services;


namespace PasteleriaCanelas.Api.Controllers;

// controlador para precios de productos

[ApiController]
[Route("api/[controller]")]
public class PreciosController : ControllerBase
{
    private readonly IProductoService _productoService;

    public PreciosController(IProductoService productoService)
    {
        _productoService = productoService;
    }

    // Endpoint 2: Agregar productos
    // POST /api/Precios
    [HttpPost]
    public async Task<ActionResult<ProductoPrecioDto>> PostPrecio(ProductoPrecioCreacionDto precioDto)
    {
        var nuevoPrecio = await _productoService.CrearPrecio(precioDto);
        if (nuevoPrecio == null)
        {
            return NotFound(ApiMensajes.ProductoParaPrecioNoEncontrado);
        }
        return CreatedAtAction(null, null, nuevoPrecio);
    }

    // Endpoint 2: Agregar productos
    // PUT /api/Precios/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPrecio(int id, ProductoPrecioActualizacionDto precioDto)
    {
        if (id != precioDto.ProductoPrecioId)
        {
            return BadRequest(ApiMensajes.IdInvalido);
        }

        var exito = await _productoService.ActualizarPrecio(precioDto);
        if (!exito)
        {
            return NotFound(ApiMensajes.PrecioNoEncontrado);
        }
        return NoContent();
    }

    // Endpoint 2: Eliminar productos
    // DELETE /api/Precios/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePrecio(int id)
    {
        var exito = await _productoService.EliminarPrecio(id);
        if (!exito)
        {
            return NotFound(ApiMensajes.PrecioNoEncontrado);
        }
        return NoContent();
    }
}