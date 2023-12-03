using System.Net;
using lanstreamer_api.App.Data.Models.Enums;
using lanstreamer_api.App.Exceptions;
using lanstreamer_api.Data.Modules.Client;
using lanstreamer_api.Models;
using lanstreamer_api.services;
using OperatingSystem = lanstreamer_api.App.Data.Models.Enums.OperatingSystem;

namespace lanstreamer_api.App.Client;

public class ClientService
{
    private readonly IClientRepository _clientRepository;
    private readonly ClientConverter _clientConverter;
    private readonly HttpRequestInfoService _httpRequestInfoService;

    public ClientService(
        ClientConverter clientConverter,
        IClientRepository clientRepository,
        HttpRequestInfoService httpRequestInfoService
    )
    {
        _clientRepository = clientRepository;
        _clientConverter = clientConverter;
        _httpRequestInfoService = httpRequestInfoService;
    }

    public async Task<ClientDto> CreateClient(ClientDto clientDto, HttpContext httpContext)
    {
        var ipAddress = _httpRequestInfoService.GetIpAddress(httpContext);
        var operatingSystem = _httpRequestInfoService.GetOs(httpContext);
        var defaultLanguage = _httpRequestInfoService.GetDefaultLanguage(httpContext);

        clientDto.VisitTime = DateTime.Now.ToUniversalTime();
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
        var clientEntity = clientDto.Id.HasValue ? await _clientRepository.GetById(clientDto.Id.Value) : null;

        if (clientEntity == null)
        {
            throw new AppException(HttpStatusCode.NotFound, $"Client with id: {clientDto.Id} not found");
        }

        var newClientEntity = _clientConverter.Convert(clientDto);

        clientEntity.Feedbacks = newClientEntity.Feedbacks;

        var updatedClientEntity = await _clientRepository.Update(clientEntity);
        var updatedClientDto = _clientConverter.Convert(updatedClientEntity);
        return updatedClientDto;
    }

    public async Task UpdateSessionDuration(int clientId)
    {
        var clientEntity = await _clientRepository.GetById(clientId);

        if (clientEntity == null)
        {
            throw new AppException(HttpStatusCode.NotFound, $"Client with id: {clientId} not found");
        }

        clientEntity.TimeOnSite = DateTime.Now - clientEntity.VisitTime;

        await _clientRepository.Update(clientEntity);
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