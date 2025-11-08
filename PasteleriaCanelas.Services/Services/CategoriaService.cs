﻿using PasteleriaCanelas.Data.Context;
using PasteleriaCanelas.Domain.Entities;
using PasteleriaCanelas.Services.DTOs;
using PasteleriaCanelas.Services.Interfaces;
using PasteleriaCanelas.Services.Utilities;
using Microsoft.EntityFrameworkCore;

namespace PasteleriaCanelas.Services;

public class CategoriaService : ICategoriaService
{
    //se declara variable para el contexto de la base de datos
    private readonly PasteleriaDbContext _context;

    // Constructor, este es el punto para la inyección de dependencias
    // se inyecta una instancia del contexto cuando se crea ProductoService
    public CategoriaService(PasteleriaDbContext context)
    {
        _context = context;
    }

public async Task<CategoriaDto?> CrearCategoria(CategoriaCreacionDto categoriaDto)
{
    var nuevaCategoria = new Categoria
    {
        Nombre = categoriaDto.Nombre,
        Descripcion = categoriaDto.Descripcion,
        ImagenUrl = categoriaDto.ImagenUrl,
        Icono = categoriaDto.Icono,
        Slug = SlugGenerator.GenerateSlug(categoriaDto.Nombre!),
        Activo = true
    };

    _context.Categorias.Add(nuevaCategoria);
    await _context.SaveChangesAsync();

    return new CategoriaDto
    {
        CategoriaId = nuevaCategoria.CategoriaId,
        Nombre = nuevaCategoria.Nombre,
        Slug = nuevaCategoria.Slug,
        ImagenUrl = nuevaCategoria.ImagenUrl,
        Descripcion = nuevaCategoria.Descripcion,
        Icono = nuevaCategoria.Icono,
        Activo = nuevaCategoria.Activo
    };
}

    public async Task<IEnumerable<CategoriaDto>> ObtenerCategorias()
    {
        return await _context.Categorias
            .AsNoTracking()
            .Select(c => new CategoriaDto
            {
                CategoriaId = c.CategoriaId,
                Nombre = c.Nombre,
                Slug = c.Slug,
                Descripcion = c.Descripcion,
                ImagenUrl = c.ImagenUrl,
                Icono = c.Icono,
                Activo = c.Activo
            })
            .ToListAsync();
    }

    public async Task<CategoriaDto?> ObtenerCategoriaPorId(int categoriaId)
    {
        return await _context.Categorias
            .AsNoTracking()
            .Where(c => c.CategoriaId == categoriaId)
            .Select(c => new CategoriaDto
            {
                CategoriaId = c.CategoriaId,
                Nombre = c.Nombre,
                Slug = c.Slug,
                Descripcion = c.Descripcion,
                ImagenUrl = c.ImagenUrl,
                Icono = c.Icono,
                Activo = c.Activo
            })
            .FirstOrDefaultAsync();
    }

    public async Task<bool> ActualizarCategoria(CategoriaActualizacionDto categoriaDto)
    {
        var categoria = await _context.Categorias.FindAsync(categoriaDto.CategoriaId);
        if (categoria == null)
        {
            return false;
        }

        categoria.Nombre = categoriaDto.Nombre;
        categoria.Descripcion = categoriaDto.Descripcion;
        categoria.ImagenUrl = categoriaDto.ImagenUrl;
        categoria.Icono = categoriaDto.Icono;
        categoria.Activo = categoriaDto.Activo;
        categoria.Slug = SlugGenerator.GenerateSlug(categoriaDto.Nombre!);

        _context.Categorias.Update(categoria);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> EliminarCategoria(int categoriaId)
    {
        var categoria = await _context.Categorias.FindAsync(categoriaId);
        if (categoria == null)
        {
            return false;
        }

        // Verificamos si hay algún producto asociado a esta categoría.
        var tieneProductos = await _context.Productos.AnyAsync(p => p.CategoriaId == categoriaId);

        // Si la categoría tiene productos, devolvemos false.
        if (tieneProductos)
        {
            return false;
        }

        // Si no tiene productos, procedemos a eliminarla de forma segura.
        _context.Categorias.Remove(categoria);
        await _context.SaveChangesAsync();
        return true;
    }


}
