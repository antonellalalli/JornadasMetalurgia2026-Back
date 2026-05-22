using Jornadas_Metalurgia_2026.Config;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Jornadas_Metalurgia_2026.Repositories
{

    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null);
        Task<T> GetOneAsync(Expression<Func<T, bool>>? filter = null);
        Task CreateOneAsync(T entity);
        Task UpdateOneAsync(T entity);
        Task DeleteOneAsync(T entity);
        Task SaveAsync();


    };
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _contextDB;
        internal DbSet<T> dbSet { get; set; } = null!;

        public Repository(ApplicationDbContext contextDB)
        {
            _contextDB = contextDB;

            dbSet = _contextDB.Set<T>();

        }


        async public Task CreateOneAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            await SaveAsync();
        }

        public async Task DeleteOneAsync(T entity)
        {
            dbSet.Attach(entity);
            _contextDB.Entry(entity).State = EntityState.Modified;

            await SaveAsync();
        }





        public async virtual Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }

        public async virtual Task<T> GetOneAsync(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task UpdateOneAsync(T entity)
        {
            dbSet.Update(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _contextDB.SaveChangesAsync();
        }
    }
}
