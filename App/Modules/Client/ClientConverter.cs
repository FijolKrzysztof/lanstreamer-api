using AutoMapper;
using lanstreamer_api.App.Data.Models;
using lanstreamer_api.Data.Modules.IpLocation;
using lanstreamer_api.Entities;
using lanstreamer_api.Models;
using lanstreamer_api.services.Abstract;

namespace lanstreamer_api.App.Client;

public class ClientConverter : BaseConverter, IClientConverter
{
    public ClientConverter() : base(CreateMapperConfiguration())
    {
    }
    
    private static MapperConfiguration CreateMapperConfiguration()
    {
        return new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Data.Models.Client, ClientEntity>()
                .ForMember(entity => entity.Feedbacks, opt =>
                {
                    opt.MapFrom(client => client.Feedbacks.Select(feedback => new FeedbackEntity
                    {
                        ClientId = client.Id,
                        Message = feedback,
                    }));
                })
                .ReverseMap()
                .ForMember(client => client.Feedbacks, opt =>
                {
                    opt.MapFrom(entity => entity.Feedbacks.Select(feedback => feedback.Message).ToList());
                });

            cfg.CreateMap<ClientDto, Data.Models.Client>().ReverseMap();
            cfg.CreateMap<IpLocation, IpLocationEntity>().ReverseMap();
        });
    }
}