using Microsoft.EntityFrameworkCore;
using MusicAr.AuthService.Data;
using MusicAr.AuthService.Models;

namespace MusicAr.AuthService.Services
{
    public interface IFavoritoService
    {
        Task<List<int>> ObtenerCancionIdsFavoritasAsync(int usuarioId);
        Task<bool> ToggleFavoritoAsync(int usuarioId, int cancionId);
        Task<bool> EsFavoritoAsync(int usuarioId, int cancionId);
    }

    public class FavoritoService : IFavoritoService
    {
        private readonly AuthDbContext _context;

        public FavoritoService(AuthDbContext context)
        {
            _context = context;
        }

        public async Task<List<int>> ObtenerCancionIdsFavoritasAsync(int usuarioId)
        {
            return await _context.Favoritos
                .AsNoTracking()
                .Where(f => f.UsuarioId == usuarioId)
                .Select(f => f.CancionId)
                .ToListAsync();
        }

        public async Task<bool> ToggleFavoritoAsync(int usuarioId, int cancionId)
        {
            var existente = await _context.Favoritos
                .FirstOrDefaultAsync(f => f.UsuarioId == usuarioId && f.CancionId == cancionId);

            if (existente != null)
            {
                _context.Favoritos.Remove(existente);
                await _context.SaveChangesAsync();
                return false;
            }

            var nuevo = new Favorito
            {
                UsuarioId = usuarioId,
                CancionId = cancionId
            };

            _context.Favoritos.Add(nuevo);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EsFavoritoAsync(int usuarioId, int cancionId)
        {
            return await _context.Favoritos
                .AsNoTracking()
                .AnyAsync(f => f.UsuarioId == usuarioId && f.CancionId == cancionId);
        }
    }
}
