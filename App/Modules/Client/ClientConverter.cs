using AutoMapper;
using lanstreamer_api.Entities;
using lanstreamer_api.Models;

namespace lanstreamer_api.App.Client;

public class ClientConverter
{
    private readonly IMapper _mapper;
    
    ClientConverter(IMapper mapper)
    {
        _mapper = mapper;
    }
    
    public ClientEntity Convert(ClientDto clientDto)
    {
        if (clientDto == null)
        {
            throw new ArgumentNullException(nameof(clientDto));
        }

        var clientEntity = _mapper.Map<ClientEntity>(clientDto);

        if (clientDto.Feedbacks != null)
        {
            clientEntity.feedbacks = clientDto.Feedbacks.Select(feedbackDto => new FeedbackEntity
            {
                message = feedbackDto,
                clientId = clientDto.Id
            }).ToList();
        }

        return clientEntity;
    }

    public ClientDto Convert(ClientEntity clientEntity)
    {
        if (clientEntity == null)
        {
            throw new ArgumentNullException(nameof(clientEntity));
        }

        var clientDto = _mapper.Map<ClientDto>(clientEntity);

        clientDto.Feedbacks = clientEntity.feedbacks.Select(feedbackEntity => feedbackEntity.message).ToList();

        return clientDto;
    }
}