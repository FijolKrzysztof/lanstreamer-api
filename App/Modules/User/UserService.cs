using lanstreamer_api.App.Data.Models;
using lanstreamer_api.Data.Modules.AccessCode;
using lanstreamer_api.Data.Modules.User;
using lanstreamer_api.Models;
using lanstreamer_api.Models.Responses;
using lanstreamer_api.services;

namespace lanstreamer_api.App.Modules;

public class UserService
{
    private readonly IUserConverter _userConverter;
    private readonly IUserRepository _userRepository;
    private readonly IServerSentEventsService<bool> _serverSentEventsService;
    private readonly IHttpRequestInfoService _httpRequestInfoService;
    private readonly IAccessRepository _accessRepository;

    public UserService(
        IUserConverter userConverter,
        IUserRepository userRepository,
        IServerSentEventsService<bool> serverSentEventsService,
        IHttpRequestInfoService httpRequestInfoService,
        IAccessRepository accessRepository
    )
    {
        _userConverter = userConverter;
        _userRepository = userRepository;
        _serverSentEventsService = serverSentEventsService;
        _httpRequestInfoService = httpRequestInfoService;
        _accessRepository = accessRepository;
    }

    public async Task<LoginResponse> Login(UserDto newUserDto, HttpContext httpContext)
    {
        var googleId = _httpRequestInfoService.GetIdentity(httpContext)!;

        var userEntity = await _userRepository.GetByGoogleId(googleId) ?? new UserEntity();
        var user = _userConverter.Convert<User>(userEntity);
        
        user.GoogleId = googleId;
        user.Email = _httpRequestInfoService.GetEmail(httpContext)!;
        user.LastLogin = DateTime.UtcNow.ToUniversalTime();
        
        var ipAddress = _httpRequestInfoService.GetIpAddress(httpContext);

        if (ipAddress != null)
        {
            user.IpLocation = await _httpRequestInfoService.GetIpLocation(ipAddress);
        }

        var newUserEntity = _userConverter.Convert<UserEntity>(user);
        newUserEntity = await _userRepository.UpdateOrCreate(newUserEntity);

        var accessCode = newUserDto.AccessCode;
        
        if (!string.IsNullOrEmpty(accessCode))
        {
            var oldAccessEntity = await _accessRepository.FindByUserId(newUserEntity.Id);
            if (oldAccessEntity != null)
            {
                await _accessRepository.Delete(oldAccessEntity.Id);
            }
            
            var accessEntity = await _accessRepository.FindByCode(accessCode);
            accessEntity!.UserId = newUserEntity.Id;
            await _accessRepository.Update(accessEntity);
            
            await _serverSentEventsService.Send(accessCode, true);
        }
        
        // TODO: test do sprawdzania czy poprzednie access się usunęło
     
        return new LoginResponse()
        {
            Roles = _httpRequestInfoService.GetRoles(httpContext),
        };
    }

    public async Task CleanupOldAccessRecords()
    {
        var expiredAccesses = await _accessRepository.GetExpiredRecords();
        if (!expiredAccesses.Any())
        {
            return;
        }
        await _accessRepository.DeleteMany(expiredAccesses.Select(a => a.Id));
        foreach(var expiredAccess in expiredAccesses)
        {
            _serverSentEventsService.Unsubscribe(expiredAccess.Code);
        }
    }
}