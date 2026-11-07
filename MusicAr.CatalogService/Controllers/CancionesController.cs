
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
    }
}
