using System;
using System.Linq;
using Crowdfund.Data;
using Crowdfund.Models;
using Crowdfund.Services.Interfaces;
using Crowdfund.Services.Options.MediaOptions;
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
        private readonly IMediaService _mediaService;
        private readonly IPostService _postService;

        public ProjectService(DataContext context, IUserService userService,
             IRewardService rewardService, IMediaService mediaService, IPostService postService)
        {
            _context = context;
            _userService = userService;
            _rewardService = rewardService;
            _mediaService = mediaService;
            _postService = postService;
        }
        public Project CreateProject(CreateProjectOptions createProjectOptions)
        {
            if (!Enum.IsDefined(typeof(Category), createProjectOptions.CategoryId) || createProjectOptions.Goal <= 0 ||
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
            if (user == null)
            {
                return null;
            }
            
            var userProject = new UserProjectReward
            {
                IsOwner = true,
                Project = project
            };

            user.UserProjectReward.Add(userProject);
            
            return _context.SaveChanges() > 0 ? project : null;
        }

        public Project GetProjectById(int? id)
        {
            if(id == null)
            {
                return null;
            }

            return _context.Set<Project>()
                .Include(p => p.RewardPackages)
                .FirstOrDefault(p => p.ProjectId == id);
        }

        public Project UpdateProject(UpdateProjectOptions updateProjectOptions)
        {
            if (updateProjectOptions == null 
                || updateProjectOptions.ProjectId == null 
                || updateProjectOptions.UserId == null)
            {
                return null;
            }

            var project = GetProjectById(updateProjectOptions.ProjectId);

            if (project == null)
            {
                return null;
            }

            var user = _context.Set<UserProjectReward>()
                                .Where(pj => pj.UserId == updateProjectOptions.UserId
                                     && pj.ProjectId == updateProjectOptions.ProjectId
                                     && pj.IsOwner == true)
                                .SingleOrDefault();
            if (user == null)
            {
                return null;
            }           

            if (!string.IsNullOrWhiteSpace(updateProjectOptions.Description))
            {
                project.Description = updateProjectOptions.Description;
            }

            if (!string.IsNullOrWhiteSpace(updateProjectOptions.Title))
            {
                project.Title = updateProjectOptions.Title;
            }

            if (updateProjectOptions.DueTo != null)
            {
                project.DueTo = updateProjectOptions.DueTo.Value;
            }

            if (updateProjectOptions.Goal != null)
            {
                project.Goal = updateProjectOptions.Goal.Value;
            }

            if (_context.SaveChanges() > 0)
            {
                return project;
            }

            return _context.SaveChanges()>0 ? project : null;
        }

        public IQueryable<Project> SearchProject(SearchProjectOptions searchProjectOptions)
        {
            if (searchProjectOptions == null)
            {
                return null;
            }

            var query = _context
                            .Set<Project>()
                            .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchProjectOptions.Title))
            {
                query = query.Where(pj => pj.Title == searchProjectOptions.Title);
            }

            if (!string.IsNullOrWhiteSpace(searchProjectOptions.Description))
            {
                query = query.Where(pj => pj.Description == searchProjectOptions.Description);
            }

            if (searchProjectOptions.Category != null)
            {
                foreach (var item in searchProjectOptions.Category)
                {
                   // query = query.Where(pj => pj.Category == item);
                }               
            }

            return query;
        }

        public bool DeleteProject(int? userId, int? projectId)
        {
            if (userId == null || projectId == null)
            {
                return false;
            }

            var project = GetProjectById(projectId);

            if (project == null)
            {
                return false;
            }

            var user = _context.Set<UserProjectReward>()
                               .Where(pj => pj.UserId == userId
                                    && pj.ProjectId == projectId
                                    && pj.IsOwner == true)
                               .SingleOrDefault();
            if (user == null)
            {
                return false;
            }
          
            _context.Remove(project);
            
            return _context.SaveChanges()>0 ? true : false;
        }

        public RewardPackage AddRewardPackage(CreateRewardPackageOptions createRewardOptions)
        {
            if (createRewardOptions == null 
                || createRewardOptions.ProjectId == null 
                || createRewardOptions.UserId == null 
                || createRewardOptions.Quantity < 0
                || createRewardOptions.MinAmount <= 0 || createRewardOptions.MinAmount == null)
            {
                return null;
            }

            var project = GetProjectById(createRewardOptions.ProjectId);

            if(project == null)
            {
                return null;
            }

            var user = _context.Set<UserProjectReward>()
                               .Where(pj => pj.UserId == createRewardOptions.UserId
                                    && pj.ProjectId == createRewardOptions.ProjectId
                                    && pj.IsOwner == true)
                               .SingleOrDefault();
            if (user == null)
            {
                return null;
            }           
          
            var reward = _rewardService.CreateRewardPackage(createRewardOptions);

            if (reward != null)
            {
                project.RewardPackages.Add(reward);
            }
            
            return _context.SaveChanges() > 0 ? reward : null;
        }

        public RewardPackage UpdateRewardPackage(UpdateRewardPackageOptions updateRewardOptions)
        {
            if (updateRewardOptions == null
                || updateRewardOptions.ProjectId == null
                || updateRewardOptions.UserId == null
                || updateRewardOptions.RewardPackageId == null
                || updateRewardOptions.Quantity < 0
                || updateRewardOptions.MinAmount <= 0 )
            {
                return null;
            }

            var project = GetProjectById(updateRewardOptions.ProjectId);

            if(project == null)
            {
                return null;
            }

            var user = _context.Set<UserProjectReward>()
                               .Where(pj => pj.UserId == updateRewardOptions.UserId
                                    && pj.ProjectId == updateRewardOptions.ProjectId
                                    && pj.IsOwner == true)
                               .SingleOrDefault();
            if (user == null)
            {
                return null;
            }
                       
           var reward = _rewardService.UpdateRewardPackage(updateRewardOptions);

            if (reward == null)
            {
                return null;
            }
                
            return _context.SaveChanges() > 0 ? reward : null;
        }

        public bool DeleteRewardPackage(int? userId, int? projectId, int? rewardPackageId)
        {
            if(userId == null || projectId == null || rewardPackageId == null)
            {
                return false;
            }

            var project = GetProjectById(projectId);

            if (project == null)
            {
                return false;
            }

            var user = _context.Set<UserProjectReward>()
                               .Where(pj => pj.UserId == userId
                                    && pj.ProjectId == projectId
                                    && pj.IsOwner == true)
                               .SingleOrDefault();
            if (user == null)
            {
                return false;
            }

            var result = _rewardService.DeleteRewardPackage(projectId, rewardPackageId);

            return result;
        }

        public Media AddMedia(CreateMediaOptions createMediaOptions, int? userId, int? projectId)
        {
            if (createMediaOptions == null
                || projectId == null
                || userId == null)
            {
                return null;
            }

            var project = GetProjectById(projectId);

            if (project == null)
            {
                return null;
            }

            var user = _context.Set<UserProjectReward>()
                               .Where(pj => pj.UserId == userId
                                    && pj.ProjectId == projectId
                                    && pj.IsOwner == true)
                               .SingleOrDefault();
            if (user == null)
            {
                return null;
            }

            var media = _mediaService.CreateMedia(createMediaOptions);

            if(media != null)
            {
                project.Medias.Add(media);
            }

            return _context.SaveChanges() > 0 ? media : null;
        }

        public bool DeleteMedia(int? userId, int? projectId, int? mediaId)
        {
            if (userId == null || projectId == null || mediaId == null)
            {
                return false;
            }

            var project = GetProjectById(projectId);

            if (project == null)
            {
                return false;
            }
            
            var user = _context.Set<UserProjectReward>()
                               .Where(pj => pj.UserId == userId
                                    && pj.ProjectId == projectId
                                    && pj.IsOwner == true)
                               .SingleOrDefault();
            if (user == null)
            {
                return false;
            }

            
            var result = _mediaService.DeleteMedia(projectId, mediaId);

            return result;
        }
    }   
}