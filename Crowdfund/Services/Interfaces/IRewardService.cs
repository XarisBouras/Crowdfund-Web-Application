using Crowdfund.Data;
using Crowdfund.Models;
using Crowdfund.Services.Options.RewardPackageOptions;

namespace Crowdfund.Services.Interfaces
{
    public interface IRewardService
    {
        RewardPackage CreateRewardPackage(CreateRewardPackageOptions createRewardPackageOptions);

        RewardPackage UpdateRewardPackage(UpdateRewardPackageOptions updateRewardPackageOptions);

        bool DeleteRewardPackage(int? projectId, int? rewardPackageId);
        
        RewardPackage GetRewardPackageById(int? packageId);
    }
}