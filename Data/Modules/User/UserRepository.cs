using lanstreamer_api.Data.Context;
using lanstreamer_api.Data.Utils;
using Microsoft.EntityFrameworkCore;

namespace lanstreamer_api.Data.Modules.User;

public class UserRepository : BaseRepository<UserEntity>, IUserRepository
{
    public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<UserEntity?> GetByGoogleId(string googleId)
    {
        return await dbContext.Set<UserEntity>().FirstOrDefaultAsync(entity => entity.GoogleId == googleId);
    }
    
    public async Task<UserEntity> UpdateOrCreate(UserEntity entity)
    {
        var existingEntity = await GetById(entity.Id);

        if (existingEntity != null)
        {
            entity = await Update(entity);
        }
        else
        {
            entity = await Create(entity);
        }

        await dbContext.SaveChangesAsync();
        return entity;
    }
}
