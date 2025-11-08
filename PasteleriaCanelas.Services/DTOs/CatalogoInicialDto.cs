using PasteleriaCanelas.Services.DTOs;

public class CatalogoInicialDto
{
    public List<CategoriaDto> Categorias { get; set; } = new();
    public List<ProductoResumenConCategoriaDto> Productos { get; set; } = new();
    public List<ProductoResumenConCategoriaDto> Temporada { get; set; } = new();
}