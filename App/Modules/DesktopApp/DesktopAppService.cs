using System.Globalization;
using System.Net;
using System.Text;
using lanstreamer_api.App.Data.Models.Enums;
using lanstreamer_api.App.Exceptions;
using lanstreamer_api.Data.Configuration;
using lanstreamer_api.Data.Modules.AccessCode;
using lanstreamer_api.Data.Modules.User;
using lanstreamer_api.services;

namespace lanstreamer_api.App.Modules.Access;

public class DesktopAppService
{
    private readonly IConfigurationRepository _configurationRepository;
    private readonly IServerSentEventsService<bool> _serverSentEventsService;
    private readonly IUserRepository _userRepository;
    private readonly IAccessRepository _accessRepository;

    public DesktopAppService(
        IConfigurationRepository configurationRepository,
        IServerSentEventsService<bool> serverSentEventsService,
        IUserRepository userRepository,
        IAccessRepository accessRepository
    )
    {
        _configurationRepository = configurationRepository;
        _serverSentEventsService = serverSentEventsService;
        _userRepository = userRepository;
        _accessRepository = accessRepository;
    }

    public async Task Access(float version, string accessCode, HttpResponse response)
    {
        var versionObj = (await _configurationRepository.GetByKey(ConfigurationKey.DesktopAppVersion.ToString()))!;

        var culture = CultureInfo.CreateSpecificCulture("en-US");
        var versionValue = float.Parse(versionObj.Value, culture);
        if (versionValue > version)
        {
            throw new AppException(HttpStatusCode.Unauthorized, "Version is not supported");
        }
        
        var loginTimeoutObj = await _configurationRepository.GetByKey(ConfigurationKey.LoginTimeoutSeconds.ToString());
        var timeout = int.Parse(loginTimeoutObj!.Value, culture);

        var accessEntity = await _accessRepository.Create(new AccessEntity()
        {
            Code = accessCode,
            ExpirationDate = DateTime.Now.AddSeconds(timeout).ToUniversalTime(),
        });
        
        var reader = _serverSentEventsService.Subscribe(accessCode);
        
        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(timeout));

        var cancellationToken = cancellationTokenSource.Token;
        
        try
        {
            while (await reader.WaitToReadAsync(cancellationToken))
            {
                while (reader.TryRead(out var access))
                {
                    if (access)
                    {
                        var accessEntityWithUserId = (await _accessRepository.GetById(accessEntity.Id))!;
                        accessEntityWithUserId = await _accessRepository.Reload(accessEntityWithUserId);
                        var userEntity = await _userRepository.GetById(accessEntityWithUserId!.UserId!.Value);

                        userEntity!.AppVersion = version;

                        await _userRepository.Update(userEntity);
                        
                        var offlineLoginsObj = await _configurationRepository.GetByKey(ConfigurationKey.OfflineLogins.ToString());
                        
                        await response.Body.WriteAsync(Encoding.UTF8.GetBytes(offlineLoginsObj!.Value), cancellationToken);
                        await response.Body.FlushAsync(cancellationToken);
                        
                        _serverSentEventsService.Unsubscribe(accessCode);
                        await _accessRepository.Delete(accessEntity.Id);
                        return;
                    }
                }
            }
        }
        catch (Exception e)
        {
            // TODO: czy nie powinienem logować tych błędów?
            _serverSentEventsService.Unsubscribe(accessCode);
            throw new AppException(HttpStatusCode.RequestTimeout, "Timeout waiting for user login");
        }
    }
}
