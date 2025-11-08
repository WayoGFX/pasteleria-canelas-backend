using Microsoft.AspNetCore.Mvc;
using PasteleriaCanelas.Services.DTOs;
using PasteleriaCanelas.Api.Helpers;
using PasteleriaCanelas.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PasteleriaCanelas.Api.Controllers;

// controlador de las categorías - para administrador

[ApiController]
[Route("api/[controller]")]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaService _categoriaService;

    public CategoriasController(ICategoriaService categoriaService)
    {
        _categoriaService = categoriaService;
    }

    // Endpoint 1: Mostrar todas las categorias | solo si está activa
    // GET /api/Categorias
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoriaDto>>> GetCategorias()
    {
        var categorias = await _categoriaService.ObtenerCategorias();
        return Ok(categorias);
    }

    // Endpoint 2: Mostrar categoria por id | solo si está activa
    // GET /api/Categorias/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<CategoriaDto>> GetCategoriaId(int id)
    {
        var categoria = await _categoriaService.ObtenerCategoriaPorId(id);
        if (categoria == null)
        {
            return NotFound(ApiMensajes.CategoriaNoEncontrada);
        }
        return Ok(categoria);
    }

    // Endpoint 3: Agregar nueva categoria
    // POST /api/Categorias
    [HttpPost]
    public async Task<ActionResult<CategoriaDto>> PostCategoria(CategoriaCreacionDto categoriaDto)
    {
        var nuevaCategoria = await _categoriaService.CrearCategoria(categoriaDto);

        if (nuevaCategoria == null)
        {
            return BadRequest(ApiMensajes.CreacionCategoriaFallida);
        }

        return CreatedAtAction(nameof(GetCategoriaId), new { id = nuevaCategoria.CategoriaId }, nuevaCategoria);
    }

    // Endpoint 3: actualizar la categoria
    // PUT /api/Categorias/{id}
    [HttpPut("{id}")]
public async Task<IActionResult> ActualizarCategoria(int id, [FromBody] CategoriaActualizacionDto categoriaDto)
{
    // Verifica que el ID en la URL coincida con el ID en el cuerpo de la solicitud
    if (id != categoriaDto.CategoriaId)
    {
        return BadRequest(ApiMensajes.IdInvalido);
    }

    var resultado = await _categoriaService.ActualizarCategoria(categoriaDto);

    if (!resultado)
    {
        return NotFound(ApiMensajes.CategoriaNoEncontrada);
    }

    return NoContent();
}

    // Endpoint 4: Eliminar categoria | solo si no tiene productos
    // DELETE /api/Categorias/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategoria(int id)
    {
        var exito = await _categoriaService.EliminarCategoria(id);
        if (!exito)
        {
            return NotFound(ApiMensajes.CategoriaNoEncontrada);
        }
        return NoContent();
    }
}