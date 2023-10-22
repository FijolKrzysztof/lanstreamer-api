using AutoMapper;
using lanstreamer_api.Data.Authentication;
using lanstreamer_api.Data.Modules.User;
using lanstreamer_api.Models;

namespace lanstreamer_api.App.Modules;

public class UserConverter
{
    private readonly IMapper _mapper;

    UserConverter(IMapper mapper)
    {
        _mapper = mapper;
    }

    public UserEntity Convert(UserDto userDto)
    {
        if (userDto == null)
        {
            throw new ArgumentNullException();
        }

        var userEntity = _mapper.Map<UserEntity>(userDto);

        if (userDto.access != null)
        {
            userEntity.access = new AccessEntity()
            {
                user = userDto.access.
            };
        }

        return userEntity;
    }

    public UserDto Convert(UserEntity userEntity)
    {
        if (userEntity == null)
        {
            throw new ArgumentNullException();
        }

        var userDto = _mapper.Map<UserDto>(userEntity);

        userEntity.access = userDto.access;
    }
}