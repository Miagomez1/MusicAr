
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicAr.Logica;
using MusicAr.Infraestructura;

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

        [HttpGet("generos/{id}/canciones")]
        public async Task<IActionResult> ObtenerDetalleGenero(int id)
        {
            // 1. Obtenemos el género para saber su nombre (usando tu servicio existente)
            var generos = await _catalogoService.ObtenerGenerosAsync();
            var genero = generos.FirstOrDefault(g => g.Id == id);

            if (genero == null)
            {
                return NotFound();
            }

            // 2. Buscamos las canciones que pertenecen a ese género
            var canciones = await _context.Canciones
                .AsNoTracking()
                .Where(c => c.GeneroId == id)
                .Select(c => new
                {
                    c.Id,
                    c.Titulo,
                    c.DurationInSeconds,
                    c.AudioUrl,
                    c.CoverImageUrl,
                    Genero = genero.Nombre,
                    Artista = c.Artista != null ? c.Artista.Name : "Desconocido"
                })
                .ToListAsync();

            // 3. Empaquetamos ambos datos para que coincidan con el GeneroDetalleViewModel de la Web
            var resultado = new
            {
                Nombre = genero.Nombre,
                Canciones = canciones
            };

            return Ok(resultado);
        }
    }
}
