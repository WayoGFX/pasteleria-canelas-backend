namespace PasteleriaCanelas.Services.DTOs;

public class CategoriaDto
{
    public int CategoriaId { get; set; }
    public string Nombre { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string? Descripcion { get; set; }
    public string? Icono { get; set; }
    public string? ImagenUrl { get; set; } 
    public bool Activo { get; set; }
}