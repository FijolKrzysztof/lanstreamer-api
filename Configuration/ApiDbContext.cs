using lanstreamer_api.Models;
using Microsoft.EntityFrameworkCore;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext>options) : base(options)
    {
        
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Authorization> Authorizations { get; set; }
    public DbSet<Configuration> Configurations { get; set; }
    public DbSet<Referrer> Referrers { get; set; }
}