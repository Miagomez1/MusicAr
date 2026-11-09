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
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<PlaylistCancion> PlaylistCanciones { get; set; }
        public DbSet<Favorito> Favoritos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlaylistCancion>()
                .HasKey(pc => new { pc.PlaylistId, pc.CancionId });

            modelBuilder.Entity<Favorito>()
                .HasKey(f => new { f.UsuarioId, f.CancionId });
        }
    }
}
