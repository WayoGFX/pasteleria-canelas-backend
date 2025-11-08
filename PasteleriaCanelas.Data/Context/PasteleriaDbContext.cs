using Microsoft.EntityFrameworkCore;
using PasteleriaCanelas.Domain.Entities;

// clase contexto de la base de datos
namespace PasteleriaCanelas.Data.Context;

public partial class PasteleriaDbContext : DbContext
{
    public PasteleriaDbContext()
    {
    }

    public PasteleriaDbContext(DbContextOptions<PasteleriaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<ProductoPrecio> ProductoPrecios { get; set; }

    // configuración para las entidades
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.CategoriaId).HasName("PK__Categori__F353C1C5DDF7CA49");

            entity.Property(e => e.CategoriaId).HasColumnName("CategoriaID");
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Descripcion).HasMaxLength(500);
            entity.Property(e => e.ImagenUrl).HasMaxLength(500);
            entity.Property(e => e.Icono).HasMaxLength(100);
            entity.Property(e => e.Slug).HasMaxLength(150).IsRequired();
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.ProductoId).HasName("PK__Producto__A430AE83F7DC6428");

            entity.Property(e => e.ProductoId).HasColumnName("ProductoID");
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.CategoriaId).HasColumnName("CategoriaID");
            entity.Property(e => e.ImagenUrl).HasMaxLength(500);
            entity.Property(e => e.Nombre).HasMaxLength(150);
            entity.Property(e => e.Slug)
                .HasMaxLength(250)
                .HasDefaultValue("");
            entity.Property(e => e.EsDeTemporada).HasDefaultValue(false);

            entity.HasOne(d => d.Categoria).WithMany(p => p.Productos)
                .HasForeignKey(d => d.CategoriaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Productos__Categ__398D8EEE");
        });

        modelBuilder.Entity<ProductoPrecio>(entity =>
        {
            entity.HasKey(e => e.ProductoPrecioId).HasName("PK__Producto__9B63E9A7233E63A6");

            entity.Property(e => e.ProductoPrecioId).HasColumnName("ProductoPrecioID");
            entity.Property(e => e.DescripcionPrecio).HasMaxLength(100);
            entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ProductoId).HasColumnName("ProductoID");

            entity.HasOne(d => d.Producto).WithMany(p => p.ProductoPrecios)
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductoP__Produ__3D5E1FD2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}