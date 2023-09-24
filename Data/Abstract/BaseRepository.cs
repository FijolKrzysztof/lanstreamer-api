using Microsoft.EntityFrameworkCore;

namespace lanstreamer_api.Data.Utils;

public abstract class BaseRepository<T> where T : class
{
    protected readonly DbContext _dbContext;
    protected readonly DbSet<T> _dbSet;

    protected BaseRepository(DbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _dbSet = _dbContext.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<T> CreateAsync(T entity)
    {
        _dbSet.Add(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _dbContext.Set<T>().FindAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}