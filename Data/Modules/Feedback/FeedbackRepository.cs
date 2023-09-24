using lanstreamer_api.Data.Context;
using lanstreamer_api.Data.Utils;

namespace lanstreamer_api.Entities;

public class FeedbackRepository : BaseRepository<FeedbackEntity>
{
    FeedbackRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}