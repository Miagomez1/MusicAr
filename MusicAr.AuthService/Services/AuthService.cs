using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MusicAr.AuthService.Data;
using MusicAr.AuthService.Models;

namespace MusicAr.AuthService.Services
{
    public class AuthService
    {
        private readonly AuthDbContext _context;
        private readonly PasswordHasher<Usuario> _passwordHasher;

        public AuthService(AuthDbContext context)
        {
            _context = context;
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
    }
}