using lanstreamer_api.Data.Utils;

namespace lanstreamer_api.Data.Modules.AccessCode;

public interface IAccessRepository : IBaseRepository<AccessEntity>
{
    Task<AccessEntity?> FindByCode(string code);
    Task<AccessEntity?> FindByUserId(int userId);
    Task<IEnumerable<AccessEntity>> GetExpiredRecords();
}