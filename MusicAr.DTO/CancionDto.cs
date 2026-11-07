using System;
using System.Collections.Generic;
using System.Text;

namespace MusicAr.DTO
{
    public class CancionDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public int? DurationInSeconds { get; set; }
        public string AudioUrl { get; set; } = string.Empty;
        public string? CoverImageUrl { get; set; }
        public string Genero { get; set; } = string.Empty;
        public string Artista { get; set; } = string.Empty;
    }
}