using Microsoft.AspNetCore.Mvc;
using MusicAr.Web.Models;

namespace MusicAr.Web.Controllers
{
    public class PlaylistsController : Controller
    {
        [HttpGet]
        public IActionResult Nueva()
        {
            return View(new NuevaPlaylistViewModel { Sugeridas = DatosDemo.Canciones(6) });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Nueva(NuevaPlaylistViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Sugeridas = DatosDemo.Canciones(6);
                return View(model);
            }

            // TODO: crear la playlist llamando al servicio de Playlists (vía ApiGateway)
            return RedirectToAction("Index", "Home");
        }
    }
}
