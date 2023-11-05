using lanstreamer_api.Data.Authentication;
using lanstreamer_api.Data.Configuration;
using lanstreamer_api.Data.Modules.User;
using lanstreamer_api.Entities;
using Microsoft.EntityFrameworkCore;

namespace lanstreamer_api.Data.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<ClientEntity> Clients { get; set; }
    public DbSet<FeedbackEntity> Feedbacks { get; set; }
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<AccessEntity> Accesses { get; set; }
    public DbSet<ConfigurationEntity> Configurations { get; set; }
}