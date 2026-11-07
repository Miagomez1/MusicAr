using MusicAr.DTO;
using MusicAr.Infraestructura;
using Microsoft.EntityFrameworkCore;
namespace MusicAr.Logica
{
    public class CatalogoService : ICatalogoService
    {
        private readonly CatalogoDBContext _context;
        public CatalogoService(CatalogoDBContext context) { 
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
    }

    public interface ICatalogoService
    {
        Task<List<GeneroDto>> ObtenerGenerosAsync();
    }
}
