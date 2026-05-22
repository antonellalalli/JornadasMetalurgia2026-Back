using Jornadas_Metalurgia_2026.Config;
using Jornadas_Metalurgia_2026.Models.User;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace Jornadas_Metalurgia_2026.Repositories
{
    public interface IUserRepository : IRepository<User> { }
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _contextDB;
        public UserRepository(ApplicationDbContext contextDB) : base(contextDB)
        {
            _contextDB = contextDB;

        }
        public override async Task<User> GetOneAsync(Expression<Func<User, bool>>? filter = null)
        {
            IQueryable<User> query = dbSet.Include(x => x.Roles);
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }


        public override async Task<IEnumerable<User>> GetAllAsync(Expression<Func<User, bool>>? filter = null)
        {
            IQueryable<User> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.Include(x => x.Roles).ToListAsync();
        }

    }
}
