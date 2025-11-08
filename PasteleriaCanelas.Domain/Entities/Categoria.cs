namespace PasteleriaCanelas.Domain.Entities;

// referencia a tabla de categoria en la base de datos
public class Categoria
{
    public int CategoriaId { get; set; }

    public string Nombre { get; set; } = null!;

    public bool Activo { get; set; }
    public string? Descripcion { get; set; }
    public string? ImagenUrl { get; set; }
    public string? Icono { get; set; }
    public string Slug { get; set; } = null!;

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();

}


