using System.ComponentModel.DataAnnotations;

namespace MusicAr.Web.Models
{
    public class BuscadorViewModel
    {
        [Required]
        public string Q { get; set; } = string.Empty;

        public List<CancionViewModel>? Canciones { get; set; } = [];
    }
}
