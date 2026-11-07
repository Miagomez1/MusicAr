using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MusicAr.AuthService.Data;
using MusicAr.AuthService.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

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
            _configuration = configuration;
            _passwordHasher = new PasswordHasher<Usuario>();
        }

        public async Task<Usuario?> BuscarUsuarioAsync(string identificador)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync
            (u => u.Nombre == identificador || u.Email == identificador);

            return usuario;
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
   
        public string GenerarToken(Usuario usuario)
        {
            var claveSecreta = _configuration["JwtSettings:SecretKey"];
            var issuer = _configuration["JwtSettings:Issuer"];
            var audience = _configuration["JwtSettings:Audience"];

            if (string.IsNullOrEmpty(claveSecreta) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
            {
                throw new InvalidOperationException("La configuración JWT está incompleta.");
            }

            var clave = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(claveSecreta)
            );

            var credenciales = new SigningCredentials(
                clave,
                SecurityAlgorithms.HmacSha256
            );

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Name, usuario.Nombre),
            new Claim(ClaimTypes.Email, usuario.Email)
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credenciales
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
