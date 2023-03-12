using lanstreamer_api.services;
using Microsoft.EntityFrameworkCore;

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
        services.AddCors(options => options.AddPolicy("allowAll", builder =>
        {
            builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
        }));
        
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddDbContext<ApiDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString("db_connection")));
        services.AddMvc().AddXmlSerializerFormatters();
        services.AddControllers();

        services.AddSingleton<AmazonS3Service>();
        services.AddSingleton<BlogService>();
        services.AddScoped<MainService>();
    }
    
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApiDbContext apiDbContext)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors("allowAll");
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}