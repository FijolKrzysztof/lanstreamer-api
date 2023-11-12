using System.Net;
using lanstreamer_api.App.Data.Models;
using lanstreamer_api.App.Data.Models.Enums;
using lanstreamer_api.App.Exceptions;
using lanstreamer_api.Data.Authentication;
using lanstreamer_api.Models;
using Newtonsoft.Json;
using OperatingSystem = lanstreamer_api.App.Data.Models.Enums.OperatingSystem;

namespace lanstreamer_api.App.Client;

public class ClientService
{
    private readonly ClientRepository _clientRepository;
    private readonly ClientConverter _clientConverter;
    private readonly AccessRepository _accessRepository;
    private readonly ILogger<ClientService> _logger;

    public ClientService(
        ILogger<ClientService> logger,
        ClientConverter clientConverter,
        ClientRepository clientRepository,
        AccessRepository accessRepository
    )
    {
        _logger = logger;
        _clientRepository = clientRepository;
        _clientConverter = clientConverter;
        _accessRepository = accessRepository;
    }

    public async Task<ClientDto> CreateClient(ClientDto clientDto, string xForwardedFor, string userAgent,
        string acceptLanguage)
    {
        var ipAddress = GetIpAddress(xForwardedFor);
        var operatingSystem = GetOs(userAgent);
        var defaultLanguage = GetDefaultLanguage(acceptLanguage);
        var client = _clientConverter.Convert(clientDto);

        client.VisitTime = DateTime.Now;
        client.TimeOnSite = TimeSpan.Zero;

        if (ipAddress != null)
        {
            client.IpLocation = await GetIpLocation(ipAddress);
        }

        if (defaultLanguage != null)
        {
            client.Language = defaultLanguage;
        }

        client.OperatingSystem = operatingSystem;

        var clientEntity = _clientConverter.ConvertToEntity(client);
        var createdClientEntity = await _clientRepository.Create(clientEntity);
        var createdClient = _clientConverter.Convert(createdClientEntity);
        var createdClientDto = _clientConverter.ConvertToDto(createdClient);
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

    public async Task CleanupOldAccessRecords()
    {
        await _accessRepository.DeleteRecordsOlderThan(DateTime.UtcNow.AddHours(-1));
    }

    private string? GetIpAddress(string xForwardedForHeader)
    {
        if (string.IsNullOrEmpty(xForwardedForHeader))
        {
            return null;
        }

        var ipAddresses = xForwardedForHeader.Split(',');

        return ipAddresses[0].Trim();
    }

    private OperatingSystem GetOs(string userAgent)
    {
        if (userAgent.Contains("Windows", StringComparison.OrdinalIgnoreCase))
        {
            return OperatingSystem.Windows;
        }

        if (userAgent.Contains("Mac OS", StringComparison.OrdinalIgnoreCase))
        {
            return OperatingSystem.MacOS;
        }

        if (userAgent.Contains("Linux", StringComparison.OrdinalIgnoreCase))
        {
            return OperatingSystem.Linux;
        }

        if (userAgent.Contains("Android", StringComparison.OrdinalIgnoreCase))
        {
            return OperatingSystem.Android;
        }

        if (userAgent.Contains("iOS", StringComparison.OrdinalIgnoreCase))
        {
            return OperatingSystem.IOS;
        }

        return OperatingSystem.Other;
    }

    private string? GetDefaultLanguage(string acceptLanguage)
    {
        if (string.IsNullOrEmpty(acceptLanguage))
        {
            return null;
        }

        var languages = acceptLanguage.Split(',');
        var firstLanguage = languages[0]?.Trim();
        return firstLanguage?.Split('-')[0];
    }

    private async Task<IpLocation> GetIpLocation(string ip)
    {
        var ipLocation = new IpLocation()
        {
            Ip = ip,
        };
        try
        {
            var client = new HttpClient();
            var response = await client.GetStringAsync($"http://ipinfo.io/{ip}");

            ipLocation = JsonConvert.DeserializeObject<IpLocation>(response) ?? ipLocation;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while fetching IP location for address {IpAddress}. Details: {ErrorMessage}", ip, e.Message);
        }

        return ipLocation;
    }
}