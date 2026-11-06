using Microsoft.EntityFrameworkCore;
using MusicAr.Entidades;

namespace MusicAr.Infraestructura
{
    public class CatalogoDBContext : DbContext
    { 
         public DbSet<Cancion> Canciones { get; set; } = null!;
         public DbSet<Artista> Artistas { get; set; } = null!;
         public DbSet<Genero> Generos { get; set; } = null!;

        public CatalogoDBContext(DbContextOptions<CatalogoDBContext> options) : base(options)
        { }
    }
}
