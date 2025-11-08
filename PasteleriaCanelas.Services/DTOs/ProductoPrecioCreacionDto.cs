namespace PasteleriaCanelas.Services.DTOs;

public class ProductoPrecioCreacionDto
{
    public int ProductoId { get; set; }
    public string DescripcionPrecio { get; set; } = null!;
    public decimal Precio { get; set; }

}