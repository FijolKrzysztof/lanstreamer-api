using lanstreamer_api.Data.Utils;

namespace lanstreamer_api.Data.Configuration;

public interface IConfigurationRepository : IBaseRepository<ConfigurationEntity>
{
    Task<ConfigurationEntity?> GetByKey(string key);
}