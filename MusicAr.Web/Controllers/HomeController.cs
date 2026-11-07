using Microsoft.AspNetCore.Mvc;
using MusicAr.Web.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;

namespace MusicAr.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
      

        public IActionResult Index()
        {
            return View();
        }

     
        public IActionResult Privacy()
        {
            return View();
        }

    }
}
