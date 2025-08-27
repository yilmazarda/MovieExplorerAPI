using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace movie_explorer.Repositories
{
    public interface IRepository<T>
    {
        Task<IQueryable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task DeleteAllAsync(IEnumerable<T> entities);
    }
}