namespace PasteleriaCanelas.Services.DTOs;

public class ProductoPrecioActualizacionDto
{
    public int ProductoPrecioId { get; set; }
    public string DescripcionPrecio { get; set; } = null!;
    public decimal Precio { get; set; }

}
