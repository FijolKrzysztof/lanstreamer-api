using lanstreamer_api.Data.Context;
using lanstreamer_api.Data.Utils;
using Microsoft.EntityFrameworkCore;

namespace lanstreamer_api.Entities;

public class FeedbackRepository : BaseRepository<FeedbackEntity>
{
    public FeedbackRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}