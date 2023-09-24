using lanstreamer_api.Data.Context;
using lanstreamer_api.Data.Utils;
using Microsoft.EntityFrameworkCore;

namespace lanstreamer_api.Data.Configuration;

public class ConfigurationRepository : BaseRepository<ConfigurationEntity>
{
    ConfigurationRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
    
    public async Task<ConfigurationEntity> GetByKey(string key)
    {
        return await _dbSet.FirstOrDefaultAsync(c => c.key == key);
    }
}