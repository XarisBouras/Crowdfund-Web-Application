using Crowdfund.Data;
using Crowdfund.Models;
using Crowdfund.Services.Options.RewardPackageOptions;

namespace Crowdfund.Services.Interfaces
{
    public interface IRewardService
    {
        RewardPackage CreateRewardPackage(CreateRewardPackageOptions createRewardPackageOptions);
        
        RewardPackage GetRewardPackageById(int id);
    }
}