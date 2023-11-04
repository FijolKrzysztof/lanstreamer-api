using lanstreamer_api.App.Client;
using lanstreamer_api.Data.Authentication;

namespace lanstreamer_api.App.Workers;

public class CleanupScheduler
{
    private readonly ILogger<CleanupScheduler> _logger;
    private readonly Timer _accessTimer;
    private readonly ClientService _clientService;
    
    public CleanupScheduler(ILogger<CleanupScheduler> logger, ClientService clientService)
    {
        _clientService = clientService;
        _logger = logger;
        _accessTimer = new Timer(CleanupOldAccessRecords, null, 0, TimeSpan.FromDays(1).Milliseconds);
        
        LogTimerInfo();
    }
    
    private async void CleanupOldAccessRecords(object? state)
    {
        await Task.Run(async () =>
        {
            await _clientService.CleanupOldAccessRecords();
        });
    }

    private void LogTimerInfo()
    {
        if (_accessTimer?.Change(1, -1) ?? false)
        {
            _logger.LogInformation("AccessTimer is active");
        }
    }
}