namespace MusicAr.Web.Models
{
    public class GeneroDetalleViewModel
    {
        public string Nombre { get; set; } = string.Empty;
        public List<CancionViewModel> Canciones { get; set; } = new();
    }
}
