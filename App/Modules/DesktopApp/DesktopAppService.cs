using System.Net;
using System.Threading.Channels;
using lanstreamer_api.App.Exceptions;
using lanstreamer_api.Data.Configuration;
using lanstreamer_api.services;

namespace lanstreamer_api.App.Modules.Access;

public class DesktopAppService
{
    private readonly ConfigurationRepository _configurationRepository;
    private readonly ServerSentEventsService<bool> _serverSentEventsService;

    public DesktopAppService(ConfigurationRepository configurationRepository, ServerSentEventsService<bool> serverSentEventsService)
    {
        _configurationRepository = configurationRepository;
        _serverSentEventsService = serverSentEventsService;
    }
    
    public async Task<ChannelReader<bool>> Access(float version, string accessCode)
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

        // TODO: reader raczej do zmiennej i usuwanie accessEntity wykorzystanych jeśli reader coś zczyta
        return _serverSentEventsService.Subscribe(accessCode);
    }
}
