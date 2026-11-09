<<<<<<< Updated upstream
﻿using Microsoft.AspNetCore.Mvc;
=======
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
>>>>>>> Stashed changes
using MusicAr.DTO;

[Authorize]
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

<<<<<<< Updated upstream
    // TEMPORAL: cuando exista el login con JWT, este valor sale del token
    private int ObtenerUsuarioId() => 1;
=======
    [HttpPost("{id}/canciones")]
    public async Task<IActionResult> AgregarCancion(int id, [FromBody] AgregarCancionDto dto)
    {
        var usuarioId = ObtenerUsuarioId();
        if (usuarioId <= 0)
        {
            return Unauthorized(new { mensaje = "Usuario no autenticado." });
        }

        var resultado = await _playlistService.AgregarCancionAsync(id, usuarioId, dto);
        if (resultado == null)
        {
            return NotFound(new { mensaje = "Playlist no encontrada o no pertenece al usuario." });
        }

        return Ok(resultado);
    }

    private int ObtenerUsuarioId()
    {
        var claimValue = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (int.TryParse(claimValue, out var idFromClaim) && idFromClaim > 0)
        {
            return idFromClaim;
        }

        var authHeader = Request.Headers.Authorization.ToString();
        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            var token = authHeader["Bearer ".Length..].Trim();
            var handler = new JwtSecurityTokenHandler();
            if (handler.CanReadToken(token))
            {
                var jwt = handler.ReadJwtToken(token);
                var idClaim = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type == "nameid" || c.Type == "sub");
                if (idClaim != null && int.TryParse(idClaim.Value, out var id) && id > 0)
                {
                    return id;
                }
            }
        }

        return 0;
    }
>>>>>>> Stashed changes
}