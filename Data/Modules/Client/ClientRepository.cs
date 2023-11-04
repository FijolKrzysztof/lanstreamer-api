using lanstreamer_api.Data.Context;
using lanstreamer_api.Data.Utils;
using lanstreamer_api.Entities;
using Microsoft.EntityFrameworkCore;

namespace lanstreamer_api.App.Client;

public class ClientRepository : BaseRepository<ClientEntity>
{
    public ClientRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}