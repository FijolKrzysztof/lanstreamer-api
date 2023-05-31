using lanstreamer_api.services;
using Microsoft.EntityFrameworkCore;

namespace lanstreamer_api;

using MongoDB.Driver;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    private IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCors(options => options.AddPolicy("allowAll", builder =>
        {
            builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
        }));

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        
        // Configure MongoDB connection
        string mongoConnectionString = Configuration.GetConnectionString("db_connection");
        services.AddSingleton<IMongoClient>(_ => new MongoClient(mongoConnectionString));
        services.AddScoped<IMongoDatabase>(serviceProvider =>
        {
            var client = serviceProvider.GetRequiredService<IMongoClient>();
            return client.GetDatabase("lanstreamer");
        });
        
        services.AddSingleton<AmazonS3Service>();
        services.AddSingleton<BlogService>();
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
        // app.UseHttpsRedirection();
        app.UseRouting();
        // app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
