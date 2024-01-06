using lanstreamer_api.App.Data.Models.Enums;
using lanstreamer_api.Data.Utils;

namespace lanstreamer_api.Data.Configuration;

public interface IConfigurationRepository : IBaseRepository<ConfigurationEntity>
{
    Task<string> GetByKey(ConfigurationKey key);
}