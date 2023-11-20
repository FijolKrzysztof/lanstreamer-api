using AutoMapper;
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

        return _mapper.Map<UserEntity>(userDto);
    }

    public UserDto Convert(UserEntity userEntity)
    {
        if (userEntity == null)
        {
            throw new ArgumentNullException();
        }

        return _mapper.Map<UserDto>(userEntity);
    }
}