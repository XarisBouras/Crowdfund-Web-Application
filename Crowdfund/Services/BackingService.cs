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

        public BackingService(DataContext context, IUserService userService, IProjectService projectService)
        {
            _context = context;
            _userService = userService;
            _projectService = projectService;
        }

        public bool CreateBacking(int userId, int projectId, int rewardPackageId, decimal amount)
        {
            var user = _userService.GetUserById(userId);
            var project = _projectService.GetProjectById(projectId);

            if (user == null || project == null) return false;

            var projectOwner = _context.Set<UserProjectReward>()
                .Any(u => u.UserId == userId && u.ProjectId == projectId && u.IsOwner == true);
            
            if (projectOwner) return false;

            var rewardPackage = project.RewardPackages
                .FirstOrDefault(rp => amount >= rp.MinAmount && rp.RewardPackageId == rewardPackageId);

            if (rewardPackage == null && rewardPackageId != 0) return false;

            var userProjectBacking = new UserProjectReward
            {
                IsOwner = false,
                Project = project,
                RewardPackage = rewardPackageId == 0 ? null : rewardPackage,
                Amount = amount
            };
            
             user.UserProjectReward.Add(userProjectBacking);

             return _context.SaveChanges() > 0;
        }
    }
}