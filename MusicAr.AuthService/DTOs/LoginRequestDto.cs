namespace MusicAr.AuthService.DTOs
{
    public class LoginRequestDto
    {
        public string Identificador { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
