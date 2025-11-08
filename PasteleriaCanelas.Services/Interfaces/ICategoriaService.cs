using PasteleriaCanelas.Services.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace PasteleriaCanelas.Services.Interfaces;

public interface ICategoriaService
{
    Task<CategoriaDto?> CrearCategoria(CategoriaCreacionDto categoriaDto);
    Task<IEnumerable<CategoriaDto>> ObtenerCategorias();
    Task<CategoriaDto?> ObtenerCategoriaPorId(int categoriaId);
    Task<bool> ActualizarCategoria(CategoriaActualizacionDto categoriaDto);
    Task<bool> EliminarCategoria(int categoriaId);
}