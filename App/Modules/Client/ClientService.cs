using System.Net;
using lanstreamer_api.App.Data.Dto;
using lanstreamer_api.App.Exceptions;
using lanstreamer_api.Data.Modules.Client;
using lanstreamer_api.Entities;
using lanstreamer_api.Models;
using lanstreamer_api.services;
using lanstreamer_api.services.FileService;
using OperatingSystem = lanstreamer_api.App.Data.Models.Enums.OperatingSystem;

namespace lanstreamer_api.App.Client;

public class ClientService
{
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IClientConverter _clientConverter;
    private readonly IHttpRequestInfoService _httpRequestInfoService;
    private readonly IFileService _fileService;

    public ClientService(
        IFeedbackRepository feedbackRepository,
        IClientConverter clientConverter,
        IClientRepository clientRepository,
        IHttpRequestInfoService httpRequestInfoService,
        IFileService fileService
    )
    {
        _feedbackRepository = feedbackRepository;
        _fileService = fileService;
        _clientRepository = clientRepository;
        _clientConverter = clientConverter;
        _httpRequestInfoService = httpRequestInfoService;
    }

    public async Task<CreatedObjResponse> CreateClient(ClientDto clientDto, HttpContext httpContext)
    {
        var client = _clientConverter.Convert<Data.Models.Client>(clientDto);

        var ipAddress = _httpRequestInfoService.GetIpAddress(httpContext);
        var operatingSystem = _httpRequestInfoService.GetOs(httpContext);
        var defaultLanguage = _httpRequestInfoService.GetDefaultLanguage(httpContext);

        client.VisitTime = DateTime.Now.ToUniversalTime();
        client.TimeOnSite = TimeSpan.Zero;

        if (ipAddress != null)
        {
            client.IpLocation = await _httpRequestInfoService.GetIpLocation(ipAddress);
        }

        if (defaultLanguage != null)
        {
            client.Language = defaultLanguage;
        }

        client.OperatingSystem = operatingSystem;

        var clientEntity = _clientConverter.Convert<ClientEntity>(client);
        var createdClientEntity = await _clientRepository.Create(clientEntity);
        var createdClientDto = _clientConverter.ChainConvert<Data.Models.Client>(createdClientEntity).To<ClientDto>();
        return new CreatedObjResponse()
        {
            Id = createdClientDto.Id,
        };
    }

    public async Task AddFeedbacks(int clientId, string feedback)
    {
        var clientEntity = await _clientRepository.GetById(clientId);

        if (clientEntity == null)
        {
            throw new AppException(HttpStatusCode.NotFound, $"Client with id: {clientId} not found");
        }

        var client = _clientConverter.Convert<Data.Models.Client>(clientEntity);

        client.Feedbacks ??= new List<string>();
        client.Feedbacks.Add(feedback);

        var newClientEntity = _clientConverter.Convert<ClientEntity>(client);

        await _feedbackRepository.Create(newClientEntity.Feedbacks!.ElementAt(0));
    }

    public async Task UpdateSessionDuration(int clientId)
    {
        var clientEntity = await _clientRepository.GetById(clientId);

        if (clientEntity == null)
        {
            throw new AppException(HttpStatusCode.NotFound, $"Client with id: {clientId} not found");
        }

        clientEntity.TimeOnSite = DateTime.Now.ToUniversalTime() - clientEntity.VisitTime;

        await _clientRepository.Update(clientEntity);
    }

    public async Task<FileStream> GetFileStream(int clientId, OperatingSystem operatingSystem)
    {
        var clientEntity = await _clientRepository.GetById(clientId);
        if (clientEntity == null)
        {
            throw new AppException(HttpStatusCode.NotFound, $"Client with ID {clientId} doesn't exist");
        }

        var filePath = await _fileService.GetDesktopAppPath(operatingSystem);
        if (!_fileService.Exists(filePath))
        {
            throw new AppException(HttpStatusCode.NotFound, "File not found");
        }

        var fileStream = _fileService.ReadFileStream(filePath);

        clientEntity.Downloads = clientEntity.Downloads == null ? 1 : clientEntity.Downloads + 1;

        await _clientRepository.Update(clientEntity);

        return fileStream;
    }
}