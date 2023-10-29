using System.Net;
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

    public async Task<DownloadResponse> Download(int clientId, OperatingSystem operatingSystem)
    {
        var s3Objects = (await _amazonS3Service.GetObjectList("downloads"))
            .FindAll(obj => obj.Key.Contains(operatingSystem.ToString().ToLower()))
            .FindAll(obj => _amazonS3Service.GetFilePermissions(obj.Key).Result
                .FindAll(grant => grant.Grantee.URI == "http://acs.amazonaws.com/groups/global/AllUsers"
                                  && (grant.Permission.Value == "READ" || grant.Permission.Value == "READ_ACP"))
                .Count == 2);

        s3Objects.Sort((obj1, obj2) => obj2.LastModified.CompareTo(obj1.LastModified));
        var s3Object = s3Objects.First();
        
        var clientEntity = await _clientRepository.GetByIdAsync(clientId);
        if (clientEntity == null)
        {
            throw new AppException(HttpStatusCode.NotFound, $"Client with ID {clientId} not found");
        }
        clientEntity.Downloads++;
        await _clientRepository.UpdateAsync(clientEntity);

        return new DownloadResponse()
        {
            Link = $"https://lanstreamer.s3.eu-west-2.amazonaws.com/{s3Object.Key}"
        }; 
    }
}