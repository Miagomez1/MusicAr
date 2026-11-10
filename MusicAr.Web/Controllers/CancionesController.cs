using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace MusicAr.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CancionesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public CancionesController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet("{id:int}/audio")]
        public async Task<IActionResult> ObtenerAudio(int id)
        {
            try
            {
                var handler = new HttpClientHandler { AllowAutoRedirect = false };
                using var client = new HttpClient(handler);
                client.Timeout = TimeSpan.FromSeconds(5);
                var gatewayUrl = _configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5086/";
                var requestUrl = new Uri(new Uri(gatewayUrl), $"api/canciones/{id}/audio");

                var response = await client.GetAsync(requestUrl);

                if (response.StatusCode == HttpStatusCode.Redirect || 
                    response.StatusCode == HttpStatusCode.MovedPermanently || 
                    response.StatusCode == HttpStatusCode.Found)
                {
                    var redirectUrl = response.Headers.Location;
                    if (redirectUrl != null)
                    {
                        return Redirect(redirectUrl.ToString());
                    }
                }

                if (response.IsSuccessStatusCode)
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    return File(stream, "audio/mpeg", enableRangeProcessing: true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Web Audio] Error obteniendo audio de api-gateway: {ex.Message}");
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
