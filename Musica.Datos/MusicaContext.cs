using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Musica.Entidades;

namespace Musica.Datos;

public partial class MusicaContext : DbContext
{
    public MusicaContext(DbContextOptions<MusicaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Album> Albums { get; set; }
    public virtual DbSet<Usuario> Usuarios { get; set; }
    public virtual DbSet<Cancion> Cancions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Album>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("Album");

            entity.Property(e => e.Artista).HasMaxLength(100);
            entity.Property(e => e.Genero).HasMaxLength(50);
            entity.Property(e => e.Titulo).HasMaxLength(100);
        });

        modelBuilder.Entity<Cancion>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("Cancion");

            entity.Property(e => e.Titulo).HasMaxLength(100);

            entity.HasOne(d => d.Album).WithMany(p => p.Cancions)
                .HasForeignKey(d => d.AlbumId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasIndex(e => e.Correo, "UQ_Usuarios_Correo").IsUnique();
            entity.Property(e => e.Correo).HasMaxLength(100);
            entity.Property(e => e.ClaveHash).HasMaxLength(100);
            entity.Property(e => e.Rol).HasMaxLength(30);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}