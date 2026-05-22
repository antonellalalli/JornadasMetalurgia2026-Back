using Jornadas_Metalurgia_2026.Config;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using Jornadas_Metalurgia_2026.Models.Inscription;
namespace Jornadas_Metalurgia_2026.Repositories
{
    public interface IInscriptionRepository : IRepository<Inscription> {

        IQueryable<Inscription> GetAllQueryable();
    }
    public class InscriptionRepository : Repository<Inscription>, IInscriptionRepository
    {

        private readonly ApplicationDbContext _contextDB;
        public InscriptionRepository(ApplicationDbContext contextDB) : base(contextDB)
        {
            _contextDB = contextDB;
        }

        public override async Task<Inscription> GetOneAsync(Expression<Func<Inscription, bool>>? filter = null)
        {
            IQueryable<Inscription> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.FirstOrDefaultAsync();
        }


        public  IQueryable<Inscription> GetAllQueryable()
        {
            return dbSet.AsQueryable();
        }
        public override async Task<IEnumerable<Inscription>> GetAllAsync(Expression<Func<Inscription, bool>>? filter = null)
        {
            IQueryable<Inscription> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.ToListAsync();
        }

    }
}
