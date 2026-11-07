
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicAr.Logica;
using MusicAr.Infraestructura;
using MusicAr.Infraestructura.DTOs;

namespace MusicAr.ApiCatalog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CancionesController : ControllerBase
    {
        private readonly CatalogoDBContext _context;
        private readonly ICatalogoService _catalogoService;

        public CancionesController(CatalogoDBContext context, ICatalogoService catalogoService)
        {
            _context = context;
            _catalogoService = catalogoService;
        }


        [HttpGet("generos")] 
        public async Task<IActionResult> Generos()
        {
            var generos = await _catalogoService.ObtenerGenerosAsync();
            return Ok(generos);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerCanciones()
        {
            var canciones = await _context.Canciones
                .AsNoTracking()
                .Select(c => new
                {
                    c.Id,
                    c.Titulo,
                    c.DurationInSeconds,
                    c.AudioUrl,
                    c.CoverImageUrl,
                    Genero = c.Genero != null
                        ? c.Genero.Nombre
                        : null,
                    Artista = c.Artista.Name
                })
                .ToListAsync();

            return Ok(canciones);
        }

        [HttpGet("genero/{generoId:int}")]
        public async Task<IActionResult> ObtenerPorGenero(int generoId)
        {
            var canciones = await _catalogoService.ObtenerCancionesPorGeneroAsync(generoId);
            return Ok(canciones);
        }

        [HttpGet("{id:int}/audio")]
        public async Task<IActionResult> ObtenerAudioCancion(int id, [FromServices] IHttpClientFactory httpClientFactory)
        {
            var cancion = await _context.Canciones
                .AsNoTracking()
                .Include(c => c.Artista)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cancion == null) return NotFound();

            try
            {
                var client = httpClientFactory.CreateClient();
                var query = Uri.EscapeDataString($"{cancion.Artista?.Name} {cancion.Titulo}");
                var url = $"https://api.deezer.com/search?q={query}";
                var resp = await client.GetFromJsonAsync<DeezerTrackResponse>(url);
                var preview = resp?.Data?.FirstOrDefault(d => !string.IsNullOrEmpty(d.Preview))?.Preview;

                if (!string.IsNullOrEmpty(preview))
                {
                    return Redirect(preview);
                }
            }
            catch
            {
                // Si Deezer falla, pasamos al fallback
            }

            return Redirect("https://www.soundhelix.com/examples/mp3/SoundHelix-Song-1.mp3");
        }
    }

}
