using lanstreamer_api.Data.Context;
using lanstreamer_api.Data.Utils;

namespace lanstreamer_api.Data.Configuration;

public class ConfigurationRepository : AbstractRepository<ConfigurationEntity>
{
    ConfigurationRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}