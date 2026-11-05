using Microsoft.EntityFrameworkCore;
using MusicAr.AuthService.Models;

namespace MusicAr.AuthService.Data
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
    }
}
