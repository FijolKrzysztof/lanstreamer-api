using lanstreamer_api.Data.Context;
using lanstreamer_api.Data.Utils;
using lanstreamer_api.Entities;

namespace lanstreamer_api.App.Client;

public class ClientRepository : AbstractRepository<ClientEntity>
{
    ClientRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}