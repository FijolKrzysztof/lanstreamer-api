using System.Net;
using lanstreamer_api.App.Exceptions;
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
        var versionObj = await _configurationRepository.GetByKey("version");
        if (versionObj == null)
        {
            throw new InvalidDataException("Database error");
        }
        if (float.Parse(versionObj.Value) > float.Parse(version))
        {
            throw new AppException(HttpStatusCode.Unauthorized, "Version is not supported");
        }

        var accessEntity = await _accessRepository.GetByCode(authorizationString);
        if (accessEntity == null)
        {
            throw new AppException(HttpStatusCode.Unauthorized, null);
        }
        await _accessRepository.Delete(accessEntity.Id);

        var offlineLoginsObj = await _configurationRepository.GetByKey("offline_logins");
        if (offlineLoginsObj == null)
        {
            throw new InvalidDataException("Database error");
        }
        return offlineLoginsObj.Value;
    }
}