using FileManager.Data;
using FileManager.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FileManager.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly FileManagerContext _context;
        private readonly DbSet<T> _db;

        public GenericRepository(FileManagerContext context)
        {
            _context = context;
            _db = _context.Set<T>();
        }

        public async Task Delete(int id)
        {
            var entity = await _db.FindAsync(id);
            _db.Remove(entity);
        }

        public async Task<T> Get(System.Linq.Expressions.Expression<System.Func<T, bool>> expression, List<string> includes = null)
        {
            IQueryable<T> query = _db;
            if (includes != null)
            {
                foreach (var includeProperty in includes)
                {
                    query = query.Include(includeProperty);
                }
            }

            return await query.AsNoTracking().FirstOrDefaultAsync(expression);

        }

        public async Task<EntityEntry<T>> Insert(T entity)
        {
            return await _db.AddAsync(entity);
        }

        public async Task InsertRange(IEnumerable<T> entity)
        {
            await _db.AddRangeAsync(entity);
        }

        public async Task Update(T entity)
        {
            _db.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;

        }
    }
}
