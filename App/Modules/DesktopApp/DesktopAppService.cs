using System.Net;
using System.Threading.Channels;
using lanstreamer_api.App.Exceptions;
using lanstreamer_api.Data.Authentication;
using lanstreamer_api.Data.Configuration;
using lanstreamer_api.services;

namespace lanstreamer_api.App.Modules.Access;

public class DesktopAppService
{
    private readonly ConfigurationRepository _configurationRepository;
    private readonly ServerSentEventsService<bool> _serverSentEventsService;
    private readonly AccessRepository _accessRepository;

    public DesktopAppService(
        ConfigurationRepository configurationRepository,
        ServerSentEventsService<bool> serverSentEventsService,
        AccessRepository accessRepository
    )
    {
        _configurationRepository = configurationRepository;
        _serverSentEventsService = serverSentEventsService;
        _accessRepository = accessRepository;
    }

    public async Task<bool> Access(float version, string accessCode)
    {
        var versionObj = await _configurationRepository.GetByKey("version");
        if (versionObj == null)
        {
            throw new InvalidDataException("Database error");
        }

        if (float.Parse(versionObj.Value) > version)
        {
            throw new AppException(HttpStatusCode.Unauthorized, "Version is not supported");
        }

        var reader = _serverSentEventsService.Subscribe(accessCode);
        var access = await reader.ReadAsync();

        await _accessRepository.DeleteByCode(accessCode);
        
        return access;
    }
}