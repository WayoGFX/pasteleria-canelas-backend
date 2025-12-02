using Microsoft.AspNetCore.Mvc;
using PasteleriaCanelas.Services.Interfaces;
using PasteleriaCanelas.Services.DTOs;
using System.Threading.Tasks;

namespace PasteleriaCanelas.Api.Controllers;

// controlador del catálogo para clientes

[ApiController]
[Route("api/[controller]")]
public class CatalogoController : ControllerBase
{
    //el controlador pide la interfaz de IProductoService | principio clave de la inyección de dependencias
    private readonly IProductoService _productoService;

    // el constructor recibe la interfaz de IProductoService, no la implementación directa
    // entonces .NET se encargade darle una instancia de la clase ProductoService
    public CatalogoController(IProductoService productoService)
    {
        _productoService = productoService;
    }


    // Endpoint de catálogo inicial
    // GET /api/Catalogo/inicial
    [HttpGet("inicial")]
    public async Task<ActionResult<CatalogoInicialDto>> GetCatalogoInicial()
    {
        var catalogo = await _productoService.ObtenerCatalogoInicial();
        return Ok(catalogo);
    }

}
