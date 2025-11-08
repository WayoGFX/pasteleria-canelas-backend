using PasteleriaCanelas.Services.DTOs;

namespace PasteleriaCanelas.Services.DTOs;

public class ProductoResumenConCategoriaDto
{
    public int ProductoId { get; set; }
    public string? Nombre { get; set; }
    public string? Slug { get; set; }
    public string? Descripcion { get; set; }
    public string? ImagenUrl { get; set; }
    public bool EsDeTemporada { get; set; }
    public string CategoriaSlug { get; set; } = string.Empty;
    public List<ProductoPrecioDto> ProductoPrecios { get; set; } = new();
}