using lanstreamer_api.Data.Context;
using lanstreamer_api.Data.Utils;
using Microsoft.EntityFrameworkCore;

namespace lanstreamer_api.Data.Authentication;

public class AccessRepository : BaseRepository<AccessEntity>
{
    public AccessRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
    
    public async Task<AccessEntity> GetByCode(string code)
    {
        return await _dbSet.FirstOrDefaultAsync(a => a.code == code);
    }
}