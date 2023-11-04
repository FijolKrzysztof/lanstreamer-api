using lanstreamer_api.Data.Context;
using lanstreamer_api.Data.Utils;
using Microsoft.EntityFrameworkCore;

namespace lanstreamer_api.Data.Authentication;

public class AccessRepository : BaseRepository<AccessEntity>
{
    public AccessRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<AccessEntity?> GetByCode(string code)
    {
        return await dbSet.FirstOrDefaultAsync(entity => entity.Code == code);
    }

    public async Task DeleteRecordsOlderThan(DateTime dateTime)
    {
        var records = await dbContext.Set<AccessEntity>()
            .Where(entity => entity.Timestamp.Date < dateTime)
            .ToListAsync();
        foreach (var record in records)
        {
            dbContext.Remove(record);
        }

        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteByCode(string code)
    {
        var record = await dbContext.Set<AccessEntity>().FirstOrDefaultAsync(entity => entity.Code == code);
        if (record != null)
        {
            dbContext.Remove(record);
            await dbContext.SaveChangesAsync();
        }
    }
}