using lanstreamer_api.Data.Utils;

namespace lanstreamer_api.Data.Modules.User;

public interface IUserRepository : IBaseRepository<UserEntity>
{
    Task<UserEntity?> GetByGoogleId(string googleId);
    Task<UserEntity?> GetByAccessCode(string accessCode);
    Task<UserEntity> UpdateOrCreate(UserEntity entity);
    Task RemoveAccessCodeOlderThan(DateTime dateTime);
}