using Microsoft.EntityFrameworkCore;
using MusicAr.Entidades; 
using MusicAr.Infraestructura.DTOs;
using System.Net.Http.Json;

namespace MusicAr.Infraestructura.Services;

public class CatalogSeederService
{
    private readonly CatalogoDBContext _context;
    private readonly HttpClient _httpClient;

    public CatalogSeederService(CatalogoDBContext context, HttpClient httpClient)
    {
        _context = context;
        _httpClient = httpClient;
    }

    public async Task SeedByArtistNameAsync(string nombreArtista, string generoPorDefecto)
    {
        // 1. Limpiar espacios en blanco de los parámetros de entrada
        nombreArtista = nombreArtista.Trim();
        generoPorDefecto = generoPorDefecto.Trim();

        // 2. Buscar o crear el Artista en la base local
        var artistaExistente = await _context.Artistas
            .FirstOrDefaultAsync<Artista>(a => a.Name != null && a.Name.ToLower() == nombreArtista.ToLower());

        if (artistaExistente == null)
        {
            artistaExistente = new Artista { Name = nombreArtista };
            _context.Artistas.Add(artistaExistente);
            await _context.SaveChangesAsync();
        }

        // 3. Buscar o crear el Género en la base local
        var generoEntity = await _context.Generos
            .FirstOrDefaultAsync<Genero>(g => g.Nombre != null && g.Nombre.ToLower() == generoPorDefecto.ToLower());

        if (generoEntity == null)
        {
            generoEntity = new Genero { Nombre = generoPorDefecto };
            _context.Generos.Add(generoEntity);
            await _context.SaveChangesAsync();
        }

        // 4. Buscar el ID oficial del artista en Deezer
        var searchArtistUrl = $"https://api.deezer.com/search/artist?q=\"{Uri.EscapeDataString(nombreArtista)}\"";
        var artistSearchResponse = await _httpClient.GetFromJsonAsync<DeezerArtistSearchResponse>(searchArtistUrl);

        var deezerArtist = artistSearchResponse?.Data?.FirstOrDefault();
        if (deezerArtist == null) return;

        // 5. Obtener el Top 10 de canciones oficiales de ese ID de artista en Deezer
        var topTracksUrl = $"https://api.deezer.com/artist/{deezerArtist.Id}/top?limit=10";
        var topTracksResponse = await _httpClient.GetFromJsonAsync<DeezerTrackResponse>(topTracksUrl);

        if (topTracksResponse?.Data == null || !topTracksResponse.Data.Any()) return;

        // 6. Recorrer la respuesta e insertar las canciones en la base de datos
        foreach (var item in topTracksResponse.Data)
        {
            if (string.IsNullOrWhiteSpace(item.Title)) continue;

            // Evitar duplicados por título y artista en la BD
            bool cancionExiste = await _context.Canciones
                .AnyAsync<Cancion>(c => c.Titulo != null && c.Titulo.ToLower() == item.Title.ToLower() && c.ArtistaId == artistaExistente.Id);

            if (cancionExiste) continue;

            var nuevaCancion = new Cancion
            {
                Titulo = item.Title,
                DurationInSeconds = item.Duration,
                // Asigna la URL del preview MP3 de 30 segundos directo de Deezer (o fallback en caso de nulo)
                AudioUrl = !string.IsNullOrWhiteSpace(item.Preview)
                    ? item.Preview
                    : "https://upload.wikimedia.org/wikipedia/commons/c/c8/Example.ogg",
                CoverImageUrl = item.Album?.CoverMedium ?? "https://via.placeholder.com/300",
                ArtistaId = artistaExistente.Id,
                GeneroId = generoEntity.Id
            };

            _context.Canciones.Add(nuevaCancion);
        }

        // Guardar de un solo tiro todas las canciones procesadas del artista
        await _context.SaveChangesAsync();
    }
}