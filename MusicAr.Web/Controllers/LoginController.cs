using Microsoft.AspNetCore.Mvc;
using MusicAr.Web.Models;
using MusicAr.Web.DTOs;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace MusicAr.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LoginController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var client = _httpClientFactory.CreateClient("ApiGateway");

            var response = await client.PostAsJsonAsync("api/Auth/login", new
            {
                identificador = model.Identificador,
                password = model.Password
            });

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Usuario o contraseña incorrectos.");
                return View(model);
            }

            var resultado = await response.Content.ReadFromJsonAsync<LoginResponseDto>();

            if (resultado == null || string.IsNullOrEmpty(resultado.Token))
            {
                ModelState.AddModelError("", "No se pudo obtener el token.");
                return View(model);
            }

            var claims = new List<Claim>
{
               new Claim(ClaimTypes.Name, model.Identificador),
                new Claim("access_token", resultado.Token)
};

            var identidad = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            var principal = new ClaimsPrincipal(identidad);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal
            );

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            return RedirectToAction("Index", "Login");
        }
    }
}
