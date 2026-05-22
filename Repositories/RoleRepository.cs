using Jornadas_Metalurgia_2026.Config;
using Jornadas_Metalurgia_2026.Models.Role;

namespace Jornadas_Metalurgia_2026.Repositories
{
    public interface IRoleRepository : IRepository<Role> { }
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        private readonly ApplicationDbContext _contextDB;
        public RoleRepository(ApplicationDbContext contextDB) : base(contextDB)
        {
            _contextDB = contextDB;
        }
    }
}
