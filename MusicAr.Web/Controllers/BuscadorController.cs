using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MusicAr.DTO;
using MusicAr.Web.Models;
using System.Text.Json;

namespace MusicAr.Web.Controllers
{
    public class BuscadorController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMapper _mapper;

        public BuscadorController(IHttpClientFactory httpClientFactory, IMapper mapper)
        {
            _httpClientFactory = httpClientFactory;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Buscar([FromQuery] string q)
        {
            var model = new BuscadorViewModel { Q = q ?? string.Empty };

            if (string.IsNullOrWhiteSpace(model.Q))
            {
                model.Canciones = new List<CancionViewModel>();
                return View(model);
            }

            var client = _httpClientFactory.CreateClient("ApiGatewayClient");
            var response = await client.GetAsync($"api/canciones/buscar?q={Uri.EscapeDataString(model.Q)}");

            if (!response.IsSuccessStatusCode)
            {
                model.Canciones = new List<CancionViewModel>();
                return View(model);
            }

            // Habilitar insensibilidad a mayúsculas/minúsculas para deserializar el JSON de la API
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var cancionesDto = await response.Content.ReadFromJsonAsync<List<CancionDto>>(options)
                               ?? new List<CancionDto>();

            int index = 1;
            model.Canciones = cancionesDto.Select(c => new CancionViewModel(
                Numero: index++,
                Titulo: c.Titulo,
                Artista: c.Artista,
                AudioUrl: c.AudioUrl,
                CoverImageUrl: c.CoverImageUrl,
                DurationInSeconds: c.DurationInSeconds
            )).ToList();

            return View(model);
        }
    }
}
