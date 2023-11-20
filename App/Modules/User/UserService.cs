using lanstreamer_api.Data.Modules.User;
using lanstreamer_api.Models;
using lanstreamer_api.services;

namespace lanstreamer_api.App.Modules;

public class UserService
{
    private readonly UserConverter _userConverter;
    private readonly UserRepository _userRepository;
    private readonly ServerSentEventsService<bool> _serverSentEventsService;
    private readonly HttpRequestInfoService _httpRequestInfoService;

    public UserService(
        UserConverter userConverter,
        UserRepository userRepository,
        ServerSentEventsService<bool> serverSentEventsService,
        HttpRequestInfoService httpRequestInfoService
    )
    {
        _userConverter = userConverter;
        _userRepository = userRepository;
        _serverSentEventsService = serverSentEventsService;
        _httpRequestInfoService = httpRequestInfoService;
    }

    public async Task Login(UserDto newUserDto, HttpContext httpContext)
    {
        var googleId = _httpRequestInfoService.GetIdentity(httpContext)!;

        var userEntity = await _userRepository.GetByGoogleId(googleId) ?? new UserEntity();
        var userDto = _userConverter.Convert(userEntity);

        userDto.GoogleId = googleId;
        userDto.AccessCode = newUserDto.AccessCode;
        userDto.Email = _httpRequestInfoService.GetEmail(httpContext)!;
        userDto.LastLogin = DateTime.UtcNow;
        
        var ipAddress = _httpRequestInfoService.GetIpAddress(httpContext);

        if (ipAddress != null)
        {
            userDto.IpLocation = await _httpRequestInfoService.GetIpLocation(ipAddress);
        }

        var newUserEntity = _userConverter.Convert(userDto);
        await _userRepository.Update(newUserEntity);

        if (userDto.AccessCode != null)
        {
            await _serverSentEventsService.Send(userDto.AccessCode, true);
        }
    }

    public async Task CleanupOldAccessRecords()
    {
        await _userRepository.RemoveAccessCodeOlderThan(DateTime.UtcNow.AddHours(-1));
    }
}