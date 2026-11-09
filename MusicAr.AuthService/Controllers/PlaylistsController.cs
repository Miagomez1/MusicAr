using Microsoft.AspNetCore.Mvc;
using MusicAr.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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
        if (usuarioId <= 0)
        {
            return Unauthorized(new { mensaje = "Usuario no autenticado." });
        }

        var playlist = await _playlistService.CrearAsync(usuarioId, dto);
        return Ok(playlist);
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
}
