using AutoMapper;
using lanstreamer_api.Data.Authentication;
using lanstreamer_api.Data.Modules.User;
using lanstreamer_api.Models;

namespace lanstreamer_api.App.Modules;

public class UserConverter
{
    private readonly IMapper _mapper;

    public UserConverter(IMapper mapper)
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

        if (userDto.Access != null)
        {
            userEntity.Access = _mapper.Map<AccessEntity>(userDto.Access);
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

        if (userEntity.Access != null)
        {
            userDto.Access = _mapper.Map<Data.Models.Access>(userEntity.Access);
        }

        return userDto;
    }
}