using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicAr.DTO;
using MusicAr.Web.Models;
using System.Net.Http.Headers;

namespace MusicAr.Web.Controllers
{
    [Authorize]
    public class PlaylistsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PlaylistsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Nueva()
        {
            return View(new NuevaPlaylistViewModel { Sugeridas = await CargarSugeridasAsync() });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Nueva(NuevaPlaylistViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Sugeridas = await CargarSugeridasAsync();
                return View(model);
            }

            var client = _httpClientFactory.CreateClient("ApiGatewayClient");
            var token = User.FindFirst("TokenJwt")?.Value;
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var dto = new CrearPlaylistDto { Nombre = model.Nombre };

            try
            {
                var respuesta = await client.PostAsJsonAsync("api/playlists", dto);
                if (respuesta.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "No se pudo crear la playlist.");
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(string.Empty, "No se pudo conectar con el servidor.");
            }

            model.Sugeridas = await CargarSugeridasAsync();
            return View(model);
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
