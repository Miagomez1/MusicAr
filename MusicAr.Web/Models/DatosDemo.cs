namespace MusicAr.Web.Models
{
    // Datos de prueba para poder armar y ver las vistas.
    // Se reemplazan por llamadas al CatalogService (a través del ApiGateway).
    public static class DatosDemo
    {
        public static List<GeneroViewModel> Generos { get; } = new()
        {
            new(1, "Rock nacional", "plum"),
            new(2, "Cumbia", "lime"),
            new(3, "Folklore", "gray"),
            new(4, "Tango", "lime"),
            new(5, "Jazz", "plum"),
            new(6, "Pop", "gray"),
        };

        public static List<CancionViewModel> Canciones(int cantidad = 5) =>
            Enumerable.Range(1, cantidad)
                .Select(i => new CancionViewModel(i, $"Canción de ejemplo {i}", "Artista", Sonando: i == 2))
                .ToList();

        public static List<CancionViewModel> Cola(int cantidad = 4) =>
            Enumerable.Range(3, cantidad)
                .Select(i => new CancionViewModel(i, $"Canción de ejemplo {i}", "Artista"))
                .ToList();
    }
}
