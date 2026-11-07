using AutoMapper;
using MusicAr.DTO;
using MusicAr.Web.Models;
namespace MusicAr.Web.Mappings
{
    public class MappingProfile : Profile
    {
        // Los acentos exactos que definió tu compañero en el site.css
        private static readonly string[] AcentosValidos = { "plum", "lime", "gray" };

        public MappingProfile()
        {
            CreateMap<GeneroDto, GeneroViewModel>()
                .ConstructUsing(src => new GeneroViewModel(
                    src.Id,
                    src.Nombre,
                    // Asigna plum, lime o gray de forma determinista usando el Id del género
                    AcentosValidos[Math.Abs(src.Id) % AcentosValidos.Length]
                ));
        }
    }
}
