using Microsoft.EntityFrameworkCore;
using MusicAr.DTO;
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

        public async Task<List<CancionDto>> BuscarCancionesPorTextoAsync(string termino)
        {
            string filtro = termino.Trim().ToLower();

            return await Canciones
                .Include(c => c.Artista)
                .AsNoTracking()
                .Where(c => c.Titulo.ToLower().Contains(filtro) ||
                            (c.Artista != null && c.Artista.Name.ToLower().Contains(filtro)))
                .Select(c => new CancionDto
                {
                    Id = c.Id,
                    Titulo = c.Titulo,
                    DurationInSeconds = c.DurationInSeconds,
                    Artista = c.Artista != null ? c.Artista.Name : string.Empty,
                    CoverImageUrl = c.CoverImageUrl,
                    AudioUrl = $"/api/canciones/{c.Id}/audio"
                })
                .ToListAsync();
        }
    }
}
