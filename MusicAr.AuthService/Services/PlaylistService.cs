using MusicAr.AuthService.Data;
using MusicAr.AuthService.Models;
using MusicAr.DTO;

public interface IPlaylistService
{
    Task<PlaylistDto> CrearAsync(int usuarioId, CrearPlaylistDto dto);
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
}