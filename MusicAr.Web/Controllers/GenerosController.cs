using Microsoft.AspNetCore.Mvc;
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

        // /Generos/Detalle/5
        public async Task<IActionResult> Detalle(int id)
        {
            var client = _httpClientFactory.CreateClient("ApiGatewayClient");

            try
            {
                // Hacemos la petición al API Gateway para obtener el detalle del género y sus canciones
                var vm = await client.GetFromJsonAsync<GeneroDetalleViewModel>($"api/canciones/generos/{id}/canciones");

                if (vm is null || string.IsNullOrEmpty(vm.Nombre))
                {
                    return NotFound();
                }

                return View(vm);
            }
            catch (HttpRequestException)
            {
                // Si ocurre un error de red o el microservicio no responde
                return NotFound();
            }
        }
    }
}