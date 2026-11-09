using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicAr.DTO;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PlaylistsController : ControllerBase
{
    private readonly IPlaylistService _playlistService;

    public PlaylistsController(IPlaylistService playlistService) => _playlistService = playlistService;

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var usuarioId = ObtenerUsuarioId();
        if (usuarioId <= 0) return Unauthorized();
        return Ok(await _playlistService.ObtenerPorUsuarioAsync(usuarioId));
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CrearPlaylistDto dto)
    {
        var usuarioId = ObtenerUsuarioId();
        if (usuarioId <= 0) return Unauthorized();
        var playlist = await _playlistService.CrearAsync(usuarioId, dto);
        return Ok(playlist);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] CrearPlaylistDto dto)
    {
        var usuarioId = ObtenerUsuarioId();
        if (usuarioId <= 0) return Unauthorized();
        var ok = await _playlistService.ActualizarNombreAsync(id, usuarioId, dto.Nombre);
        if (!ok) return BadRequest();
        return Ok();
    }

    [HttpPost("{playlistId}/canciones/{cancionId}")]
    public async Task<IActionResult> AgregarCancion(int playlistId, int cancionId)
    {
        var ok = await _playlistService.AgregarCancionAsync(playlistId, cancionId);
        if (!ok) return BadRequest();
        return Ok();
    }

    private int ObtenerUsuarioId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(claim, out var id) ? id : 0;
    }
}