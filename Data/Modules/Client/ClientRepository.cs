using lanstreamer_api.Data.Context;
using lanstreamer_api.Data.Modules.Client;
using lanstreamer_api.Data.Utils;
using lanstreamer_api.Entities;
using Microsoft.EntityFrameworkCore;

namespace lanstreamer_api.App.Client;

public class ClientRepository : BaseRepository<ClientEntity>, IClientRepository
{
    public ClientRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}