using lanstreamer_api.App.Attributes;
using lanstreamer_api.App.Client;
using lanstreamer_api.App.Middleware;
using lanstreamer_api.App.Modules;
using lanstreamer_api.App.Modules.Access;
using lanstreamer_api.App.Modules.Admin;
using lanstreamer_api.App.Modules.Shared.GoogleAuthenticationService;
using lanstreamer_api.App.Workers;
using lanstreamer_api.Data.Configuration;
using lanstreamer_api.Data.Context;
using lanstreamer_api.Data.Modules.Client;
using lanstreamer_api.Data.Modules.IpLocation;
using lanstreamer_api.Data.Modules.User;
using lanstreamer_api.Entities;
using lanstreamer_api.services;
using lanstreamer_api.services.FileService;
using Microsoft.EntityFrameworkCore;

namespace lanstreamer_api;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("allowAll", builder =>
            {
                builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader(); // TODO: to raczej dać tylko do access method i podczas testowania
            });
        });

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(Configuration.GetConnectionString("Database"));
        });
        
        services.AddSwaggerGen();
        services.AddControllers();

        // TODO: dodać do Development appsettings testową bazę danych - czyli w pscale czy jak się to nazywalo trzeba dodać kolejną bazę danych ale tylko do testów

        services.AddAutoMapper(typeof(Startup));
        services.AddHttpContextAccessor();
        
        // services.AddHostedService<CleanupScheduler>(); // TODO: odkomentować

        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<ConfigurationRepository>();
        services.AddScoped<IFeedbackRepository, FeedbackRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IIpLocationRepository, IpLocationRepository>();

        services.AddScoped<IUserConverter, UserConverter>();
        services.AddScoped<IClientConverter, ClientConverter>();
        
        services.AddScoped<DesktopAppService>();
        services.AddScoped<AdminService>();
        services.AddScoped<UserService>();
        services.AddScoped<ClientService>();

        services.AddScoped<IGoogleAuthenticationService, GoogleAuthenticationService>();
        services.AddScoped<IHttpRequestInfoService, HttpRequestInfoService>();
        services.AddSingleton<IServerSentEventsService<bool>, ServerSentEventsService<bool>>();
    }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    private IConfiguration Configuration { get; }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseCors("allowAll"); // TODO: trzeba to zdjąć i enablować tylko lokalnie i w przypadku metody AppAccess
        app.UseRouting();
        app.UseMiddleware<ErrorHandlingMiddleware>();
        app.UseMiddleware<GoogleSignInMiddleware>();
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}