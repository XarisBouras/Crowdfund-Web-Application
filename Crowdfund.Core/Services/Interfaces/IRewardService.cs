using Crowdfund.Core.Models;
using Crowdfund.Core.Services.Options.RewardPackageOptions;

namespace Crowdfund.Core.Services.Interfaces
{
    public interface IRewardService
    {
        RewardPackage CreateRewardPackage(CreateRewardPackageOptions createRewardPackageOptions);

        RewardPackage UpdateRewardPackage(RewardPackage rewardPackage ,UpdateRewardPackageOptions updateRewardPackageOptions);

        bool DeleteRewardPackage(RewardPackage rewardPackage);
        
        RewardPackage GetRewardPackageById(int? packageId);
    }
}