namespace MusicAr.AuthService.Models
{
    public class PlaylistCancion
    {
        public int PlaylistId { get; set; }        // clave foránea hacia Playlist
        public Playlist? Playlist { get; set; }

        public int CancionId { get; set; }     // id de la canción en el Catálogo
    }
}