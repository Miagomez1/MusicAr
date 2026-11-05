using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MusicAr.AuthService.Models;

namespace MusicAr.AuthService.Data
{
    public static class AuthInicio
    {
        public static void Inicializar(AuthDbContext context)
        {
            if (context.Usuarios.Any())
            {
                return;
            }

            var usuario = new Usuario
            {
                Nombre = "admin",
                Email = "admin@musicar.com"
            };

            var hasher = new PasswordHasher<Usuario>();

            usuario.Pass = hasher.HashPassword(usuario, "MusicAr123!");

            context.Usuarios.Add(usuario);
            context.SaveChanges();
        }
    }
}