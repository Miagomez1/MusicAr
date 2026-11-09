namespace MusicAr.AuthService.Models
{
    public class PlaylistCancion
    {
        public int PlaylistId { get; set; }
        public Playlist Playlist { get; set; }

        public int CancionId { get; set; } // Solo almacenamos el ID de la canción
        public int Orden { get; set; }     // Para mantener la secuencia en el mini-player
    }
}