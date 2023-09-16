using lanstreamer_api.Data.Context;
using lanstreamer_api.Data.Utils;

namespace lanstreamer_api.Entities;

public class FeedbackRepository : AbstractRepository<FeedbackEntity>
{
    FeedbackRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}