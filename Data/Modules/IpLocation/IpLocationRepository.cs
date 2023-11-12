using lanstreamer_api.Data.Context;
using lanstreamer_api.Data.Utils;

namespace lanstreamer_api.Data.Modules.IpLocation;

public class IpLocationRepository : BaseRepository<IpLocationEntity>
{
    public IpLocationRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}