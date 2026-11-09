using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicAr.DTO;
using MusicAr.Web.Models;

namespace MusicAr.Web.Controllers
{
    public class PlaylistsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PlaylistsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Nueva()   // <-- ahora async Task
        {
            return View(new NuevaPlaylistViewModel { Sugeridas = await CargarSugeridasAsync() });
        }

        [HttpPost]
        [Route("playlists/crear-ajax")]
        public async Task<IActionResult> CrearAjax([FromBody] CrearPlaylistDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Nombre))
            {
<<<<<<< Updated upstream
                model.Sugeridas = await CargarSugeridasAsync();   // <-- cambió
                return View(model);
            }

            var client = _httpClientFactory.CreateClient("ApiGatewayClient");
            var dto = new CrearPlaylistDto { Nombre = model.Nombre };
=======
                return BadRequest(new { mensaje = "El nombre de la playlist es requerido." });
            }

            var client = _httpClientFactory.CreateClient("ApiGatewayClient");
            var token = User.FindFirst("TokenJwt")?.Value;
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
>>>>>>> Stashed changes

            try
            {
                var respuesta = await client.PostAsJsonAsync("api/playlists", dto);
                if (respuesta.IsSuccessStatusCode)
<<<<<<< Updated upstream
                    return RedirectToAction("Index", "Home");
=======
                {
                    // Lee el resultado (que contiene el ID generado) y lo devuelve como JSON al frontend
                    var resultado = await respuesta.Content.ReadFromJsonAsync<object>();
                    return Ok(resultado);
                }
>>>>>>> Stashed changes

                return BadRequest(new { mensaje = "No se pudo crear la playlist en el servidor." });
            }
            catch (HttpRequestException)
            {
                return StatusCode(500, new { mensaje = "Error de conexión con el servidor." });
            }
        }

        [HttpPost]
        [Route("playlists/{playlistId}/canciones")]
        public async Task<IActionResult> AgregarCancionAjax(int playlistId, [FromBody] AgregarCancionDto dto)
        {
            if (dto == null || dto.CancionId <= 0)
            {
                return BadRequest(new { mensaje = "Los datos de la canción son inválidos." });
            }

<<<<<<< Updated upstream
            model.Sugeridas = await CargarSugeridasAsync();   // <-- cambió
            return View(model);
=======
            var client = _httpClientFactory.CreateClient("ApiGatewayClient");
            var token = User.FindFirst("TokenJwt")?.Value;
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            try
            {
                // Reenvía la petición POST al microservicio a través del Gateway
                var respuesta = await client.PostAsJsonAsync($"api/playlists/{playlistId}/canciones", dto);

                if (respuesta.IsSuccessStatusCode)
                {
                    // Lee el resultado devuelto (ej. Orden de la canción) y lo retorna al script
                    var resultado = await respuesta.Content.ReadFromJsonAsync<object>();
                    return Ok(resultado);
                }

                return BadRequest(new { mensaje = "No se pudo agregar la canción a la playlist." });
            }
            catch (HttpRequestException)
            {
                return StatusCode(500, new { mensaje = "Error de conexión con el servidor." });
            }
>>>>>>> Stashed changes
        }

        private async Task<List<CancionViewModel>> CargarSugeridasAsync()
        {
            var client = _httpClientFactory.CreateClient("ApiGatewayClient");

            try
            {
                var dtos = await client.GetFromJsonAsync<List<CancionDto>>("api/canciones");

                return (dtos ?? new List<CancionDto>())
                    .Take(6)
                    .Select((c, i) => new CancionViewModel(
                        Numero: i + 1,
                        Titulo: c.Titulo,
                        Artista: c.Artista,
                        Id: c.Id))
                    .ToList();
            }
            catch (HttpRequestException)
            {
                return new List<CancionViewModel>();
            }
        }
    }
}