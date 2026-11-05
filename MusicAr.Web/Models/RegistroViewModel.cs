using System.ComponentModel.DataAnnotations;

namespace MusicAr.Web.Models
{
    public class RegistroViewModel
    {
        [Required(ErrorMessage = "Ingresá un nombre de usuario")]
        [StringLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres")]
        [Display(Name = "Nombre de usuario")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingresá tu email")]
        [EmailAddress(ErrorMessage = "El email no es válido")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingresá una contraseña")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener al menos {2} caracteres")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Pass { get; set; } = string.Empty;

        // Solo se valida en la vista; no se envía al AuthService
        [Required(ErrorMessage = "Repetí la contraseña")]
        [DataType(DataType.Password)]
        [Compare(nameof(Pass), ErrorMessage = "Las contraseñas no coinciden")]
        [Display(Name = "Repetir contraseña")]
        public string ConfirmarPass { get; set; } = string.Empty;
    }
}
