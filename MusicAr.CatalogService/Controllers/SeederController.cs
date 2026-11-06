using Microsoft.AspNetCore.Mvc;
using MusicAr.Infraestructura.Services;

namespace MusicAr.ApiCatalog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeederController : ControllerBase
    {
        private readonly CatalogSeederService _seederService;

        public SeederController(CatalogSeederService seederService)
        {
            _seederService = seederService;
        }

        [HttpPost("populardatos")]
        public async Task<IActionResult> CargarDatos()
        {
            var listaArtistas = new List<(string Artista, string GeneroDefecto)>
        {
            ("Duki", "Trap"),
            ("Bad Bunny", "Trap"),
            ("Anuel AA", "Trap"),
            ("Cazzu", "Trap"),
            ("Khea", "Trap"),
            ("YSY A", "Trap"),
            ("Paulo Londra", "Trap"),
            ("Trueno", "Trap"),
            ("Neo Pistea", "Trap")

        };

            foreach (var item in listaArtistas)
            {
                await _seederService.SeedByArtistNameAsync(item.Artista, item.GeneroDefecto);
            }

            return Ok(new { mensaje = "Base de datos cargada correctamente con artistas, géneros y canciones." });
        }
    }
}
