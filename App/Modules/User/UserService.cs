using lanstreamer_api.App.Data.Models;
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

    public UserService(
        IUserConverter userConverter,
        IUserRepository userRepository,
        IServerSentEventsService<bool> serverSentEventsService,
        IHttpRequestInfoService httpRequestInfoService
    )
    {
        _userConverter = userConverter;
        _userRepository = userRepository;
        _serverSentEventsService = serverSentEventsService;
        _httpRequestInfoService = httpRequestInfoService;
    }

    public async Task<LoginResponse> Login(UserDto newUserDto, HttpContext httpContext)
    {
        var googleId = _httpRequestInfoService.GetIdentity(httpContext)!;

        var userEntity = await _userRepository.GetByGoogleId(googleId) ?? new UserEntity();
        var user = _userConverter.Convert<User>(userEntity);
        
        user.GoogleId = googleId;
        user.AccessCode = newUserDto.AccessCode;
        user.Email = _httpRequestInfoService.GetEmail(httpContext)!;
        user.LastLogin = DateTime.UtcNow.ToUniversalTime();
        
        var ipAddress = _httpRequestInfoService.GetIpAddress(httpContext);

        if (ipAddress != null)
        {
            user.IpLocation = await _httpRequestInfoService.GetIpLocation(ipAddress);
        }

        var newUserEntity = _userConverter.Convert<UserEntity>(user);
        await _userRepository.Update(newUserEntity);

        if (user.AccessCode != null)
        {
            await _serverSentEventsService.Send(user.AccessCode, true);
        }

        return new LoginResponse()
        {
            Roles = _httpRequestInfoService.GetRoles(httpContext),
        };
    }

    public async Task CleanupOldAccessRecords()
    {
        await _userRepository.RemoveAccessCodeOlderThan(DateTime.UtcNow.AddHours(-1));
    }
}