namespace PasteleriaCanelas.Domain.Entities;

// referencia a tabla de producto en la base de datos
public class Producto
{
  public int ProductoId { get; set; }

  public int CategoriaId { get; set; }

  public string? Nombre { get; set; }

  public string? Descripcion { get; set; }

  public string? ImagenUrl { get; set; }

  public bool Activo { get; set; }
    
  public string Slug { get; set; } = null!;

  public bool EsDeTemporada { get; set; }

  public virtual Categoria Categoria { get; set; } = null!;

  public virtual ICollection<ProductoPrecio> ProductoPrecios { get; set; } = new List<ProductoPrecio>();

}
