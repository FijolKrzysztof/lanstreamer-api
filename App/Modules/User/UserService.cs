using System.Net;
using lanstreamer_api.App.Exceptions;
using lanstreamer_api.Data.Authentication;
using lanstreamer_api.Data.Modules.User;
using lanstreamer_api.Models;
using lanstreamer_api.services;

namespace lanstreamer_api.App.Modules;

public class UserService
{
    private readonly UserConverter _userConverter;
    private readonly UserRepository _userRepository;
    private readonly AccessRepository _accessRepository;
    private readonly ServerSentEventsService<bool> _serverSentEventsService;
    private readonly HttpRequestInfoService _httpRequestInfoService;

    public UserService(
        UserConverter userConverter,
        UserRepository userRepository,
        AccessRepository accessRepository,
        ServerSentEventsService<bool> serverSentEventsService,
        HttpRequestInfoService httpRequestInfoService
    )
    {
        _userConverter = userConverter;
        _userRepository = userRepository;
        _accessRepository = accessRepository;
        _serverSentEventsService = serverSentEventsService;
        _httpRequestInfoService = httpRequestInfoService;
    }

    public async Task<UserDto> Create(UserDto userDto, string xForwarderFor)
    {
        userDto = await UpdateUserAndNotify(userDto, xForwarderFor);

        var userEntity = _userConverter.Convert(userDto);
        var createdUserEntity = await _userRepository.Create(userEntity);
        var createdUserDto = _userConverter.Convert(createdUserEntity);

        return createdUserDto;
    }

    public async Task<UserDto> Update(UserDto userDto, string xForwardedFor)
    {
        var userEntity = await _userRepository.GetById(userDto.Id);

        if (userEntity == null)
        {
            throw new AppException(HttpStatusCode.NotFound, $"User with id: {userDto.Id} not found");
        }

        userDto = await UpdateUserAndNotify(userDto, xForwardedFor);
        
        var newUserEntity = _userConverter.Convert(userDto);

        userEntity.IpLocation = newUserEntity.IpLocation;
        userEntity.Access = newUserEntity.Access;
        userEntity.LastLogin = newUserEntity.LastLogin;
        
        var updatedUserEntity = await _userRepository.Update(userEntity);
        var updatedUserDto = _userConverter.Convert(updatedUserEntity);

        return updatedUserDto;
    }

    public async Task CleanupOldAccessRecords()
    {
        await _accessRepository.DeleteRecordsOlderThan(DateTime.UtcNow.AddHours(-1));
    }

    private async Task<UserDto> UpdateUserAndNotify(UserDto userDto, string xForwarderFor)
    {
        userDto.LastLogin = DateTime.UtcNow;

        if (userDto.Access != null)
        {
            userDto.Access.Timestamp = DateTime.UtcNow;

            await _serverSentEventsService.Send(userDto.Access.Code, true);
        }
        
        var ipAddress = _httpRequestInfoService.GetIpAddress(xForwarderFor);

        if (ipAddress != null)
        {
            userDto.IpLocation = await _httpRequestInfoService.GetIpLocation(ipAddress);
        }

        return userDto;
    }
}