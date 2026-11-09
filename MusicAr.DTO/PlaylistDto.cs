namespace MusicAr.DTO
{
    public class PlaylistDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<CancionDto> Canciones { get; set; } = new List<CancionDto>();
    }
}