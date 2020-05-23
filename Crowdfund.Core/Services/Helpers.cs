using Crowdfund.Core.Data;
using Crowdfund.Core.Models;
using System.Linq;

namespace Crowdfund.Core.Services
{
    public static class Helpers
    {
        public static bool UserOwnsProject(DataContext dbCtx ,int? userId, int? projectId)
        {
            return dbCtx.Set<UserProjectReward>()
                .Any(pj => pj.UserId == userId
                           && pj.ProjectId == projectId
                           && pj.IsOwner == true);
        }
    }
}