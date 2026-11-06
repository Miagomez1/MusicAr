
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicAr.Infraestructura;

namespace MusicAr.ApiCatalog.Controllers
{
    [ApiController]
    [Route("api/canciones")]
    public class CancionesController : ControllerBase
    {
        private readonly CatalogoDBContext _context;

        public CancionesController(CatalogoDBContext context)
        {
            _context = context;
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
