namespace PasteleriaCanelas.Services.DTOs;

public class ProductoDetallesDto
{
    public int ProductoId { get; set; }
    public string? Nombre { get; set; }
    public string? Descripcion { get; set; }
    public string? ImagenUrl { get; set; }
    public bool Activo { get; set; }
    public string Slug { get; set; } = null!;
    public CategoriaDto Categoria { get; set; } = null!;

    public bool EsDeTemporada { get; set; }
    public ICollection<ProductoPrecioDto> ProductoPrecios { get; set; } = new List<ProductoPrecioDto>();
}