using Microsoft.AspNetCore.Mvc;
using MusicAr.AuthService.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MusicAr.AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FavoritosController : ControllerBase
    {
        private readonly IFavoritoService _favoritoService;

        public FavoritosController(IFavoritoService favoritoService)
        {
            _favoritoService = favoritoService;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerFavoritos()
        {
            var usuarioId = ObtenerUsuarioId();
            if (usuarioId <= 0)
            {
                return Unauthorized(new { mensaje = "Usuario no autenticado." });
            }

            var canciones = await _favoritoService.ObtenerCancionIdsFavoritasAsync(usuarioId);
            return Ok(canciones);
        }

        [HttpPost("toggle/{cancionId:int}")]
        public async Task<IActionResult> Toggle(int cancionId)
        {
            var usuarioId = ObtenerUsuarioId();
            if (usuarioId <= 0)
            {
                return Unauthorized(new { mensaje = "Usuario no autenticado." });
            }

            var esFavorito = await _favoritoService.ToggleFavoritoAsync(usuarioId, cancionId);
            return Ok(new { cancionId, esFavorito });
        }

        [HttpGet("es-favorito/{cancionId:int}")]
        public async Task<IActionResult> EsFavorito(int cancionId)
        {
            var usuarioId = ObtenerUsuarioId();
            if (usuarioId <= 0)
            {
                return Unauthorized(new { mensaje = "Usuario no autenticado." });
            }

            var esFavorito = await _favoritoService.EsFavoritoAsync(usuarioId, cancionId);
            return Ok(new { cancionId, esFavorito });
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
}
