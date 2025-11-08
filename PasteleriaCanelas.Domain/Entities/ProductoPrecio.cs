namespace PasteleriaCanelas.Domain.Entities;

// referencia a tabla de producto precio en la base de datos
public class ProductoPrecio
{
    public int ProductoPrecioId { get; set; }

    public int ProductoId { get; set; }

    public string DescripcionPrecio { get; set; } = null!;

    public decimal Precio { get; set; }

    public virtual Producto Producto { get; set; } = null!;

}
