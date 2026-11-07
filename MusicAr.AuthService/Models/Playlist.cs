namespace MusicAr.AuthService.Models
{
    public class Playlist
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;

        public int UsuarioId { get; set; }        // el id del usuario dueño
        public Usuario? Usuario { get; set; }   // la propiedad de navegación

        public List<PlaylistCancion> Canciones { get; set; } = new();
    }
}