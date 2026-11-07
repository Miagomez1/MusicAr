namespace MusicAr.Web.Models
{
    public record CancionViewModel(
        int Numero,
        string Titulo,
        string Artista,
        string AudioUrl = "",
        string? CoverImageUrl = null,
        int? DurationInSeconds = null,
        bool Sonando = false
    );
}