using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Jornadas_Metalurgia_2026.Models.User.DTO
{
    public class LoginDTO
    {
        [Required]
        [MinLength(3)]
        [JsonPropertyName("emailOrUsername")]
        public string EmailOrUsername { get; set; } = null;


        [Required]
        [MinLength(3)]
        [JsonPropertyName("password")]
        public string Password { get; set; } = null;
    }
}
