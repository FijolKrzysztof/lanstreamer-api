using System.Net;
using lanstreamer_api.App.Data.Models.Enums;
using lanstreamer_api.App.Exceptions;
using lanstreamer_api.Data.Configuration;
using lanstreamer_api.Data.Modules.User;
using lanstreamer_api.services;

namespace lanstreamer_api.App.Modules.Access;

public class DesktopAppService
{
    private readonly ConfigurationRepository _configurationRepository;
    private readonly IServerSentEventsService<bool> _serverSentEventsService;
    private readonly IUserRepository _userRepository;

    public DesktopAppService(
        ConfigurationRepository configurationRepository,
        IServerSentEventsService<bool> serverSentEventsService,
        IUserRepository userRepository
    )
    {
        _configurationRepository = configurationRepository;
        _serverSentEventsService = serverSentEventsService;
        _userRepository = userRepository;
    }

    public async Task<bool> Access(float version, string accessCode)
    {
        var versionObj = (await _configurationRepository.GetByKey(ConfigurationKey.AppVersion))!;

        if (float.Parse(versionObj.Value) > version)
        {
            throw new AppException(HttpStatusCode.Unauthorized, "Version is not supported");
        }
        
        var reader = _serverSentEventsService.Subscribe(accessCode);
        var access = await reader.ReadAsync();
        
        var userEntity = (await _userRepository.GetByAccessCode(accessCode))!;

        userEntity.AppVersion = version;
        userEntity.AccessCode = null;

        await _userRepository.Update(userEntity);
        
        return access;
    }
}