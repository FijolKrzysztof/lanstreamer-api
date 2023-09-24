using System.Data;
using lanstreamer_api.Data.Authentication;
using lanstreamer_api.Data.Configuration;
using Org.BouncyCastle.Bcpg;

namespace lanstreamer_api.App.Modules.Legacy;

public class LegacyService
{
    private readonly ConfigurationRepository _configurationRepository;
    private readonly AccessRepository _accessRepository;
    
    public LegacyService(ConfigurationRepository configurationRepository, AccessRepository accessRepository)
    {
        _configurationRepository = configurationRepository;
        _accessRepository = accessRepository;
    }
    
    public async Task<string> AppAccess(string authorizationString, string version)
    {
        var versionValue = (await _configurationRepository.GetByKey("version")).value;
        if (int.Parse(versionValue) > int.Parse(version))
        {
            throw new UnsupportedPacketVersionException("Version is not supported!");
        }

        var accessEntity = await _accessRepository.GetByCode(authorizationString);
        if (accessEntity == null)
        {
            throw new UnauthorizedAccessException();
        }
        await _accessRepository.DeleteAsync(accessEntity.id);
        
        return (await _configurationRepository.GetByKey("offline_logins")).value;
    }
}