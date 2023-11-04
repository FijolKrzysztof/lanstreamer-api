using Google.Protobuf.WellKnownTypes;
using lanstreamer_api.Data.Modules.User;
using lanstreamer_api.Models;
using lanstreamer_api.services;

namespace lanstreamer_api.App.Modules;

public class UserService
{
    private readonly UserConverter _userConverter;
    private readonly UserRepository _userRepository;
    private readonly ServerSentEventsService<bool> _serverSentEventsService;

    UserService(UserConverter userConverter, UserRepository userRepository, ServerSentEventsService<bool> serverSentEventsService)
    {
        _userConverter = userConverter;
        _userRepository = userRepository;
        _serverSentEventsService = serverSentEventsService;
    }
    
    public async Task<UserDto> Create(UserDto userDto)
    {
        userDto = await UpdateUserAndNotify(userDto);
        
        var userEntity = _userConverter.Convert(userDto);
        var createdUserEntity = await _userRepository.Create(userEntity);
        var createdUserDto = _userConverter.Convert(createdUserEntity);

        return createdUserDto;
    }
    
    public async Task<UserDto> Update(UserDto userDto)
    {
        userDto = await UpdateUserAndNotify(userDto);

        var userEntity = _userConverter.Convert(userDto);
        var updatedUserEntity = await _userRepository.Update(userEntity);
        var updatedUserDto = _userConverter.Convert(updatedUserEntity);

        return updatedUserDto;
    }

    private async Task<UserDto> UpdateUserAndNotify(UserDto userDto)
    {
        userDto.LastLogin = DateTime.Now;

        if (userDto.Access != null)
        {
            userDto.Access.Timestamp = Timestamp.FromDateTime(DateTime.Now);
            
            await _serverSentEventsService.Send(userDto.Access.Code, true);
        }

        return userDto;
    }
}