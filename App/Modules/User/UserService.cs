using System.Net;
using Google.Apis.Auth;
using lanstreamer_api.App.Exceptions;
using lanstreamer_api.Data.Modules.User;
using lanstreamer_api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
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
    
    public async Task<ActionResult<UserDto>> Create(UserDto userDto, string idToken)
    {
        try
        {
            await GoogleJsonWebSignature.ValidateAsync(idToken);
        }
        catch (InvalidJwtException)
        {
            throw new AppException(HttpStatusCode.Unauthorized, "Invalid google id token");
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