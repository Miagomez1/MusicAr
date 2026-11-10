using MusicAr.DTO;
using MusicAr.Infraestructura;
using Microsoft.EntityFrameworkCore;


namespace MusicAr.Logica
{
    public class CatalogoService : ICatalogoService
    {
        private readonly CatalogoDBContext _context;

        public CatalogoService(CatalogoDBContext context)
        {
            _context = context;
        }

        public async Task<List<GeneroDto>> ObtenerGenerosAsync()
        {
            return await _context.Generos
                .Select(g => new GeneroDto
                {
                    Id = g.Id,
                    Nombre = g.Nombre
                })
                .ToListAsync();
        }

        public async Task<List<CancionDto>> ObtenerCancionesPorGeneroAsync(int generoId)
        {
            return await _context.Canciones
                .AsNoTracking()
                .Where(c => c.GeneroId == generoId)
                .Select(c => new CancionDto
                {
                    Id = c.Id,
                    Titulo = c.Titulo,
                    DurationInSeconds = c.DurationInSeconds,
                    AudioUrl = $"/api/canciones/{c.Id}/audio",
                    CoverImageUrl = c.CoverImageUrl,
                    Genero = c.Genero != null ? c.Genero.Nombre : string.Empty,
                    Artista = c.Artista != null ? c.Artista.Name : string.Empty
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<CancionDto>> BuscarCancionesAsync(string termino)
        {
            return await _context.BuscarCancionesPorTextoAsync(termino);
        }

       
    }

    public interface ICatalogoService
    {
        Task<List<GeneroDto>> ObtenerGenerosAsync();
        Task<List<CancionDto>> ObtenerCancionesPorGeneroAsync(int generoId);
        Task<IEnumerable<CancionDto>> BuscarCancionesAsync(string termino);


    }
}
