namespace MusicAr.Web.Models
{
    public record CancionViewModel(
        int Numero,
        string Titulo,
        string Artista,
        string AudioUrl,
        string CoverImageUrl,
        bool Sonando = false
    );
}