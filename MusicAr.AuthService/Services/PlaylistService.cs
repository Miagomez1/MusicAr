using Microsoft.EntityFrameworkCore;
using MusicAr.AuthService.Data;
using MusicAr.AuthService.Models;
using MusicAr.DTO;

public interface IPlaylistService
{
    Task<PlaylistDto> CrearAsync(int usuarioId, CrearPlaylistDto dto);
    Task<PlaylistCancionDto> AgregarCancionAsync(int playlistId, int usuarioId, AgregarCancionDto dto);
}

public class PlaylistService : IPlaylistService
{
    private readonly AuthDbContext _context;

    public PlaylistService(AuthDbContext context)
    {
        _context = context;
    }

    public async Task<PlaylistDto> CrearAsync(int usuarioId, CrearPlaylistDto dto)
    {
        var playlist = new Playlist { Nombre = dto.Nombre, UsuarioId = usuarioId }; 
        _context.Playlists.Add(playlist);
        await _context.SaveChangesAsync();

        return new PlaylistDto { Id = playlist.Id, Nombre = playlist.Nombre };
    }

    public async Task<PlaylistCancionDto> AgregarCancionAsync(int playlistId, int usuarioId, AgregarCancionDto dto)
    {
        var playlist = await _context.Playlists
            .Include(p => p.Canciones)
            .FirstOrDefaultAsync(p => p.Id == playlistId && p.UsuarioId == usuarioId);

        if (playlist == null) return null;

        int siguienteOrden = playlist.Canciones.Any()
            ? playlist.Canciones.Max(c => c.Orden) + 1
            : 1;

        var playlistCancion = new PlaylistCancion
        {
            PlaylistId = playlistId,
            CancionId = dto.CancionId,
            Orden = siguienteOrden
        };

        _context.PlaylistCanciones.Add(playlistCancion);
        await _context.SaveChangesAsync();

        return new PlaylistCancionDto
        {
            CancionId = dto.CancionId,
            Orden = siguienteOrden
        };
    }
}