using System.Net;
using lanstreamer_api.App.Data.Models.Enums;
using lanstreamer_api.App.Exceptions;
using lanstreamer_api.Models;
using lanstreamer_api.services;
using OperatingSystem = lanstreamer_api.App.Data.Models.Enums.OperatingSystem;

namespace lanstreamer_api.App.Client;

public class ClientService
{
    private readonly ClientRepository _clientRepository;
    private readonly ClientConverter _clientConverter;
    private readonly AmazonS3Service _amazonS3Service;

    ClientService(ClientConverter clientConverter, ClientRepository clientRepository, AmazonS3Service amazonS3Service)
    {
        _clientRepository = clientRepository;
        _clientConverter = clientConverter;
        _amazonS3Service = amazonS3Service;
    }

    public async Task<ClientDto> CreateClient(ClientDto clientDto)
    {
        var clientEntity = _clientConverter.Convert(clientDto);
        var createdClientEntity = await _clientRepository.Create(clientEntity);
        var createdClientDto = _clientConverter.Convert(createdClientEntity);
        return createdClientDto;
    }

    public async Task<ClientDto> UpdateClient(ClientDto clientDto)
    {
        var clientEntity = _clientConverter.Convert(clientDto);
        var updatedClientEntity = await _clientRepository.Update(clientEntity);
        var updatedClientDto = _clientConverter.Convert(updatedClientEntity);
        return updatedClientDto;
    }

    public async Task<byte[]> GetFile(int clientId, OperatingSystem operatingSystem)
    {
        var clientEntity = await _clientRepository.GetById(clientId);
        if (clientEntity == null)
        {
            throw new AppException(HttpStatusCode.Unauthorized, $"Client with ID {clientId} doesn't exist");
        }
        
        var filePath = ApplicationBuildPath.GetPath(operatingSystem);
        if (!File.Exists(filePath))
        {
            throw new AppException(HttpStatusCode.NotFound, "File not found");
        }

        var fileBytes = await File.ReadAllBytesAsync(filePath);
        
        clientEntity.Downloads++;
        await _clientRepository.Update(clientEntity);

        return fileBytes;
    }
}