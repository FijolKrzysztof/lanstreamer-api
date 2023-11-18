using System.Net;
using lanstreamer_api.App.Data.Models;
using lanstreamer_api.App.Data.Models.Enums;
using lanstreamer_api.App.Exceptions;
using lanstreamer_api.Data.Authentication;
using lanstreamer_api.Models;
using lanstreamer_api.services;
using Newtonsoft.Json;
using OperatingSystem = lanstreamer_api.App.Data.Models.Enums.OperatingSystem;

namespace lanstreamer_api.App.Client;

public class ClientService
{
    private readonly ClientRepository _clientRepository;
    private readonly ClientConverter _clientConverter;
    private readonly HttpRequestInfoService _httpRequestInfoService;

    public ClientService(
        ClientConverter clientConverter,
        ClientRepository clientRepository,
        HttpRequestInfoService httpRequestInfoService
    )
    {
        _clientRepository = clientRepository;
        _clientConverter = clientConverter;
        _httpRequestInfoService = httpRequestInfoService;
    }

    public async Task<ClientDto> CreateClient(ClientDto clientDto, string xForwardedFor, string userAgent,
        string acceptLanguage)
    {
        var ipAddress = _httpRequestInfoService.GetIpAddress(xForwardedFor);
        var operatingSystem = _httpRequestInfoService.GetOs(userAgent);
        var defaultLanguage = _httpRequestInfoService.GetDefaultLanguage(acceptLanguage);

        clientDto.VisitTime = DateTime.Now;
        clientDto.TimeOnSite = TimeSpan.Zero;

        if (ipAddress != null)
        {
            clientDto.IpLocation = await _httpRequestInfoService.GetIpLocation(ipAddress);
        }

        if (defaultLanguage != null)
        {
            clientDto.Language = defaultLanguage;
        }

        clientDto.OperatingSystem = operatingSystem;

        var clientEntity = _clientConverter.Convert(clientDto);
        var createdClientEntity = await _clientRepository.Create(clientEntity);
        var createdClientDto = _clientConverter.Convert(createdClientEntity);
        return createdClientDto;
    }

    public async Task<ClientDto> UpdateClient(ClientDto clientDto)
    {
        var clientEntity = _clientConverter.Convert(clientDto);

        clientEntity.TimeOnSite = DateTime.Now - clientEntity.VisitTime;
        
        var updatedClientEntity = await _clientRepository.Update(clientEntity);
        var updatedClientDto = _clientConverter.Convert(updatedClientEntity);
        return updatedClientDto;
    }
    
    public async Task<byte[]> GetFile(int clientId, OperatingSystem operatingSystem)
    {
        var clientEntity = await _clientRepository.GetById(clientId);
        if (clientEntity == null)
        {
            throw new AppException(HttpStatusCode.NotFound, $"Client with ID {clientId} doesn't exist");
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