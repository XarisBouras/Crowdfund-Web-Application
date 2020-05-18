using System;
using System.Linq;
using Crowdfund.Data;
using Crowdfund.Models;
using Crowdfund.Services.Interfaces;
using Crowdfund.Services.Options.ProjectOptions;
using Crowdfund.Services.Options.RewardPackageOptions;
using Microsoft.EntityFrameworkCore;

namespace Crowdfund.Services
{
    public class ProjectService : IProjectService
    {
        private readonly DataContext _context;
        private readonly IUserService _userService;
        private readonly IRewardService _rewardService;

        public ProjectService(DataContext context, IUserService userService, IRewardService rewardService)
        {
            _context = context;
            _userService = userService;
            _rewardService = rewardService;
        }
        public Project CreateProject(CreateProjectOptions createProjectOptions)
        {
            if (!Enum.IsDefined(typeof(Category), createProjectOptions.CategoryId) || createProjectOptions.Goal == 0 ||
                createProjectOptions.Title == null || createProjectOptions.DueTo == null)
            {
                return null;
            }
            
            var project = new Project
            {
                Title = createProjectOptions.Title,
                Category =  (Category)createProjectOptions.CategoryId,
                Description = createProjectOptions.Description,
                DueTo = createProjectOptions.DueTo,
                Goal = createProjectOptions.Goal
            };
            
            _context.Set<Project>().Add(project);

            var user = _userService.GetUserById(createProjectOptions.UserId);
            if (user == null) return null;
            
            var userProject = new UserProjectReward
            {
                IsOwner = true,
                Project = project
            };

            user.UserProjectReward.Add(userProject);
            
            return _context.SaveChanges() > 0 ? project : null;
        }

        public Project GetProjectById(int id)
        {
            return _context.Set<Project>().Include(p => p.RewardPackages).FirstOrDefault(p => p.ProjectId == id);
        }

        public bool AddRewardPackage(Project project, CreateRewardPackageOptions rewardPackageOptions)
        {
            var rewardPackage = _rewardService.CreateRewardPackage(rewardPackageOptions);
            
            project.RewardPackages.Add(rewardPackage);

            return _context.SaveChanges() > 0;
        }
    }
}