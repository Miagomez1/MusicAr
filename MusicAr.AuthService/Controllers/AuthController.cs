using Microsoft.AspNetCore.Mvc;
using MusicAr.AuthService.DTOs;
using MusicAr.AuthService.Services;

namespace MusicAr.AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly MusicAr.AuthService.Services.AuthService _authService;

        public AuthController(MusicAr.AuthService.Services.AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (string.IsNullOrWhiteSpace(model.Identificador) || string.IsNullOrWhiteSpace(model.Password))
            {
                return BadRequest(new { mensaje = "El identificador y la contraseña son obligatorios." });
            }

            var usuario = await _authService.BuscarUsuarioAsync(model.Identificador);

            if (usuario == null || !_authService.VerificarPassword(usuario, model.Password))
            {
                return Unauthorized(new { mensaje = "Credenciales inválidas." });
            }

            var token = _authService.GenerarJwtToken(usuario);

            return Ok(new
            {
                token = token,
                usuario = new { usuario.Id, usuario.Nombre, usuario.Email }
            });
        }
    }
}