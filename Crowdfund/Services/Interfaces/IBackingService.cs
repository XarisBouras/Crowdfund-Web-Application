using Crowdfund.Data;
using Crowdfund.Models;

namespace Crowdfund.Services.Interfaces
{
    public interface IBackingService
    {
        bool CreateBacking(int? userId, int? projectId, int rewardPackageId, decimal amount);

        decimal? GetProjectBackingsAmount(int? userId, int? projectId);

        int? GetProjectBackings(int? userId, int? projectId);
    }
}