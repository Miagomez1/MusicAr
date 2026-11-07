using Microsoft.AspNetCore.Mvc;
using MusicAr.DTO;

[ApiController]
[Route("api/[controller]")]
public class PlaylistsController : ControllerBase
{
    private readonly IPlaylistService _playlistService;

    public PlaylistsController(IPlaylistService playlistService)
    {
        _playlistService = playlistService;
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CrearPlaylistDto dto)
    {
        var usuarioId = ObtenerUsuarioId();
        var playlist = await _playlistService.CrearAsync(usuarioId, dto);
        return Ok(playlist);
    }

    // TEMPORAL: cuando exista el login con JWT, este valor sale del token
    private int ObtenerUsuarioId() => 1;
}