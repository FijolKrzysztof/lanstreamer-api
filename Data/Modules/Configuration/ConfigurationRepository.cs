using System.Net;
using lanstreamer_api.App.Data.Models.Enums;
using lanstreamer_api.App.Exceptions;
using lanstreamer_api.Data.Context;
using lanstreamer_api.Data.Utils;
using Microsoft.EntityFrameworkCore;

namespace lanstreamer_api.Data.Configuration;

public class ConfigurationRepository : BaseRepository<ConfigurationEntity>, IConfigurationRepository
{
    public ConfigurationRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<string> GetByKey(ConfigurationKey key)
    {
        var entity = await dbSet.FirstOrDefaultAsync(entity => entity.Key == key.ToString());

        if (entity == null)
        {
            throw new AppException(HttpStatusCode.InternalServerError, "Configuration not found");
        }

        return entity.Value;
    }
}