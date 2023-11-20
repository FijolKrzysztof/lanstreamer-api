using lanstreamer_api.Data.Context;
using lanstreamer_api.Data.Utils;
using Microsoft.EntityFrameworkCore;

namespace lanstreamer_api.Data.Modules.User;

public class UserRepository : BaseRepository<UserEntity>
{
    public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<UserEntity?> GetByGoogleId(string googleId)
    {
        return await dbContext.Set<UserEntity>().FirstOrDefaultAsync(entity => entity.GoogleId == googleId);
    }

    public async Task<UserEntity?> GetByAccessCode(string accessCode)
    {
        return await dbContext.Set<UserEntity>().FirstOrDefaultAsync(entity => entity.AccessCode == accessCode);
    }
    
    public async Task RemoveAccessCodeOlderThan(DateTime dateTime)
    {
        var entities = await dbContext.Set<UserEntity>()
            .Where(entity => entity.LastLogin < dateTime)
            .ToListAsync();
        
        foreach (var entity in entities)
        {
            entity.AccessCode = null;
            dbContext.Entry(entity).State = EntityState.Modified;
        }
        
        await dbContext.SaveChangesAsync();
    }
}
