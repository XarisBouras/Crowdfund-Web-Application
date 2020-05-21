using Crowdfund.Data;
using Crowdfund.Models;
using Crowdfund.Services.Options.RewardPackageOptions;

namespace Crowdfund.Services.Interfaces
{
    public interface IRewardService
    {
        RewardPackage CreateRewardPackage(CreateRewardPackageOptions createRewardPackageOptions);

        RewardPackage UpdateRewardPackage(RewardPackage rewardPackage ,UpdateRewardPackageOptions updateRewardPackageOptions);

        bool DeleteRewardPackage(RewardPackage rewardPackage);
        
        RewardPackage GetRewardPackageById(int? packageId);
    }
}