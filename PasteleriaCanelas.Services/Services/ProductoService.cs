using PasteleriaCanelas.Data.Context;
using PasteleriaCanelas.Domain.Entities;
using PasteleriaCanelas.Services.DTOs;
using PasteleriaCanelas.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using PasteleriaCanelas.Services.Utilities; // para crear los slug
using Microsoft.Extensions.Caching.Memory;

namespace PasteleriaCanelas.Services.Services;

// implementamos la interfaz de producto service
public class ProductoService : IProductoService
{
    //se declara variable para el contexto de la base de datos
    private readonly PasteleriaDbContext _context;
    private readonly IMemoryCache _cache;

    // Constructor | es el punto para la inyección de dependencias
    // se inyecta una instancia del contexto cuando se crea ProductoService
    public ProductoService(PasteleriaDbContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }


    // ADMINISTRADOR

    // Implementación de metodo 1 : mostrar por id
    public async Task<ProductoDetallesDto?> ObtenerProductoPorId(int productoId)
    {
        return await _context.Productos
            .AsNoTracking()
            .Include(p => p.Categoria)
            .Include(p => p.ProductoPrecios)
            .Where(p => p.ProductoId == productoId)
            .Select(producto => new ProductoDetallesDto
            {
                ProductoId = producto.ProductoId,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                ImagenUrl = producto.ImagenUrl,
                Activo = producto.Activo,
                Slug = producto.Slug,
                Categoria = new CategoriaDto
                {
                    CategoriaId = producto.Categoria.CategoriaId,
                    Nombre = producto.Categoria.Nombre,
                    Slug = producto.Categoria.Slug,
                    Descripcion = producto.Categoria.Descripcion,
                    Icono = producto.Categoria.Icono,
                    ImagenUrl = producto.Categoria.ImagenUrl,
                    Activo = producto.Categoria.Activo
                },
                ProductoPrecios = producto.ProductoPrecios.Select(pp => new ProductoPrecioDto
                {
                    ProductoPrecioId = pp.ProductoPrecioId,
                    DescripcionPrecio = pp.DescripcionPrecio,
                    Precio = pp.Precio
                }).ToList()
            })
            .FirstOrDefaultAsync();
    }


    // Implementación de método 2 : mostrar productos (activos e inactivos)
    public async Task<IEnumerable<ProductoDetallesDto>?> ObtenerTodosProductos()
    {
        return await _context.Productos
            .AsNoTracking()
            .Include(p => p.Categoria)
            .Include(p => p.ProductoPrecios)
            .Select(p => new ProductoDetallesDto
            {
                ProductoId = p.ProductoId,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                ImagenUrl = p.ImagenUrl,
                Activo = p.Activo,
                Slug = p.Slug,
                EsDeTemporada = p.EsDeTemporada,
                Categoria = new CategoriaDto
                {
                    CategoriaId = p.Categoria.CategoriaId,
                    Nombre = p.Categoria.Nombre,
                    Slug = p.Categoria.Slug,
                    Descripcion = p.Categoria.Descripcion,
                    Icono = p.Categoria.Icono,
                    ImagenUrl = p.Categoria.ImagenUrl,
                    Activo = p.Categoria.Activo
                },
                ProductoPrecios = p.ProductoPrecios.Select(pp => new ProductoPrecioDto
                {
                    ProductoPrecioId = pp.ProductoPrecioId,
                    DescripcionPrecio = pp.DescripcionPrecio,
                    Precio = pp.Precio
                }).ToList()
            })
            .ToListAsync();
    }

