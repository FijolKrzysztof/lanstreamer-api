using Google.Protobuf.WellKnownTypes;
using lanstreamer_api.Data.Modules.User;
using lanstreamer_api.Models;
using Microsoft.AspNetCore.Mvc;

namespace lanstreamer_api.App.Modules;

public class UserService
{
    private readonly UserConverter _userConverter;
    private readonly UserRepository _userRepository;

    UserService(UserConverter userConverter, UserRepository userRepository)
    {
        _userConverter = userConverter;
        _userRepository = userRepository;
    }
    
    public async Task<ActionResult<UserDto>> Create(UserDto userDto)
    {
        userDto.lastLogin = DateTime.Now;
        
        if (userDto.access != null)
        {
            userDto.access.timestamp = Timestamp.FromDateTime(DateTime.Now);
        }
        
        var userEntity = _userConverter.Convert(userDto);
        var createdUserEntity = await _userRepository.CreateAsync(userEntity);
        var createdUserDto = _userConverter.Convert(createdUserEntity);

        return createdUserDto;
    }
    
    public Task<ActionResult> Update(UserDto userDto)
    {
        throw new NotImplementedException();
    }
}