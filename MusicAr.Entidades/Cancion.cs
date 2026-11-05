using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MusicAr.Entidades
{
    public class Cancion
    {
        public int Id { get; set; }
        [Required]
        public string Titulo { get; set; } = string.Empty;
        public int? DurationInSeconds { get; set; }
        [Required]
        public string AudioUrl { get; set; } = string.Empty; 
        public string? CoverImageUrl { get; set; } = string.Empty;


        public int GeneroId { get; set; }
        [Required]
        public Genero? Genero { get; set; }

        public int ArtistaId { get; set; }
        [Required]
        public Artista Artista { get; set; }
       
    }
}
