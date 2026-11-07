using Microsoft.AspNetCore.Mvc;
using MusicAr.AuthService.DTOs;
using MusicAr.AuthService.Services;

namespace MusicAr.AuthService.Controllers
{
   
        [ApiController]
        [Route("api/[controller]")]
        public class AuthController : ControllerBase {
        private readonly MusicAr.AuthService.Services.AuthService _authService;

        public AuthController(MusicAr.AuthService.Services.AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            var usuario = await _authService.BuscarUsuarioAsync(request.Identificador);

            if (usuario == null)
            {
                return Unauthorized("Usuario o contraseña incorrectos.");
            }

            bool passwordValida = _authService.VerificarPassword(usuario, request.Password);

            if (!passwordValida)
            {
                return Unauthorized("Usuario o contraseña incorrectos.");
            }

            var token = _authService.GenerarToken(usuario);

            return Ok(new
            {
                mensaje = "Inicio de sesión correcto.",
                token = token
            });
        }
    }
}
    
