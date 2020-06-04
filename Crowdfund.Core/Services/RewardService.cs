using Crowdfund.Core.Data;
using Crowdfund.Core.Models;
using Crowdfund.Core.Services.Interfaces;
using Crowdfund.Core.Services.Options.RewardPackageOptions;

namespace Crowdfund.Core.Services
{
    public class RewardService : IRewardService
    {
        private readonly DataContext _context;

        public RewardService(DataContext context)
        {
            _context = context;
        }

        public Result<RewardPackage> CreateRewardPackage(CreateRewardPackageOptions options)
        {
            options.Title = options.Title?.Trim();
            options.Description = options.Description?.Trim();
            
            if (string.IsNullOrWhiteSpace(options.Title) || options.Quantity < 0 || options.MinAmount <= 0)
            {
                return Result<RewardPackage>.Failed(StatusCode.BadRequest, "Options Not Valid");
            }
            
            var reward = new RewardPackage
            {
                Title = options.Title,
                Description = options.Description,
                MinAmount = options.MinAmount,
                Quantity = options.Quantity
            };

            return Result<RewardPackage>.Succeed(reward);
        }
       
        public Result<RewardPackage> UpdateRewardPackage(RewardPackage packageToUpdate ,UpdateRewardPackageOptions options)
        {
            
            options.Title = options.Title?.Trim();
            options.Description = options.Description?.Trim();

            if (!string.IsNullOrWhiteSpace(options.Title))
            {
                packageToUpdate.Title = options.Title;
            }

            if (!string.IsNullOrWhiteSpace(options.Description))
            {
                packageToUpdate.Description = options.Description;
            }

            if (options.Quantity != null)
            {
                packageToUpdate.Quantity = options.Quantity;
            }

            if (options.MinAmount != null || options.MinAmount <= 0)
            {
                packageToUpdate.MinAmount = options.MinAmount.Value;
            }
            //if (options.MinAmount <= 0)
            //{
            //    return Result<bool>.Failed(StatusCode.BadRequest, "Not Valid Amount");
            //}

            return Result<RewardPackage>.Succeed(packageToUpdate);
        }

        public bool DeleteRewardPackage(RewardPackage rewardPackage)
        {
            _context.Remove(rewardPackage);

            return _context.SaveChanges() > 0;
        }

        public Result<RewardPackage>  GetRewardPackageById(int? packageId)
        {
            return packageId == null ? Result<RewardPackage>.Failed(StatusCode.NotFound,
                        "Sorry, we couldn't find this page. But don't worry," +
                        " you can find plenty of other things in our homepage") : 
                        Result<RewardPackage>.Succeed(_context.Set<RewardPackage>().Find(packageId));
        }
    }
}