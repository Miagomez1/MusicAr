using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicAr.DTO;
using MusicAr.Web.Models;
using System.Net.Http.Headers;

namespace MusicAr.Web.Controllers
{
    public class FavoritosController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public FavoritosController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("ApiGatewayClient");
            var token = User.FindFirst("TokenJwt")?.Value;
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var idsFavoritos = new List<int>();
            try
            {
                var resp = await client.GetFromJsonAsync<List<int>>("api/favoritos");
                if (resp != null)
                {
                    idsFavoritos = resp;
                }
            }
            catch (HttpRequestException)
            {
            }

            var todas = new List<CancionDto>();
            try
            {
                var dtos = await client.GetFromJsonAsync<List<CancionDto>>("api/canciones");
                if (dtos != null)
                {
                    todas = dtos;
                }
            }
            catch (HttpRequestException)
            {
            }

            var setIds = new HashSet<int>(idsFavoritos);
            var favoritosModel = todas
                .Where(c => setIds.Contains(c.Id))
                .Select((c, i) => new CancionViewModel(
                    Numero: i + 1,
                    Titulo: c.Titulo,
                    Artista: c.Artista,
                    Id: c.Id,
                    CoverImageUrl: c.CoverImageUrl,
                    AudioUrl: c.AudioUrl))
                .ToList();

            return View(favoritosModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Toggle(int id)
        {
            var client = _httpClientFactory.CreateClient("ApiGatewayClient");
            var token = User.FindFirst("TokenJwt")?.Value;
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            try
            {
                var resp = await client.PostAsync($"api/favoritos/toggle/{id}", null);
                if (resp.IsSuccessStatusCode)
                {
                    var resultado = await resp.Content.ReadFromJsonAsync<ToggleFavoritoResultadoDto>();
                    return Json(new { success = true, esFavorito = resultado?.EsFavorito ?? false });
                }

                return StatusCode((int)resp.StatusCode);
            }
            catch (HttpRequestException)
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Ids()
        {
            if (User.Identity?.IsAuthenticated != true)
            {
                return Json(new int[0]);
            }

            var client = _httpClientFactory.CreateClient("ApiGatewayClient");
            var token = User.FindFirst("TokenJwt")?.Value;
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            try
            {
                var ids = await client.GetFromJsonAsync<List<int>>("api/favoritos");
                return Json(ids ?? new List<int>());
            }
            catch (HttpRequestException)
            {
                return Json(new int[0]);
            }
        }
    }

    public class ToggleFavoritoResultadoDto
    {
        public int CancionId { get; set; }
        public bool EsFavorito { get; set; }
    }
}
