using System.ComponentModel.DataAnnotations;

namespace Jornadas_Metalurgia_2026.Models.User.DTO
{
    public class UpdateUserDTO
    {

        public string? UserName { get; set; } = null;

        public string? Email { get; set; } = null;

        [StringLength(250)]
        [DataType(DataType.Password)]
        public string? Password { get; set; } = null;
    }
}
