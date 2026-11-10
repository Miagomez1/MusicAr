using Microsoft.AspNetCore.Mvc;

namespace MusicAr.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CancionesController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IWebHostEnvironment _env;

        public CancionesController(IHttpClientFactory httpClientFactory, IWebHostEnvironment env)
        {
            _httpClientFactory = httpClientFactory;
            _env = env;
        }

        [HttpGet("{id:int}/audio")]
        public async Task<IActionResult> ObtenerAudio(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("ApiGatewayClient");
                var response = await client.GetAsync($"api/canciones/{id}/audio", HttpCompletionOption.ResponseHeadersRead);

                if (response.RequestMessage?.RequestUri != null && 
                    !response.RequestMessage.RequestUri.ToString().Contains("/api/canciones/"))
                {
                    return Redirect(response.RequestMessage.RequestUri.ToString());
                }

                if (response.IsSuccessStatusCode)
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    return File(stream, "audio/mpeg", enableRangeProcessing: true);
                }
            }
            catch
            {
            }

            var rutaLocal = Path.Combine(_env.WebRootPath ?? Path.Combine(AppContext.BaseDirectory, "wwwroot"), "audios", "demo.mp3");
            if (System.IO.File.Exists(rutaLocal))
            {
                return PhysicalFile(rutaLocal, "audio/mpeg", enableRangeProcessing: true);
            }

            return NotFound();
        }
    }
}
