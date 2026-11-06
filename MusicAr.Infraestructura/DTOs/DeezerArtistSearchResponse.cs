using System.Text.Json.Serialization;

namespace MusicAr.Infraestructura.DTOs;

// DTO para buscar el ID oficial del artista en Deezer
public class DeezerArtistSearchResponse
{
    [JsonPropertyName("data")]
    public List<DeezerArtistItem>? Data { get; set; }
}

public class DeezerArtistItem
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}

// DTO para obtener el Top de canciones de un artista (incluye el preview MP3)
public class DeezerTrackResponse
{
    [JsonPropertyName("data")]
    public List<DeezerTrack>? Data { get; set; }
}

public class DeezerTrack
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("duration")]
    public int Duration { get; set; }

    [JsonPropertyName("preview")]
    public string? Preview { get; set; }

    [JsonPropertyName("artist")]
    public DeezerArtist? Artist { get; set; }

    [JsonPropertyName("album")]
    public DeezerAlbum? Album { get; set; }
}

public class DeezerArtist
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}

public class DeezerAlbum
{
    [JsonPropertyName("cover_medium")]
    public string? CoverMedium { get; set; }
}