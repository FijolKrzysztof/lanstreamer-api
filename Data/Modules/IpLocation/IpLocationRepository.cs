using lanstreamer_api.Data.Context;
using lanstreamer_api.Data.Utils;
using Microsoft.EntityFrameworkCore;

namespace lanstreamer_api.Data.Modules.IpLocation;

public class IpLocationRepository : BaseRepository<IpLocationEntity>, IIpLocationRepository
{
    public IpLocationRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
    
    public async Task<IpLocationEntity?> GetByIp(string ip)
    {
        return await dbContext.Set<IpLocationEntity>().FirstOrDefaultAsync(x => x.Ip == ip);
    }
}