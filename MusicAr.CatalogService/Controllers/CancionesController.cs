using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicAr.DTO;
using MusicAr.Infraestructura;
using MusicAr.Infraestructura.DTOs;
using MusicAr.Logica;

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

        [HttpGet("buscar")]
        public async Task<IActionResult> Buscar([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q))
            {
                return Ok(new List<CancionDto>());
            }

            var resultado = await _catalogoService.BuscarCancionesAsync(q);
            return Ok(resultado);
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
                    AudioUrl = $"/api/canciones/{c.Id}/audio",
                    c.CoverImageUrl,
                    Genero = c.Genero != null
                        ? c.Genero.Nombre
                        : null,
                    Artista = c.Artista.Name
                })
                .ToListAsync();

            return Ok(canciones);
        }

        [HttpGet("generos/{id}/canciones")]
        public async Task<IActionResult> ObtenerDetalleGenero(int id)
        {
            var generos = await _catalogoService.ObtenerGenerosAsync();
            var genero = generos.FirstOrDefault(g => g.Id == id);

            if (genero == null)
            {
                return NotFound();
            }

            var canciones = await _context.Canciones
                .AsNoTracking()
                .Where(c => c.GeneroId == id)
                .Select(c => new
                {
                    c.Id,
                    c.Titulo,
                    c.DurationInSeconds,
                    AudioUrl = $"/api/canciones/{c.Id}/audio",
                    c.CoverImageUrl,
                    Genero = genero.Nombre,
                    Artista = c.Artista != null ? c.Artista.Name : "Desconocido"
                })
                .ToListAsync();

            var resultado = new
            {
                Nombre = genero.Nombre,
                Canciones = canciones
            };

            return Ok(resultado);
        }

        [HttpGet("genero/{generoId:int}")]
        public async Task<IActionResult> ObtenerPorGenero(int generoId)
        {
            var canciones = await _catalogoService.ObtenerCancionesPorGeneroAsync(generoId);
            return Ok(canciones);
        }

        [HttpGet("{id:int}/audio")]
        public async Task<IActionResult> ObtenerAudioCancion(int id, [FromServices] IHttpClientFactory httpClientFactory, [FromServices] IWebHostEnvironment env)
        {
            var cancion = await _context.Canciones
                .AsNoTracking()
                .Include(c => c.Artista)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cancion == null) return NotFound();

            try
            {
                var client = httpClientFactory.CreateClient();
                client.Timeout = TimeSpan.FromSeconds(3);
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
            }

            var ruta = Path.Combine(env.WebRootPath ?? Path.Combine(AppContext.BaseDirectory, "wwwroot"), "audios", "demo.mp3");
            if (!System.IO.File.Exists(ruta))
            {
                return NotFound();
            }

            return PhysicalFile(ruta, "audio/mpeg", enableRangeProcessing: true);
        }
    }
}
