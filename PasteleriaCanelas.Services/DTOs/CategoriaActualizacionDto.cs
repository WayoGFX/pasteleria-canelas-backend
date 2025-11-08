namespace PasteleriaCanelas.Services.DTOs;

public class CategoriaActualizacionDto
{
    public int CategoriaId { get; set; }
    public string Nombre { get; set; } = null!;
    public string? Descripcion { get; set; }
    public string? ImagenUrl { get; set; } 
    public string? Icono { get; set; }
    public bool Activo { get; set; } 
}