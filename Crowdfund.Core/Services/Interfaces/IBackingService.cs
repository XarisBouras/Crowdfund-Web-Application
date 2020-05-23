using Crowdfund.Core.Models;
using System.Collections.Generic;

namespace Crowdfund.Core.Services.Interfaces
{
    public interface IBackingService
    {
        bool CreateBacking(int? userId, int? projectId, int rewardPackageId, decimal amount);

        decimal? GetProjectBackingsAmount(int? userId, int? projectId);

        int? GetProjectBackings(int? userId, int? projectId);

        IEnumerable<Project> TrendingProjects();
    }
}