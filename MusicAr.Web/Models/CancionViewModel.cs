namespace MusicAr.Web.Models
{
    public record CancionViewModel(int Numero, string Titulo, string Artista, bool Sonando = false);
}
