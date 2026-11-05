using Microsoft.AspNetCore.Mvc;
using MusicAr.Web.Models;

namespace MusicAr.Web.Controllers
{
    public class GenerosController : Controller
    {
        // /Generos/Detalle/5
        public IActionResult Detalle(int id)
        {
            // TODO: pedir el género y sus canciones al CatalogService
            var genero = DatosDemo.Generos.FirstOrDefault(g => g.Id == id);
            if (genero is null) return NotFound();

            var vm = new GeneroDetalleViewModel
            {
                Nombre = genero.Nombre,
                Canciones = DatosDemo.Canciones()
            };
            return View(vm);
        }
    }
}
