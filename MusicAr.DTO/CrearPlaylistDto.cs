using System.ComponentModel.DataAnnotations;

namespace MusicAr.DTO
{
    public class CrearPlaylistDto
    {
        [Required(ErrorMessage = "Ponele un nombre a tu playlist")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;
    }
}