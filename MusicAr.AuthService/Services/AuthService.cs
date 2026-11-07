using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MusicAr.AuthService.Data;
using MusicAr.AuthService.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MusicAr.AuthService.Services
{
    public class AuthService
    {
        private readonly AuthDbContext _context;
        private readonly PasswordHasher<Usuario> _passwordHasher;
        private readonly IConfiguration _configuration;

        public AuthService(AuthDbContext context, IConfiguration configuration)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<Usuario>();
            _configuration = configuration;
        }

        public async Task<Usuario?> BuscarUsuarioAsync(string identificador)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(
                u => u.Nombre == identificador || u.Email == identificador);
        }

        public bool VerificarPassword(Usuario usuario, string password)
        {
            var resultado = _passwordHasher.VerifyHashedPassword(
                usuario,
                usuario.Pass,
                password
            );
            return resultado == PasswordVerificationResult.Success;
        }

        public string GenerarJwtToken(Usuario usuario)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var keyString = jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Key no configurada");
            var key = Encoding.UTF8.GetBytes(keyString);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Email, usuario.Email)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(8), // El token expira en 8 horas
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}