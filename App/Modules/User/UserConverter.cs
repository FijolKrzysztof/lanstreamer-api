using AutoMapper;
using lanstreamer_api.App.Data.Models;
using lanstreamer_api.Data.Modules.AccessCode;
using lanstreamer_api.Data.Modules.IpLocation;
using lanstreamer_api.Data.Modules.User;
using lanstreamer_api.Models;
using lanstreamer_api.services.Abstract;

namespace lanstreamer_api.App.Modules;

public class UserConverter : BaseConverter, IUserConverter
{
    public UserConverter() : base(CreateMapperConfiguration())
    {
    }
    
    private static MapperConfiguration CreateMapperConfiguration()
    {
        return new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<User, UserEntity>().ReverseMap();
            cfg.CreateMap<UserDto, User>()
                .ForMember(dest => dest.Access, opt =>
                {
                    opt.MapFrom(src => new Data.Models.Access()
                    {
                        Code = src.AccessCode,
                    });
                }).ReverseMap();
            cfg.CreateMap<IpLocation, IpLocationEntity>().ReverseMap();
            cfg.CreateMap<Data.Models.Access, AccessEntity>().ReverseMap();
        });
    }
}