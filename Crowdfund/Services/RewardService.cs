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
            var rewardList = _context.Set<Project>()
                                 .Include(p => p.RewardPackages)
                                 .Where(p => p.ProjectId == createRewardOptions.ProjectId)
                                 .SingleOrDefault();

            if (rewardList != null)
            {
                foreach (var item in rewardList.RewardPackages)
                {
                    if (item.MinAmount == createRewardOptions.MinAmount)
                    {
                        return null;
                    }
                }
            }

            var reward = new RewardPackage()
            {
                Title = createRewardOptions.Title,
                Description = createRewardOptions.Description,
                MinAmount = createRewardOptions.MinAmount.Value,
                Quantity = createRewardOptions.Quantity
            };

            return reward;
        }
       
        public RewardPackage UpdateRewardPackage(UpdateRewardPackageOptions updateRewardOptions)
        {

            var rewardList = _context.Set<Project>()
                                .Include(p => p.RewardPackages)
                                .Where(p => p.ProjectId == updateRewardOptions.ProjectId)
                                .SingleOrDefault();
           
            foreach (var item in rewardList.RewardPackages)
            {
                if (item.MinAmount == updateRewardOptions.MinAmount)
                {
                    return null;
                }
            }

            var packageToUpdate = GetRewardPackageById(updateRewardOptions.RewardPackageId);

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

        public bool DeleteRewardPackage(int? projectId, int? rewardPackageId)
        {
            var rewardList = _context.Set<Project>()
                                 .Include(p => p.RewardPackages)
                                 .Where(p => p.ProjectId == projectId)
                                 .SingleOrDefault();

            if (rewardList == null)
            {
                return false;
            }

            var packageToDelete = GetRewardPackageById(rewardPackageId);

            if (packageToDelete == null)
            {
                return false;
            }

            _context.Remove(packageToDelete);

            return _context.SaveChanges() > 0 ? true : false;
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