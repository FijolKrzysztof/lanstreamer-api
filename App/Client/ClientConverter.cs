using lanstreamer_api.Entities;
using lanstreamer_api.Models;

namespace lanstreamer_api.App.Client;

public class ClientConverter
{
    public ClientEntity Convert(ClientDto clientDto)
    {
        if (clientDto == null)
        {
            throw new ArgumentNullException(nameof(clientDto));
        }

        var clientEntity = new ClientEntity
        {
            id = clientDto.id,
            visitTime = clientDto.visitTime,
            operatingSystem = clientDto.operatingSystem,
            defaultLanguage = clientDto.defaultLanguage,
            timeOnSite = clientDto.timeOnSite,
            referrerPage = clientDto.referrerPage,
            downloads = clientDto.downloads
        };

        if (clientDto.feedbacks != null)
        {
            clientEntity.feedbacks = clientDto.feedbacks.Select(feedbackDto => new FeedbackEntity
            {
                message = feedbackDto,
                clientId = clientDto.id
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

        var clientDto = new ClientDto
        {
            id = clientEntity.id,
            visitTime = clientEntity.visitTime,
            operatingSystem = clientEntity.operatingSystem,
            defaultLanguage = clientEntity.defaultLanguage,
            timeOnSite = clientEntity.timeOnSite,
            referrerPage = clientEntity.referrerPage,
            downloads = clientEntity.downloads
        };

        clientDto.feedbacks = clientEntity.feedbacks.Select(feedbackEntity => feedbackEntity.message).ToList();

        return clientDto;
    }
}