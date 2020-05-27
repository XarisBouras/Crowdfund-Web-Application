using System;
using Crowdfund.Core.Data;
using Crowdfund.Core.Models;
using Crowdfund.Core.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Crowdfund.Core.Services
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

        public Result<bool> CreateBacking(int? userId, int? projectId, int rewardPackageId, decimal amount)
        {
            if (userId == null || projectId == null) return Result<bool>.Failed(StatusCode.BadRequest, "Null options");

            var user = _userService.GetUserById(userId);
            var project = _projectService.GetProjectById(projectId);

            if (user == null || project == null) return Result<bool>.Failed(StatusCode.NotFound, "User or Project Not Found");

            /*var projectOwner = _context.Set<UserProjectReward>()
                .Any(u => u.UserId == userId && u.ProjectId == projectId && u.IsOwner == true);*/

            if (Helpers.UserOwnsProject(_context, userId, projectId)) return Result<bool>.Failed(StatusCode.BadRequest, "You cannot back your project");

            var rewardPackage = project.Data.RewardPackages
                .FirstOrDefault(rp => amount >= rp.MinAmount && rp.RewardPackageId == rewardPackageId);

            if (rewardPackage == null && rewardPackageId != 0) return Result<bool>.Failed(StatusCode.NotFound, "Reward Package Not Found");

            var userProjectBacking = new UserProjectReward
            {
                IsOwner = false,
                Project = project.Data,
                RewardPackage = rewardPackageId == 0 ? null : rewardPackage,
                Amount = amount
            };

            user.UserProjectReward.Add(userProjectBacking);
            
            var rows = 0;

            try {
                rows = _context.SaveChanges();
            } catch(Exception ex) {
                return Result<bool>.Failed( StatusCode.InternalServerError, ex.Message);
            }
            return rows <= 0 ? Result<bool>.Failed(StatusCode.InternalServerError, "Backing could not be created") : Result<bool>.Succeed(true);
        }

        public Result<decimal> GetUserProjectBackingsAmount(int? userId, int? projectId)
        {
            if (userId == null || projectId == null) return Result<decimal>.Failed(StatusCode.BadRequest, "Null options");

            if (Helpers.UserOwnsProject(_context, userId, projectId) == false) return Result<decimal>.Failed(StatusCode.BadRequest, "You do not own this project");

            var backingsAmount = _context.Set<UserProjectReward>()
                .Where(u => u.UserId == userId && u.ProjectId == projectId && u.IsOwner == false)
                .Sum(a => a.Amount);

            return Result<decimal>.Succeed(backingsAmount ?? 0);
        }

        public Result<decimal> GetProjectBackingsAmount(int? projectId)
        {
            if (projectId == null) return Result<decimal>.Failed(StatusCode.BadRequest, "Null option");;
            
            var backingsAmount = _context.Set<UserProjectReward>()
                .Where(u =>  u.ProjectId == projectId && u.IsOwner == false)
                .Sum(a => a.Amount);
            
            return Result<decimal>.Succeed(backingsAmount ?? 0);
        }

        public Result<int> GetUserProjectBackings(int? userId, int? projectId)
        {
            if (userId == null || projectId == null) return Result<int>.Failed(StatusCode.BadRequest, "Null options");

            if (Helpers.UserOwnsProject(_context, userId, projectId) == false) return Result<int>.Failed(StatusCode.BadRequest, "You do not own this project");

            var backings = _context.Set<UserProjectReward>()
                .Count(u => u.UserId == userId && u.ProjectId == projectId && u.IsOwner == false);

            return Result<int>.Succeed(backings);
        }
        
        public Result<int> GetProjectBackings(int? projectId)
        {
            if (projectId == null) return Result<int>.Failed(StatusCode.BadRequest, "Null option");

            var backings = _context.Set<UserProjectReward>()
                .Count(u => u.ProjectId == projectId && u.IsOwner == false);

            return Result<int>.Succeed(backings);
        }

        public IEnumerable<Project> TrendingProjects()
        {
            var projectBakings = _context.Set<UserProjectReward>()
                .Where(u => u.IsOwner == false)
                .GroupBy(p => new {p.ProjectId})
                .Select(p => new {p.Key.ProjectId, Sum = p.Sum(s => s.Amount)})
                .OrderByDescending(o => o.Sum).Select(p => p.ProjectId)
                .Take(10).ToList();

            return projectBakings.Select(id => _projectService.GetSingleProject(id));
        }
    }
}