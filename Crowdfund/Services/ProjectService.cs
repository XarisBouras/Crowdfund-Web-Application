using System;
using System.Collections.Generic;
using System.Linq;
using Crowdfund.Data;
using Crowdfund.Models;
using Crowdfund.Services.CreateOptions;
using Crowdfund.Services.Interfaces;
using Crowdfund.Services.Options.ProjectOptions;
using Crowdfund.Services.Options.RewardPackageOptions;
using Crowdfund.Services.UpdateOptions;
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

        public Project CreateProject(int? userId, CreateProjectOptions createProjectOptions)
        {
            if (createProjectOptions == null || userId == null
                || !Enum.IsDefined(typeof(Category), createProjectOptions.CategoryId)
                || createProjectOptions.Goal <= 0
                || string.IsNullOrWhiteSpace(createProjectOptions.Title)
                || createProjectOptions.DueTo == null)
            {
                return null;
            }

            if (_userService.GetUserById(userId) == null) return null;

                var project = new Project
            {
                Title = createProjectOptions.Title,
                Category = (Category) createProjectOptions.CategoryId,
                Description = createProjectOptions.Description,
                DueTo = createProjectOptions.DueTo,
                Goal = createProjectOptions.Goal
            };

            _context.Set<Project>().Add(project);

            var user = _userService.GetUserById(userId);
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
            return id != null
                ? _context.Set<Project>()
                    .Include(p => p.RewardPackages)
                    .Include(p => p.Medias)
                    .Include(p => p.Posts)
                    .FirstOrDefault(p => p.ProjectId == id)
                : null;
        }
        
        public Project GetSingleProject(int? id)
        {
            return id != null
                ? _context.Set<Project>()
                    .FirstOrDefault(p => p.ProjectId == id)
                : null;
        }

        public IQueryable<Project> GetAllProjects()
        {
            return _context.Set<Project>();
        }

        public Project UpdateProject(UpdateProjectOptions updateProjectOptions)
        {
            if (updateProjectOptions?.ProjectId == null || updateProjectOptions.UserId == null)
            {
                return null;
            }

            var project = GetProjectById(updateProjectOptions.ProjectId);

            if (project == null)
            {
                return null;
            }

            if (Helpers.UserOwnsProject(_context, updateProjectOptions.UserId, updateProjectOptions.ProjectId) == false)
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
                project.DueTo = updateProjectOptions.DueTo;
            }

            if (updateProjectOptions.Goal != null)
            {
                project.Goal = updateProjectOptions.Goal.Value;
            }

            return _context.SaveChanges() > 0 ? project : null;
        }

        public IQueryable<Project> SearchProjects(SearchProjectOptions searchProjectOptions)
        {
            if (searchProjectOptions == null)
            {
                return null;
            }

            var query = _context
                .Set<Project>()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchProjectOptions.SearchString))
            {
                query = query.Where(pj => pj.Title
                                              .Contains(searchProjectOptions.SearchString) ||
                                          pj.Description.Contains(searchProjectOptions.SearchString));
            }

            if (searchProjectOptions.CategoryIds != null)
            {
                query = query.Where(pj => searchProjectOptions.CategoryIds.Contains((int) pj.Category));
            }

            return query.Take(500);
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

            if (Helpers.UserOwnsProject(_context, userId, projectId) == false)
            {
                return false;
            }

            _context.Remove(project);

            return _context.SaveChanges() > 0;
        }

        public RewardPackage AddRewardPackage(int? projectId, int? userId,
            CreateRewardPackageOptions createRewardOptions)
        {
            if (projectId == null || userId == null || createRewardOptions.Quantity < 0 ||
                createRewardOptions.MinAmount <= 0 || createRewardOptions.MinAmount == null)
            {
                return null;
            }

            var project = GetProjectById(projectId);

            if (project == null)
            {
                return null;
            }

            if (Helpers.UserOwnsProject(_context, userId, projectId) == false)
            {
                return null;
            }

            if (project.RewardPackages.Any(r => r.MinAmount == createRewardOptions.MinAmount))
                return null;

            var reward = _rewardService.CreateRewardPackage(createRewardOptions);

            if (reward != null)
            {
                project.RewardPackages.Add(reward);
            }

            return _context.SaveChanges() > 0 ? reward : null;
        }

        public RewardPackage UpdateRewardPackage(int? projectId, int? userId, int? rewardPackageId,
            UpdateRewardPackageOptions updateRewardOptions)
        {
            if (updateRewardOptions == null
                || projectId == null
                || userId == null
                || rewardPackageId == null
                || updateRewardOptions.Quantity < 0
                || updateRewardOptions.MinAmount <= 0)
            {
                return null;
            }

            var project = GetProjectById(projectId);

            if (project == null)
            {
                return null;
            }

            if (Helpers.UserOwnsProject(_context, userId, projectId) == false)
            {
                return null;
            }

            if (project.RewardPackages.Any(r => r.MinAmount == updateRewardOptions.MinAmount))
                return null;

            var rewardPackageToUpdate = project.RewardPackages
                .FirstOrDefault(rp => rp.RewardPackageId == rewardPackageId);

            var reward = _rewardService.UpdateRewardPackage(rewardPackageToUpdate, updateRewardOptions);

            if (reward == null)
            {
                return null;
            }

            return _context.SaveChanges() > 0 ? reward : null;
        }

        public bool DeleteRewardPackage(int? userId, int? projectId, int? rewardPackageId)
        {
            if (userId == null || projectId == null || rewardPackageId == null)
            {
                return false;
            }

            if (Helpers.UserOwnsProject(_context, userId, projectId) == false)
            {
                return false;
            }

            var project = GetProjectById(projectId);

            var rewardToDelete = project.RewardPackages
                .FirstOrDefault(rp => rp.RewardPackageId == rewardPackageId);

            return rewardToDelete != null ? _rewardService.DeleteRewardPackage(rewardToDelete) : false;
        }

        public IList<Media> GetProjectPhotos(int? projectId)
        {
            if (projectId == null)
            {
                return null;
            }

            var project = GetProjectById(projectId);
            
            return project?.Medias.Where(p => p.MediaType == MediaType.Photo).ToList();
        }
        
        public IList<Media> GetProjectVideos(int? projectId)
        {
            if (projectId == null)
            {
                return null;
            }

            var project = GetProjectById(projectId);
            
            return project?.Medias.Where(p => p.MediaType == MediaType.Video).ToList();
        }

        public Media AddMedia(CreateMediaOptions createMediaOptions, int? userId, int? projectId)
        {
            if (createMediaOptions == null
                || projectId == null
                || userId == null)
            {
                return null;
            }

            if (Helpers.UserOwnsProject(_context, userId, projectId) == false)
            {
                return null;
            }

            var project = GetProjectById(projectId);

            if (project == null)
            {
                return null;
            }


            var media = _mediaService.CreateMedia(createMediaOptions);

            if (media != null)
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

            if (Helpers.UserOwnsProject(_context, userId, projectId) == false)
            {
                return false;
            }

            var mediaToDelete = project.Medias
                .FirstOrDefault(m => m.MediaId == mediaId);

            return mediaToDelete != null ? _mediaService.DeleteMedia(mediaToDelete) : false;
        }

        public Post AddPost(CreatePostOptions createPostOptions, int? userId, int? projectId)
        {
            if (createPostOptions == null
                || projectId == null
                || userId == null)
            {
                return null;
            }

            if (Helpers.UserOwnsProject(_context, userId, projectId) == false)
            {
                return null;
            }

            var project = GetProjectById(projectId);

            if (project == null)
            {
                return null;
            }

            var post = _postService.CreatePost(createPostOptions);

            if (post != null)
            {
                project.Posts.Add(post);
            }

            return _context.SaveChanges() > 0 ? post : null;
        }


        public Post UpdatePost(int? postId, int? userId, int? projectId, UpdatePostOptions updatePostOptions)
        {
            if (updatePostOptions == null || postId == null
                                          || projectId == null
                                          || userId == null)
            {
                return null;
            }

            if (Helpers.UserOwnsProject(_context, userId, projectId) == false)
            {
                return null;
            }

            var project = GetProjectById(projectId);

            if (project == null)
            {
                return null;
            }

            var post = project.Posts.SingleOrDefault(p => p.PostId == postId);

            if (post != null)
            {
                _postService.UpdatePost(post, updatePostOptions);
            }

            return _context.SaveChanges() > 0 ? post : null;
        }

        public bool DeletePost(int? postId, int? userId, int? projectId)
        {
            if (postId == null
                || projectId == null
                || userId == null)
            {
                return false;
            }

            if (Helpers.UserOwnsProject(_context, userId, projectId) == false)
            {
                return false;
            }

            var project = GetProjectById(projectId);

            var postToDelete = project.Posts.SingleOrDefault(p => p.PostId == postId);

            return postToDelete != null ? _postService.DeletePost(postToDelete) : false;
        }

        public IList<Post> GetProjectPosts(int? projectId)
        {
            if (projectId == null)
            {
                return null;
            }

            var project = GetProjectById(projectId);

            return project?.Posts.OrderByDescending(p => p.CreatedAt).ToList();
        }
    }
}