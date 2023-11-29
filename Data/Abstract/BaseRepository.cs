using Microsoft.EntityFrameworkCore;

namespace lanstreamer_api.Data.Utils;

public abstract class BaseRepository<T> where T : BaseEntity
{
    protected readonly DbContext dbContext;
    protected readonly DbSet<T> dbSet;

    protected BaseRepository(DbContext dbContext)
    {
        this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        dbSet = this.dbContext.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAll()
    {
        return await dbSet.ToListAsync();
    }

    public async Task<T?> GetById(int id)
    {
        return await dbSet.FindAsync(id);
    }

    public async Task<T> Create(T entity)
    {
        entity.Id = default;
        dbSet.Add(entity);
        await dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<T> Update(T entity)
    {
        dbContext.Entry(entity).State = EntityState.Modified;
        await dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task Delete(int id)
    {
        var entity = await dbSet.FindAsync(id);
        if (entity != null)
        {
            dbSet.Remove(entity);
            await dbContext.SaveChangesAsync();
        }
    }
}