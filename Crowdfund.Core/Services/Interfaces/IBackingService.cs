using Crowdfund.Core.Models;
using System.Collections.Generic;

namespace Crowdfund.Core.Services.Interfaces
{
    public interface IBackingService
    {
        Result<bool> CreateBacking(int? userId, int? projectId, int rewardPackageId, decimal amount);
        
        
        Result<decimal> GetProjectBackingsAmount(int? projectId);

        Result<IEnumerable<Project>> GetUserProjects(int? userId);

        Result<int> GetProjectBackings(int? projectId);

        IEnumerable<Project> TrendingProjects();
    }
}