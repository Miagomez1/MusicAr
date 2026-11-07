using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MusicAr.Web.Models;
using System.Security.Claims;

namespace MusicAr.Web.Controllers
{
    public class CuentaController : Controller
    {
        private readonly HttpClient _httpClient;

        public CuentaController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public IActionResult Login() => View(new LoginViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var response = await _httpClient.PostAsJsonAsync("https://localhost:7211/api/Auth/login", model);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Credenciales inválidas.");
                return View(model);
            }

            var resultado = await response.Content.ReadFromJsonAsync<LoginResponseDto>();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Identificador),
                new Claim("TokenJwt", resultado?.Token ?? string.Empty)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Registro() => View(new RegistroViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Registro(RegistroViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            return RedirectToAction("Login");
        }
    }
}

public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;
}