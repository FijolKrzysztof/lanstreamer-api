using lanstreamer_api.Data.Configuration;
using lanstreamer_api.Data.Modules.AccessCode;
using lanstreamer_api.Data.Modules.IpLocation;
using lanstreamer_api.Data.Modules.User;
using lanstreamer_api.Entities;
using Microsoft.EntityFrameworkCore;

namespace lanstreamer_api.Data.Context;

public class ApplicationDbContext : DbContext
{
    private readonly string defaultSchema;
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration) : base(options)
    {
        defaultSchema = configuration.GetConnectionString("Schema");
    }
    
    public DbSet<ClientEntity> Clients { get; set; }
    public DbSet<FeedbackEntity> Feedbacks { get; set; }
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<ConfigurationEntity> Configurations { get; set; }
    public DbSet<IpLocationEntity> IpLocations { get; set; }
    public DbSet<AccessEntity> Accesses { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(defaultSchema);
    }
}