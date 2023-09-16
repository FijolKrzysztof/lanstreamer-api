using lanstreamer_api.Models;

namespace lanstreamer_api.App.Client;

public class ClientService
{
    private readonly ClientRepository _clientRepository;
    private readonly ClientConverter _clientConverter;

    ClientService(ClientConverter clientConverter, ClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
        _clientConverter = clientConverter;
    }

    public async Task<ClientDto> CreateClient(ClientDto clientDto)
    {
        var clientEntity = _clientConverter.Convert(clientDto);
        var createdClientEntity = await _clientRepository.CreateAsync(clientEntity);
        var createdClientDto = _clientConverter.Convert(createdClientEntity);
        return createdClientDto;
    }

    public async Task<ClientDto> UpdateClient(ClientDto clientDto)
    {
        var clientEntity = _clientConverter.Convert(clientDto);
        var updatedClientEntity = await _clientRepository.UpdateAsync(clientEntity);
        var updatedClientDto = _clientConverter.Convert(updatedClientEntity);
        return updatedClientDto;
    }
}