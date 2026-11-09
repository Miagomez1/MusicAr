using System.ComponentModel.DataAnnotations;

namespace MusicAr.Web.Models
{
    public class NuevaPlaylistViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Ponele un nombre a tu playlist")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres")]
        [Display(Name = "Nombre de la playlist")]
        public string Nombre { get; set; } = "Mi playlist n.º 1";

        // Solo para mostrar sugerencias; no viaja en el formulario
        public List<CancionViewModel> Sugeridas { get; set; } = new();
    }
}
