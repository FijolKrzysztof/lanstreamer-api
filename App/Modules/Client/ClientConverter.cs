using AutoMapper;
using lanstreamer_api.App.Data.Models;
using lanstreamer_api.Entities;
using lanstreamer_api.Models;

namespace lanstreamer_api.App.Client;

public class ClientConverter
{
    private readonly IMapper _mapper;

    public ClientConverter()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<ClientDto, ClientEntity>()
                .ForMember(entity => entity.OperatingSystem, opt =>
                {
                    opt.MapFrom(src => src.OperatingSystem.ToString());
                })
                .ReverseMap();
        }); // TODO: dokończyć mapowanie

        _mapper = new Mapper(config);
    }

    public ClientEntity Convert(ClientDto clientDto)
    {
        if (clientDto == null)
        {
            throw new ArgumentNullException(nameof(clientDto));
        }

        var clientEntity = _mapper.Map<ClientEntity>(clientDto);

        clientEntity.Feedbacks = clientDto.Feedbacks.Select(feedbackDto => new FeedbackEntity
        {
            Message = feedbackDto,
            ClientId = clientDto.Id
        }).ToList();
        
        return clientEntity;
    }

    public ClientDto Convert(ClientEntity clientEntity)
    {
        if (clientEntity == null)
        {
            throw new ArgumentNullException(nameof(clientEntity));
        }

        var clientDto = _mapper.Map<ClientDto>(clientEntity);

        clientDto.Feedbacks = clientEntity.Feedbacks.Select(feedbackEntity => feedbackEntity.Message).ToList();

        return clientDto;
    }
}