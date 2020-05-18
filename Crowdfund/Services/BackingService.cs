using System.Linq;
using Crowdfund.Data;
using Crowdfund.Models;
using Crowdfund.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Crowdfund.Services
{
    public class BackingService : IBackingService
    {
        private readonly DataContext _context;
        private readonly IUserService _userService;
        private readonly IProjectService _projectService;
        private readonly IRewardService _rewardService;

        public BackingService(DataContext context, IUserService userService, IProjectService projectService,
            IRewardService rewardService)
        {
            _context = context;
            _userService = userService;
            _projectService = projectService;
            _rewardService = rewardService;
        }

        public bool CreateBacking(int userId, int projectId, int rewardPackageId, decimal amount)
        {
            var user = _userService.GetUserById(userId);
            var project = _projectService.GetProjectById(projectId);

            if (user == null || project == null) return false;
            
            var projectOwner = user.UserProjectReward.FirstOrDefault(u => u.IsOwner);
            
            if (projectOwner != null) return false;

            var userProjectBacking = new UserProjectReward
            {
                IsOwner = false,
                Project = project,
                RewardPackage = rewardPackageId == 0 ? null : project.RewardPackages
                                                    .FirstOrDefault(rp => rp.MinAmount >= amount && rp.RewardPackageId == rewardPackageId),
                Amount = amount
            };
            
             user.UserProjectReward.Add(userProjectBacking);

             return _context.SaveChanges() > 0;
        }
    }
}