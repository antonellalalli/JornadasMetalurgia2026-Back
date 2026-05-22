
namespace Jornadas_Metalurgia_2026.Models.User.DTO
{
    public class UserWithoutPasswordDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
    }
}
