using lanstreamer_api.Data.Authentication;

namespace lanstreamer_api.App.Workers;

public class AccessCleanupScheduler
{
    private readonly AccessRepository _accessRepository;
    private Timer _timer;
    
    public AccessCleanupScheduler(AccessRepository accessRepository)
    {
        _accessRepository = accessRepository;
        _timer = new Timer(CleanupOldRecords, null, 0, TimeSpan.FromDays(1).Milliseconds);
    }
    
    private async void CleanupOldRecords(object? state)
    {
        var cutoffTime = DateTime.Now.AddHours(-1);

        await Task.Run(async () =>
        {
            await _accessRepository.DeleteRecordsOlderThan(cutoffTime);
        });
    }
}