using System.ComponentModel.DataAnnotations;

namespace MusicAr.Web.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Ingresá tu usuario o email")]
        public string Identificador { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingresá tu contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
