using Microsoft.EntityFrameworkCore;

namespace lanstreamer_api.Data.Utils;

public abstract class BaseRepository<T> where T : class
{
    protected readonly DbContext dbContext;
    protected readonly DbSet<T> dbSet;

    protected BaseRepository(DbContext dbContext)
    {
        this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        dbSet = this.dbContext.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await dbSet.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await dbSet.FindAsync(id);
    }

    public async Task<T> CreateAsync(T entity)
    {
        dbSet.Add(entity);
        await dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        dbContext.Entry(entity).State = EntityState.Modified;
        await dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await dbContext.Set<T>().FindAsync(id);
        if (entity != null)
        {
            dbSet.Remove(entity);
            await dbContext.SaveChangesAsync();
        }
    }
}