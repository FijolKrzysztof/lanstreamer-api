using lanstreamer_api.App.Modules;

namespace lanstreamer_api.App.Workers;

public class CleanupScheduler : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private Timer? _timer;

    public CleanupScheduler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    private async void Execute(object? _)
    {
        await Task.Run(async () =>
        {
            var scope = _serviceProvider.CreateScope();
            var scopedUserService = scope.ServiceProvider.GetRequiredService<UserService>();
            await scopedUserService.CleanupOldAccessRecords();
        });
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(Execute, null, 0, TimeSpan.FromHours(1).Milliseconds);
        return Task.CompletedTask;
        
        // TODO: ustalić czy przenieść część konfiguracji z bazy do appsettings
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }
}