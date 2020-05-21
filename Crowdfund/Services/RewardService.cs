using Crowdfund.Data;
using Crowdfund.Models;
using Crowdfund.Services.Interfaces;
using Crowdfund.Services.Options.RewardPackageOptions;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Crowdfund.Services
{
    public class RewardService : IRewardService
    {
        private readonly DataContext _context;

        public RewardService(DataContext context)
        {
            _context = context;
        }

        public RewardPackage CreateRewardPackage(CreateRewardPackageOptions createRewardOptions)
        {
            var reward = new RewardPackage()
            {
                Title = createRewardOptions.Title,
                Description = createRewardOptions.Description,
                MinAmount = createRewardOptions.MinAmount!.Value,
                Quantity = createRewardOptions.Quantity
            };

            return reward;
        }
       
        public RewardPackage UpdateRewardPackage(RewardPackage packageToUpdate ,UpdateRewardPackageOptions updateRewardOptions)
        {
            if (packageToUpdate == null)
            {
                return null;
            }

            if (!string.IsNullOrWhiteSpace(updateRewardOptions.Title))
            {
                packageToUpdate.Title = updateRewardOptions.Title;
            }

            if (!string.IsNullOrWhiteSpace(updateRewardOptions.Description))
            {
                packageToUpdate.Description = updateRewardOptions.Description;
            }

            if (updateRewardOptions.Quantity != null)
            {
                packageToUpdate.Quantity = updateRewardOptions.Quantity;
            }

            if (updateRewardOptions.MinAmount != null)
            {
                packageToUpdate.MinAmount = updateRewardOptions.MinAmount.Value;
            }

            return packageToUpdate;
        }

        public bool DeleteRewardPackage(RewardPackage rewardPackage)
        {
            
            _context.Remove(rewardPackage);

            return _context.SaveChanges() > 0;
        }

        public RewardPackage GetRewardPackageById(int? packageId)
        {
            if (packageId == null)
            {
                return null;
            }

            return _context.Set<RewardPackage>().Find(packageId);
        }
    }
}