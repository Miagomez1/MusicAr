namespace MusicAr.AuthService.Models
{
    public class Favorito
    {
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
        public int CancionId { get; set; }
    }
}
