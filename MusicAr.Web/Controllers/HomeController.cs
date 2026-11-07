using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MusicAr.DTO;
using MusicAr.Web.Models;
using System.Diagnostics;


namespace MusicAr.Web.Controllers
{
    public class HomeController : Controller
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMapper _mapper;

        public HomeController(IHttpClientFactory httpClientFactory, IMapper mapper)
        {
            _httpClientFactory = httpClientFactory;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("ApiGatewayClient");

            try
            {
                var generosDto = await client.GetFromJsonAsync<List<GeneroDto>>("api/canciones/generos");

                var viewModels = _mapper.Map<IEnumerable<GeneroViewModel>>(generosDto ?? new List<GeneroDto>());

                return View(viewModels);
            }
            catch (HttpRequestException)
            {
                return View(Enumerable.Empty<GeneroViewModel>());
            }
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
