using Microsoft.EntityFrameworkCore;
using MusicAr.AuthService.Data;
using MusicAr.AuthService.Models;
using MusicAr.DTO;

public interface IPlaylistService
{
    Task<PlaylistDto> CrearAsync(int usuarioId, CrearPlaylistDto dto);
    Task<bool> ActualizarNombreAsync(int id, int usuarioId, string nombre);
    Task<bool> AgregarCancionAsync(int playlistId, int cancionId);
    Task<List<PlaylistDto>> ObtenerPorUsuarioAsync(int usuarioId);
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
        var playlist = new Playlist
        {
            Nombre = string.IsNullOrWhiteSpace(dto.Nombre) ? "Mi playlist n.º 1" : dto.Nombre,
            UsuarioId = usuarioId
        };

        _context.Playlists.Add(playlist);
        await _context.SaveChangesAsync();

        return new PlaylistDto { Id = playlist.Id, Nombre = playlist.Nombre };
    }

    public async Task<bool> ActualizarNombreAsync(int id, int usuarioId, string nombre)
    {
        var playlist = await _context.Playlists
            .FirstOrDefaultAsync(p => p.Id == id && p.UsuarioId == usuarioId);

        if (playlist == null) return false;

        playlist.Nombre = string.IsNullOrWhiteSpace(nombre) ? "Mi playlist n.º 1" : nombre.Trim();
        _context.Playlists.Update(playlist);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> AgregarCancionAsync(int playlistId, int cancionId)
    {
        var playlistExiste = await _context.Playlists.AnyAsync(p => p.Id == playlistId);
        if (!playlistExiste) return false;

        var yaExiste = await _context.PlaylistCanciones
            .AnyAsync(pc => pc.PlaylistId == playlistId && pc.CancionId == cancionId);

        if (yaExiste) return true;

        _context.PlaylistCanciones.Add(new PlaylistCancion { PlaylistId = playlistId, CancionId = cancionId });
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<List<PlaylistDto>> ObtenerPorUsuarioAsync(int usuarioId)
    {
        return await _context.Playlists
            .Where(p => p.UsuarioId == usuarioId)
            .Select(p => new PlaylistDto { Id = p.Id, Nombre = p.Nombre })
            .ToListAsync();
    }
}