using Crowdfund.Core.Models;
using System.Collections.Generic;

namespace Crowdfund.Core.Services.Interfaces
{
    public interface IBackingService
    {
        Result<bool> CreateBacking(int? userId, int? projectId, int rewardPackageId, decimal amount);

        Result<decimal> GetUserProjectBackingsAmount(int? userId, int? projectId);
        
        Result<decimal> GetProjectBackingsAmount(int? projectId);

        Result<int> GetUserProjectBackings(int? userId, int? projectId);

        Result<int> GetProjectBackings(int? projectId);

        IEnumerable<Project> TrendingProjects();
    }
}