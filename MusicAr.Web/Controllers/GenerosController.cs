using Microsoft.AspNetCore.Mvc;
using MusicAr.DTO;
using MusicAr.Web.Models;

namespace MusicAr.Web.Controllers
{
    public class GenerosController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GenerosController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Detalle(int id)
        {
            var client = _httpClientFactory.CreateClient("ApiGatewayClient");

            try
            {
                var generos = await client.GetFromJsonAsync<List<GeneroDto>>("api/canciones/generos");
                var generoActual = generos?.FirstOrDefault(g => g.Id == id);

                var cancionesDto = await client.GetFromJsonAsync<List<CancionDto>>($"api/canciones/genero/{id}");

                int index = 1;
                var cancionesViewModel = (cancionesDto ?? new List<CancionDto>()).Select(c => new CancionViewModel(
                    Numero: index++,
                    Titulo: c.Titulo,
                    Artista: c.Artista,
                    AudioUrl: c.AudioUrl,
                    CoverImageUrl: c.CoverImageUrl,
                    DurationInSeconds: c.DurationInSeconds
                )).ToList();

                var vm = new GeneroDetalleViewModel
                {
                    Nombre = generoActual?.Nombre ?? "Género",
                    Canciones = cancionesViewModel
                };

                return View(vm);
            }
            catch (Exception)
            {
                return View(new GeneroDetalleViewModel
                {
                    Nombre = "Género",
                    Canciones = new List<CancionViewModel>()
                });
            }
        }
    }
}