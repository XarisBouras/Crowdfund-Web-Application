using Crowdfund.Data;
using Crowdfund.Models;
using Crowdfund.Services.Interfaces;
using Crowdfund.Services.Options.RewardPackageOptions;

namespace Crowdfund.Services
{
    public class RewardService : IRewardService
    {
        private readonly DataContext _context;

        public RewardService(DataContext context)
        {
            _context = context;
        }
        
        public RewardPackage CreateRewardPackage(CreateRewardPackageOptions createRewardPackageOptions)
        {
            var rewardPackage = new RewardPackage
            {
                Description = createRewardPackageOptions.Description,
                MinAmount = createRewardPackageOptions.MinAmount.Value,
                Quantity = createRewardPackageOptions.Quantity
            };

            _context.Set<RewardPackage>().Add(rewardPackage);
            
            return _context.SaveChanges() > 0 ? rewardPackage : null;
        }

        public RewardPackage GetRewardPackageById(int id)
        {
            return _context.Set<RewardPackage>().Find(id);
        }
    }
}