namespace Jornadas_Metalurgia_2026.Models.User.DTO
{
    public class LoginResponseDTO
    {
        public string Token { get; set; } = null;
        public UserWithoutPasswordDTO User { get; set; } = null;
    }
}
