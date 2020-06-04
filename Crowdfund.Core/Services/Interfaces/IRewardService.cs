using Crowdfund.Core.Models;
using Crowdfund.Core.Services.Options.RewardPackageOptions;

namespace Crowdfund.Core.Services.Interfaces
{
    public interface IRewardService
    {
        Result<RewardPackage> CreateRewardPackage(CreateRewardPackageOptions createRewardPackageOptions);

        Result<RewardPackage> UpdateRewardPackage(RewardPackage rewardPackage ,UpdateRewardPackageOptions updateRewardPackageOptions);

        bool DeleteRewardPackage(RewardPackage rewardPackage);

        Result<RewardPackage> GetRewardPackageById(int? packageId);
    }
}