using lanstreamer_api.App.Data.Models.Enums;
using lanstreamer_api.Data.Context;
using lanstreamer_api.Data.Utils;
using Microsoft.EntityFrameworkCore;

namespace lanstreamer_api.Data.Configuration;

public class ConfigurationRepository : BaseRepository<ConfigurationEntity>, IConfigurationRepository
{
    public ConfigurationRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
    
    public async Task<ConfigurationEntity?> GetByKey(string key)
    {
        return await dbSet.FirstOrDefaultAsync(entity => entity.Key == key);
    }
}