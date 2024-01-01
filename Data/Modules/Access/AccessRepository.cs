using lanstreamer_api.Data.Context;
using lanstreamer_api.Data.Utils;
using Microsoft.EntityFrameworkCore;

namespace lanstreamer_api.Data.Modules.AccessCode;

public class AccessRepository : BaseRepository<AccessEntity>, IAccessRepository
{
    public AccessRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
    
    public async Task<AccessEntity?> FindByCode(string code)
    {
        return await dbContext.Set<AccessEntity>()
            .Where(entity => entity.Code == code)
            .FirstAsync();
    }
    
    public async Task<AccessEntity?> FindByUserId(int userId)
    {
        return await dbContext.Set<AccessEntity>()
            .Where(entity => entity.UserId == userId)
            .FirstOrDefaultAsync();
    }
    
    public async Task<IEnumerable<AccessEntity>> GetExpiredRecords()
    {
        return await dbContext.Set<AccessEntity>()
            .Where(entity => entity.ExpirationDate < DateTime.Now.ToUniversalTime())
            .ToListAsync();
    }
}