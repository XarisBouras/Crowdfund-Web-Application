using System.Collections.Generic;
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

        public bool CreateBacking(int? userId, int? projectId, int rewardPackageId, decimal amount)
        {
            if (userId == null || projectId == null) return false;
            
            var user = _userService.GetUserById(userId);
            var project = _projectService.GetProjectById(projectId);

            if (user == null || project == null) return false;

            /*var projectOwner = _context.Set<UserProjectReward>()
                .Any(u => u.UserId == userId && u.ProjectId == projectId && u.IsOwner == true);*/
            
            if (Helpers.UserOwnsProject(_context, userId, projectId)) return false;

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

        public decimal? GetProjectBackingsAmount(int? userId, int? projectId)
        {
            if (userId == null || projectId == null) return null;

            if (Helpers.UserOwnsProject(_context, userId, projectId) == false) return null;

            var backingsAmount = _context.Set<UserProjectReward>()
                .Where(u => u.UserId == userId && u.ProjectId == projectId && u.IsOwner == false)
                .Sum(a => a.Amount);

            return backingsAmount;
        }
        
        public int? GetProjectBackings(int? userId, int? projectId)
        {
            if (userId == null || projectId == null) return null;

            if (Helpers.UserOwnsProject(_context, userId, projectId) == false) return null;

            var backings = _context.Set<UserProjectReward>()
                .Count(u => u.UserId == userId && u.ProjectId == projectId && u.IsOwner == false);

            return backings;
        }

        public IEnumerable<Project> TrendingProjects()
        {
            var projectBakings = _context.Set<UserProjectReward>()
                .Where(u => u.IsOwner == false)
                .GroupBy(p => new {p.ProjectId})
                .Select(p => new {p.Key.ProjectId, Count = p.Count()})
                .OrderByDescending(o => o.Count).Select(p => p.ProjectId)
                .Take(10).ToList();
            
            return projectBakings.Select(id => _projectService.GetSingleProject(id));
        }
        
    }
}