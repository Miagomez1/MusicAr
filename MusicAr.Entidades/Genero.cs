using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MusicAr.Entidades
{
    public class Genero
    {
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; } = string.Empty;

        public ICollection<Cancion> Canciones { get; set; } = new List<Cancion>();
    }
}
