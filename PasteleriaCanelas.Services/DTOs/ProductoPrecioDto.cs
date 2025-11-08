namespace PasteleriaCanelas.Services.DTOs;

public class ProductoPrecioDto
{
    public int ProductoPrecioId { get; set; }
    public string DescripcionPrecio { get; set; } = null!;
    public decimal Precio { get; set; }
}