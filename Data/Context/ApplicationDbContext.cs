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
}