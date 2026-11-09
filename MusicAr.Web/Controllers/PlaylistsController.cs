using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicAr.DTO;
using MusicAr.Web.Models;
using System.Net.Http.Headers;

[Authorize]
public class PlaylistsController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public PlaylistsController(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;

    [HttpGet]
    public async Task<IActionResult> Nueva()
    {
        // Depuración rápida: Ver todas las claims disponibles
        var claimsDisponibles = User.Claims.Select(c => $"{c.Type}: {c.Value}").ToList();

        var client = _httpClientFactory.CreateClient("ApiGatewayClient");

        // Asegúrate de usar exactamente el mismo nombre de claim con el que guardaste el token al hacer login
        var token = User.FindFirst("Jwt")?.Value;

        if (string.IsNullOrEmpty(token))
        {
            // Esto te dirá qué claims sí tiene guardadas la cookie actualmente
            return Content($"Error: No se encontró el token 'TokenJwt'. Claims actuales en la cookie: {string.Join(" | ", claimsDisponibles)}");
        }

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var dto = new CrearPlaylistDto { Nombre = "Mi playlist n.º 1" };

        try
        {
            var respuesta = await client.PostAsJsonAsync("api/playlists", dto);

            // Si la API falla, mostramos el código de error y el texto para saber qué pasó
            if (!respuesta.IsSuccessStatusCode)
            {
                var errorContent = await respuesta.Content.ReadAsStringAsync();
                return Content($"Error del Backend/Gateway ({respuesta.StatusCode}): {errorContent}");
            }

            var playlistCreada = await respuesta.Content.ReadFromJsonAsync<PlaylistDto>();

            if (playlistCreada == null || playlistCreada.Id == 0)
            {
                return Content("Error: La API respondió OK pero el ID de la playlist llegó vacío o en 0.");
            }

            return Redirect($"/Playlists/Editar/{playlistCreada.Id}");
        }
        catch (Exception ex)
        {
            return Content($"Excepción de red al conectar con el Gateway: {ex.Message}");
        }
    }

    [HttpGet("Playlists/Editar/{id}")]
    public async Task<IActionResult> Editar(int id)
    {
        var sugeridas = await CargarSugeridasAsync();
        var viewModel = new NuevaPlaylistViewModel
        {
            Id = id,
            Sugeridas = sugeridas
        };

        return View("Editar", viewModel);
    }

    private async Task<List<CancionViewModel>> CargarSugeridasAsync()
    {
        var client = _httpClientFactory.CreateClient("ApiGatewayClient");
        try
        {
            var dtos = await client.GetFromJsonAsync<List<CancionDto>>("api/canciones");
            return (dtos ?? new List<CancionDto>()).Take(6).Select((c, i) => new CancionViewModel(i + 1, c.Titulo, c.Artista, c.Id.ToString())).ToList();
        }
        catch { return new List<CancionViewModel>(); }
    }
}