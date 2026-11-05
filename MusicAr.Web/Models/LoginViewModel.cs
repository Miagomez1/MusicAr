using System.ComponentModel.DataAnnotations;

namespace MusicAr.Web.Models
{
    public class LoginViewModel
    {
        // El AuthService busca al usuario por nombre o por email
        [Required(ErrorMessage = "Ingresá tu nombre de usuario o email")]
        [Display(Name = "Nombre de usuario o email")]
        public string Identificador { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingresá tu contraseña")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Pass { get; set; } = string.Empty;
    }
}
