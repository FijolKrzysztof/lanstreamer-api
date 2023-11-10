using Amazon.Runtime.Internal.Util;
using lanstreamer_api.App.Client;
using lanstreamer_api.App.Middleware;
using lanstreamer_api.App.Modules;
using lanstreamer_api.App.Modules.Access;
using lanstreamer_api.App.Modules.Admin;
using lanstreamer_api.App.Modules.Legacy;
using lanstreamer_api.App.Workers;
using lanstreamer_api.Data.Authentication;
using lanstreamer_api.Data.Configuration;
using lanstreamer_api.Data.Context;
using lanstreamer_api.Data.Modules.User;
using lanstreamer_api.Entities;
using lanstreamer_api.services;
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

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddMvc().AddXmlSerializerFormatters(); // TODO: zastanowić się czy jednak nie usunąć
        services.AddControllers();
        
        // TODO: dodać do Development appsettings testową bazę danych - czyli w pscale czy jak się to nazywalo trzeba dodać kolejną bazę danych ale tylko do testów

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(Configuration.GetConnectionString("Database"));
        });
        services.AddAutoMapper(_ => { });
        
        services.AddHostedService<CleanupScheduler>();

        services.AddScoped<AccessRepository>();
        services.AddScoped<ClientRepository>();
        services.AddScoped<ConfigurationRepository>();
        services.AddScoped<FeedbackRepository>();
        services.AddScoped<UserRepository>();

        services.AddScoped<UserConverter>();
        services.AddScoped<ClientConverter>();
        
        services.AddScoped<LegacyService>();
        services.AddScoped<DesktopAppService>();
        services.AddScoped<AdminService>();
        services.AddScoped<UserService>();
        services.AddScoped<ClientService>();
        
        services.AddSingleton<ServerSentEventsService<bool>>();
    }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    private IConfiguration Configuration { get; }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage(); // TODO: rozważyć czy nie dorzucić i zmienić - bo trzeba zrobić dobrą konfigurację na development i produkcję
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseGoogleSignInMiddleware();
        app.UseCors("allowAll");
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}