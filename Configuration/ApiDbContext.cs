using lanstreamer_api.Models;
using Microsoft.EntityFrameworkCore;

namespace lanstreamer_api.Configuration;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext>options) : base(options)
    {
        
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Authorization> Authorizations { get; set; }
}