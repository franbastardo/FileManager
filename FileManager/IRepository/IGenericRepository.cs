using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FileManager.IRepository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> Get(Expression<Func<T, bool>> expression, List<string> includes = null);
        Task<EntityEntry<T>> Insert(T entity);
        Task InsertRange(IEnumerable<T> entity);
        Task Update(T entity);
        Task Delete(int id);
    }
}
