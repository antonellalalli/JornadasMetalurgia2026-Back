using Jornadas_Metalurgia_2026.Repositories;
using Jornadas_Metalurgia_2026.Utils;
using System.Net;
using Jornadas_Metalurgia_2026.Models.Role;
namespace Jornadas_Metalurgia_2026.Services
{
    public class RoleService
    {

        private readonly IRoleRepository _repo;
        public RoleService(IRoleRepository repo)
        {
            _repo = repo;
        }

        public async Task<Role> GetOneByName(string name)
        {
            var role = await _repo.GetOneAsync(x => x.Name == name);
            if (role == null)
            {

                throw new HttpResponseError(HttpStatusCode.NotFound, $"Role {name} doesn´t exists");
            }
            return role;
        }
    }
}
