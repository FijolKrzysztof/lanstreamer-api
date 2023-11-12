using AutoMapper;
using lanstreamer_api.Entities;
using lanstreamer_api.Models;

namespace lanstreamer_api.App.Client;

public class ClientConverter
{
    private readonly IMapper _mapper;
    
    public ClientConverter(IMapper mapper)
    {
        _mapper = mapper;
    }

    public Data.Models.Client Convert(ClientDto clientDto)
    {
        if (clientDto == null)
        {
            throw new ArgumentNullException(nameof(clientDto));
        }
        
        return _mapper.Map<Data.Models.Client>(clientDto);
    }

    public ClientEntity ConvertToEntity(Data.Models.Client client)
    {
        if (client == null)
        {
            throw new ArgumentNullException(nameof(client));
        }

        var clientEntity = _mapper.Map<ClientEntity>(client);

        if (client.Feedbacks is { Count: > 0 })
        {
            clientEntity.Feedbacks = client.Feedbacks.Select(feedbackDto => new FeedbackEntity
            {
                Message = feedbackDto,
                ClientId = client.Id
            }).ToList();
        }

        return clientEntity;
    }

    public ClientDto ConvertToDto(Data.Models.Client client)
    {
        if (client == null)
        {
            throw new ArgumentNullException(nameof(client));
        }

        return _mapper.Map<ClientDto>(client);
    }

    public Data.Models.Client Convert(ClientEntity clientEntity)
    {
        if (clientEntity == null)
        {
            throw new ArgumentNullException(nameof(clientEntity));
        }

        var clientDto = _mapper.Map<Data.Models.Client>(clientEntity);

        clientDto.Feedbacks = clientEntity.Feedbacks.Select(feedbackEntity => feedbackEntity.Message).ToList();

        return clientDto;
    }
}