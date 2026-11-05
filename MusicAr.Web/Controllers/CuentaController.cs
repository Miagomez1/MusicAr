using Microsoft.AspNetCore.Mvc;
using MusicAr.Web.Models;

namespace MusicAr.Web.Controllers
{
    public class CuentaController : Controller
    {
        [HttpGet]
        public IActionResult Login() => View(new LoginViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            // TODO: enviar las credenciales al AuthService (vía ApiGateway) y guardar el JWT recibido
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Registro() => View(new RegistroViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Registro(RegistroViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            // TODO: registrar el usuario en el AuthService (vía ApiGateway)
            return RedirectToAction("Login");
        }
    }
}
