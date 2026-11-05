using Microsoft.AspNetCore.Mvc;
using MusicAr.Web.Models;
using System.Diagnostics;

namespace MusicAr.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // TODO: pedir los géneros al CatalogService
            return View(DatosDemo.Generos);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
