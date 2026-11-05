using System.ComponentModel.DataAnnotations;
namespace MusicAr.Entidades
{
    public class Artista
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; } = string.Empty;

        public ICollection<Cancion> canciones { get; set; } = new List<Cancion>();
    }
}
