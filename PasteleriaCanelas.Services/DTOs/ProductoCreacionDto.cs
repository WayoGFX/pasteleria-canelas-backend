namespace PasteleriaCanelas.Services.DTOs;

public class ProductoCreacionDto
{
    public string? Nombre { get; set; }
    public string? Descripcion { get; set; }
    public string? ImagenUrl { get; set; }
    public bool Activo { get; set; }
    public int CategoriaId { get; set; }
        public bool EsDeTemporada { get; set; }
}