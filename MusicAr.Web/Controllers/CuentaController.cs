using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MusicAr.Web.Models;
using System.Security.Claims;

namespace MusicAr.Web.Controllers
{
    public class CuentaController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CuentaController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var client = _httpClientFactory.CreateClient("ApiGatewayClient");
            var response = await client.PostAsJsonAsync("api/auth/login", model);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Credenciales inválidas.");
                return View(model);
            }

            var resultado = await response.Content.ReadFromJsonAsync<LoginResponseDto>();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, resultado?.Usuario?.Id.ToString() ?? "1"),
                new Claim(ClaimTypes.Name, resultado?.Usuario?.Nombre ?? model.Identificador),
                new Claim(ClaimTypes.Email, resultado?.Usuario?.Email ?? string.Empty),
                new Claim("TokenJwt", resultado?.Token ?? string.Empty)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

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

    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public LoginUsuarioDto? Usuario { get; set; }
    }

    public class LoginUsuarioDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
