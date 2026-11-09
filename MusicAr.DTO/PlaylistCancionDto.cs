namespace MusicAr.DTO
{
    public class PlaylistCancionDto
    {
        public int CancionId { get; set; }
        public int Orden { get; set; }
        public CancionDto Cancion { get; set; } // Opcional: para traer título, artista, etc.
    }
}