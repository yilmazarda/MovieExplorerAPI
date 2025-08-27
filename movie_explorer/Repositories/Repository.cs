using Microsoft.EntityFrameworkCore;
using movie_explorer.Data;
using movie_explorer.Repositories;

namespace movie_explorer.Repositories;

public class Repository<T>(MovieContext context) : IRepository<T> where T : class
{
  private readonly MovieContext _context = context;
  private readonly DbSet<T> _dbSet = context.Set<T>();

  public async Task<T> AddAsync(T entity)
  {
    await _dbSet.AddAsync(entity);
    await _context.SaveChangesAsync();

    return entity;
  }

  public async Task DeleteAllAsync(IEnumerable<T> entities)
  {
    _dbSet.RemoveRange(entities);
    await _context.SaveChangesAsync();
  }

  public async Task DeleteAsync(T entity)
  {
    _dbSet.Remove(entity);
    await _context.SaveChangesAsync();
  }

  public async Task<IQueryable<T>> GetAllAsync()
  {
    return  _dbSet.AsQueryable();
  }

  public async Task<T?> GetByIdAsync(int id)
  {
    return await _dbSet.FindAsync(id);
  }

  public async Task<T> UpdateAsync(T entity)
  {
    _dbSet.Entry(entity).State = EntityState.Modified;
    await _context.SaveChangesAsync();
    return entity;
  }
}