    // Se implementa de método 3: crear un producto
    public async Task<ProductoDetallesDto?> CrearProducto(ProductoCreacionDto productoDto)
    {
        // se valida que la categoría existe
        var categoria = await _context.Categorias.FindAsync(productoDto.CategoriaId);
        if (categoria == null)
        {
            // si la categoria no existe no se crea el producto y se manda null
            return null!;
        }

        // mapear el DTO de creación a la entidad de dominio
        var nuevoProducto = new Producto
        {
            Nombre = productoDto.Nombre,
            Descripcion = productoDto.Descripcion,
            ImagenUrl = productoDto.ImagenUrl,
            CategoriaId = productoDto.CategoriaId,
            Activo = productoDto.Activo,
            EsDeTemporada = productoDto.EsDeTemporada, // propiedad para decir si es de temporada
            Slug = SlugGenerator.GenerateSlug(productoDto.Nombre!) // generador de slug
        };


        // guardar los cambios en memoria y db
        _context.Productos.Add(nuevoProducto);
        await _context.SaveChangesAsync();


        // mapear la entidad en un DTO respuesta
        // se incluye la información para enviarla al cliente
        var productoRespuesta = new ProductoDetallesDto
        {
            ProductoId = nuevoProducto.ProductoId,
            Nombre = nuevoProducto.Nombre,
            Descripcion = nuevoProducto.Descripcion,
            ImagenUrl = nuevoProducto.ImagenUrl,
            Slug = nuevoProducto.Slug,
            EsDeTemporada = nuevoProducto.EsDeTemporada,
            Activo = nuevoProducto.Activo,
            Categoria = new CategoriaDto
            {
                CategoriaId = categoria.CategoriaId,
                Nombre = categoria.Nombre,
                Slug = categoria.Slug,
                Descripcion = categoria.Descripcion,
                Icono = categoria.Icono,
                ImagenUrl = categoria.ImagenUrl,
                Activo = categoria.Activo
            },
            // Al momento de la creación, no hay precios
            ProductoPrecios = new List<ProductoPrecioDto>()
        };

        return productoRespuesta;
    }

    // Implementación de método 4: actualizar producto
    public async Task<bool> ActualizarProducto(ProductoActualizacionDto productoDto)
    {
        // Busca el producto por su ID
        var producto = await _context.Productos.FindAsync(productoDto.ProductoId);

        // Si no encuentra retorna false
        if (producto == null)
        {
            return false;
        }

        // actualizar propiedades si se proporcionan en el DTO | si no entonces no se modifica
        if (productoDto.Nombre != null)
        {
            producto.Nombre = productoDto.Nombre;
        }
        if (productoDto.Descripcion != null)
        {
            producto.Descripcion = productoDto.Descripcion;
        }
        if (productoDto.ImagenUrl != null)
        {
            producto.ImagenUrl = productoDto.ImagenUrl;
        }
        if (productoDto.Activo.HasValue) // Comprueba si el valor booleano no es nulo
        {
            producto.Activo = productoDto.Activo.Value;
        }

        producto.CategoriaId = productoDto.CategoriaId; // para modificar categorias
        producto.EsDeTemporada = productoDto.EsDeTemporada;

        // regenerar el slug si se ha cambiado o ha pasado algo
    
        
        producto.Slug = SlugGenerator.GenerateSlug(productoDto.Nombre!);


        _context.Productos.Update(producto);
        await _context.SaveChangesAsync();
        return true;
    }

    // Implementación de método 4: eliminar producto
    public async Task<bool> EliminarProducto(int productoId)
    {
        // Carga el producto y sus precios asociados en una sola consulta
        var producto = await _context.Productos
            .Include(p => p.ProductoPrecios)
            .FirstOrDefaultAsync(p => p.ProductoId == productoId);

        // Si el producto no existe, retornamos false
        if (producto == null)
        {
            return false;
        }

        // Eliminamos los precios asociados primero. RemoveRange elimina toda la colección.
        _context.ProductoPrecios.RemoveRange(producto.ProductoPrecios);

        // Ahora, eliminamos el producto principal de forma segura.
        _context.Productos.Remove(producto);

        // Guardamos los cambios en la base de datos.
        await _context.SaveChangesAsync();
        return true;
    }


