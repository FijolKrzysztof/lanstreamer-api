using lanstreamer_api.Data.Utils;

namespace lanstreamer_api.Data.Modules.IpLocation;

public interface IIpLocationRepository : IBaseRepository<IpLocationEntity>
{
    Task<IpLocationEntity?> GetByIp(string ip);
}