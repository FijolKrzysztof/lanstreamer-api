using lanstreamer_api.Data.Context;
using lanstreamer_api.Data.Utils;

namespace lanstreamer_api.Data.Modules.User;

public class UserRepository : BaseRepository<UserEntity>
{
    public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}