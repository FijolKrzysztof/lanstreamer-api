using lanstreamer_api.Data.Context;
using lanstreamer_api.services;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MySql.Data.MySqlClient;

namespace lanstreamer_api;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    private IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("allowAll", builder =>
            {
                builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
            });
        });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseMySql(Configuration.GetConnectionString("Database"),
                new MySqlServerVersion(new Version(8, 0, 25)));
        });
        
        services.AddSingleton<AmazonS3Service>();
        services.AddScoped<MainService>();
        services.AddMvc().AddXmlSerializerFormatters();
        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors("allowAll");
        app.UseRouting();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}