    // PRECIOS
    // Implementación de método 1: crear precio
    public async Task<ProductoPrecioDto?> CrearPrecio(ProductoPrecioCreacionDto precioDto)
    {
        // Se valida que el producto exista
        var producto = await _context.Productos.FindAsync(precioDto.ProductoId);
        if (producto == null)
        {
            return null;
        }

        var nuevoPrecio = new ProductoPrecio
        {
            DescripcionPrecio = precioDto.DescripcionPrecio,
            Precio = precioDto.Precio,
            ProductoId = precioDto.ProductoId
        };

        _context.ProductoPrecios.Add(nuevoPrecio);
        await _context.SaveChangesAsync();

        var precioRespuesta = new ProductoPrecioDto
        {
            ProductoPrecioId = nuevoPrecio.ProductoPrecioId,
            DescripcionPrecio = nuevoPrecio.DescripcionPrecio,
            Precio = nuevoPrecio.Precio
        };
        return precioRespuesta;
    }

    // Implementación de método 2: actualizar precio
    public async Task<bool> ActualizarPrecio(ProductoPrecioActualizacionDto precioDto)
    {
        var precio = await _context.ProductoPrecios.FindAsync(precioDto.ProductoPrecioId);
        if (precio == null)
        {
            return false;
        }

        precio.DescripcionPrecio = precioDto.DescripcionPrecio;
        precio.Precio = precioDto.Precio;
        
        await _context.SaveChangesAsync();
        return true;
    }

    // Implementación de método 3: eliminar producto
    public async Task<bool> EliminarPrecio(int precioId)
    {
        var precio = await _context.ProductoPrecios.FindAsync(precioId);
        if (precio == null)
        {
            return false;
        }

        _context.ProductoPrecios.Remove(precio);
        await _context.SaveChangesAsync();
        return true;
    }


    // USUARIO NORMAL

    // NUEVO: Implementación de método optimizado - Catálogo inicial completo
    // Obtiene categorías, productos y productos de temporada en una sola consulta optimizada
    // Usado por el frontend para cargar todo el catálogo en una sola petición HTTP
    public async Task<CatalogoInicialDto> ObtenerCatalogoInicial()
    {
        const string cacheKey = "CatalogoInicial";

        if (_cache.TryGetValue(cacheKey, out CatalogoInicialDto? catalogo))
        {
            if(catalogo is not null) return catalogo;
        }

        var categorias = await _context.Categorias
            .Where(c => c.Activo)
            .Select(c => new CategoriaDto
            {
                CategoriaId = c.CategoriaId,
                Nombre = c.Nombre,
                Slug = c.Slug,
                Descripcion = c.Descripcion,
                Icono = c.Icono,
                ImagenUrl = c.ImagenUrl,
                Activo = c.Activo
            })
            .ToListAsync();

        var productos = await _context.Productos
            .Where(p => p.Activo && p.Categoria.Activo)
            .Select(p => new ProductoResumenConCategoriaDto
            {
                ProductoId = p.ProductoId,
                Nombre = p.Nombre,
                Slug = p.Slug,
                Descripcion = p.Descripcion,
                ImagenUrl = p.ImagenUrl,
                EsDeTemporada = p.EsDeTemporada,
                CategoriaSlug = p.Categoria.Slug,
                ProductoPrecios = p.ProductoPrecios
                    .Select(pp => new ProductoPrecioDto
                    {
                        ProductoPrecioId = pp.ProductoPrecioId,
                        DescripcionPrecio = pp.DescripcionPrecio,
                        Precio = pp.Precio
                    }).ToList()
            })
            .AsNoTracking()
            .ToListAsync();

        var temporada = productos.Where(p => p.EsDeTemporada).ToList();

        var catalogoCompleto = new CatalogoInicialDto
        {
            Categorias = categorias,
            Productos = productos,
            Temporada = temporada
        };

        var cacheOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(5));

        _cache.Set(cacheKey, catalogoCompleto, cacheOptions);

        return catalogoCompleto;
    }

    // METODOS DE USO

    // verificar que una categoria existe
    public async Task<bool> CategoriaExiste(int categoriaId)
    {
        return await _context.Categorias.AnyAsync(c => c.CategoriaId == categoriaId);
    }
}