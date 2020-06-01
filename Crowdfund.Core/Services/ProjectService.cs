using Crowdfund.Core.Data;
using Crowdfund.Core.Models;
using Crowdfund.Core.Services.Interfaces;
using Crowdfund.Core.Services.Options.MediaOptions;
using Crowdfund.Core.Services.Options.PostOptions;
using Crowdfund.Core.Services.Options.ProjectOptions;
using Crowdfund.Core.Services.Options.RewardPackageOptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Crowdfund.Core.Services
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

        public Result<bool> CreateProject(int? userId, CreateProjectOptions createProjectOptions)
        {
            if (createProjectOptions == null || userId == null
                                             || string.IsNullOrWhiteSpace(createProjectOptions.MainImageUrl)
                                             || !Enum.IsDefined(typeof(Category), createProjectOptions.CategoryId)
                                             || createProjectOptions.Goal <= 0
                                             || string.IsNullOrWhiteSpace(createProjectOptions.Title)
                                             || createProjectOptions.DueTo == null)
            {
                return Result<bool>.Failed(StatusCode.BadRequest, "Project Options Not Valid");
            }

            if (_userService.GetUserById(userId) == null)
            {
                return Result<bool>.Failed(StatusCode.NotFound, "Sorry, we couldn't find this page. But don't worry," +
                                                                " you can find plenty of other things in our homepage");
            }

            var project = new Project
            {
                Title = createProjectOptions.Title,
                Category = (Category) createProjectOptions.CategoryId,
                Description = createProjectOptions.Description,
                DueTo = createProjectOptions.DueTo.Value,
                MainImageUrl = createProjectOptions.MainImageUrl,
                Goal = createProjectOptions.Goal
            };

            _context.Set<Project>().Add(project);

            var user = _userService.GetUserById(userId);
            if (user == null)
            {
                return Result<bool>.Failed(StatusCode.NotFound, "Sorry, we couldn't find this page. But don't worry," +
                                                                " you can find plenty of other things in our homepage");
            }

            var userProject = new UserProjectReward
            {
                IsOwner = true,
                Project = project
            };

            user.UserProjectReward.Add(userProject);

            var rows = 0;

            try
            {
                rows = _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return Result<bool>.Failed(StatusCode.InternalServerError, ex.Message);
            }

            return rows <= 0
                ? Result<bool>.Failed(StatusCode.InternalServerError, "Project Could Not Be Created")
                : Result<bool>.Succeed(true);
        }

        public Result<Project> GetProjectById(int? id)
        {
            try
            {
                var project = _context.Set<Project>()
                    .Include(p => p.RewardPackages)
                    .Include(p => p.Medias)
                    .Include(p => p.Posts)
                    .FirstOrDefault(p => p.ProjectId == id);

                return project == null
                    ? Result<Project>.Failed(StatusCode.NotFound,
                        "Sorry, we couldn't find this page. But don't worry," +
                        " you can find plenty of other things in our homepage")
                    : Result<Project>.Succeed(project);
            }
            catch (Exception ex)
            {
                return Result<Project>.Failed(StatusCode.InternalServerError, ex.Message);
            }
        }

        public Result<Project> GetSingleProject(int? id)
        {
            try
            {
                var project = _context.Set<Project>()
                    .FirstOrDefault(p => p.ProjectId == id);

                return project == null
                    ? Result<Project>.Failed(StatusCode.NotFound,
                        "Sorry, we couldn't find this page. But don't worry," +
                        " you can find plenty of other things in our homepage")
                    : Result<Project>.Succeed(project);
            }
            catch (Exception ex)
            {
                return Result<Project>.Failed(StatusCode.InternalServerError, ex.Message);
            }
        }

        public IQueryable<Project> GetAllProjects()
        {
            return _context.Set<Project>();
        }

        public Result<bool> UpdateProject(UpdateProjectOptions updateProjectOptions)
        {
            if (updateProjectOptions?.ProjectId == null || updateProjectOptions.UserId == null)
            {
                return Result<bool>.Failed(StatusCode.BadRequest, "Project Options Not Valid");
            }

            var project = GetProjectById(updateProjectOptions.ProjectId);

            if (project == null)
            {
                return Result<bool>.Failed(StatusCode.NotFound, "Sorry, we couldn't find this page. But don't worry," +
                                                                " you can find plenty of other things in our homepage");
            }

            if (Helpers.UserOwnsProject(_context, updateProjectOptions.UserId, updateProjectOptions.ProjectId) == false)
            {
                return Result<bool>.Failed(StatusCode.BadRequest, "Can Not Access A Project You Don't Own");
            }

            if (!string.IsNullOrWhiteSpace(updateProjectOptions.Description))
            {
                project.Data.Description = updateProjectOptions.Description;
            }

            if (!string.IsNullOrWhiteSpace(updateProjectOptions.Title))
            {
                project.Data.Title = updateProjectOptions.Title;
            }

            if (updateProjectOptions.DueTo != null)
            {
                project.Data.DueTo = updateProjectOptions.DueTo.Value;
            }

            if (updateProjectOptions.Goal > 0)
            {
                project.Data.Goal = updateProjectOptions.Goal;
            }

            var rows = 0;

            try
            {
                rows = _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return Result<bool>.Failed(StatusCode.InternalServerError, ex.Message);
            }

            return rows <= 0
                ? Result<bool>.Failed(StatusCode.InternalServerError, "Project Could Not Be Updated")
                : Result<bool>.Succeed(true);
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

        public Result<bool> DeleteProject(int? userId, int? projectId)
        {
            if (userId == null || projectId == null)
            {
                return Result<bool>.Failed(StatusCode.BadRequest, "User Or Project Not Specified");
            }

            var project = GetProjectById(projectId);

            if (project == null)
            {
                return Result<bool>.Failed(StatusCode.NotFound, "Sorry, we couldn't find this page. But don't worry," +
                                                                " you can find plenty of other things in our homepage");
            }

            if (Helpers.UserOwnsProject(_context, userId, projectId) == false)
            {
                return Result<bool>.Failed(StatusCode.BadRequest, "Can Not Access A Project You Don't Own");
            }

            _context.Remove(project);

            var rows = 0;

            try
            {
                rows = _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return Result<bool>.Failed(StatusCode.InternalServerError, ex.Message);
            }

            return rows <= 0
                ? Result<bool>.Failed(StatusCode.InternalServerError, "Project Could Not Be Deleted")
                : Result<bool>.Succeed(true);
        }

        public Result<bool> AddRewardPackage(int? projectId, int? userId,
            CreateRewardPackageOptions createRewardOptions)
        {
            if (projectId == null || userId == null || createRewardOptions.Quantity < 0 ||
                createRewardOptions.MinAmount <= 0 || createRewardOptions.MinAmount == null)
            {
                return Result<bool>.Failed(StatusCode.BadRequest,
                    "Project or User Not Specified. Reward Package Options Not Valid");
            }

            var project = GetProjectById(projectId);

            if (project == null)
            {
                return Result<bool>.Failed(StatusCode.NotFound, "Sorry, we couldn't find this page. But don't worry," +
                                                                " you can find plenty of other things in our homepage");
            }

            if (Helpers.UserOwnsProject(_context, userId, projectId) == false)
            {
                return Result<bool>.Failed(StatusCode.BadRequest, "Can Not Access A Project You Don't Own");
            }

            if (project.Data.RewardPackages.Any(r => r.MinAmount == createRewardOptions.MinAmount))
            {
                return Result<bool>.Failed(StatusCode.BadRequest,
                    "Can Not Create A Reward Package With The Same Value");
            }


            var reward = _rewardService.CreateRewardPackage(createRewardOptions);

            if (reward != null)
            {
                project.Data.RewardPackages.Add(reward);
            }

            var rows = 0;

            try
            {
                rows = _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return Result<bool>.Failed(StatusCode.InternalServerError, ex.Message);
            }

            return rows <= 0
                ? Result<bool>.Failed(StatusCode.InternalServerError, "Reward Package Could Not Be Created ")
                : Result<bool>.Succeed(true);
        }

        public Result<bool> UpdateRewardPackage(int? projectId, int? userId, int? rewardPackageId,
            UpdateRewardPackageOptions updateRewardOptions)
        {
            if (updateRewardOptions == null
                || projectId == null
                || userId == null
                || rewardPackageId == null
                || updateRewardOptions.Quantity < 0
                || updateRewardOptions.MinAmount <= 0)
            {
                return Result<bool>.Failed(StatusCode.BadRequest,
                    "Project Or User Not Specified Or Update Options Not Valid");
            }

            var project = GetProjectById(projectId);

            if (project == null)
            {
                return Result<bool>.Failed(StatusCode.NotFound, "Sorry, we couldn't find this page. But don't worry," +
                                                                " you can find plenty of other things in our homepage");
            }

            if (Helpers.UserOwnsProject(_context, userId, projectId) == false)
            {
                return Result<bool>.Failed(StatusCode.BadRequest, "Can Not Access A Project You Don't Own");
            }

            if (project.Data.RewardPackages.Any(r => r.MinAmount == updateRewardOptions.MinAmount))
            {
                return Result<bool>.Failed(StatusCode.BadRequest,
                    "Can Not Update A Reward Package With The Same Value");
            }


            var rewardPackageToUpdate = project.Data.RewardPackages
                .FirstOrDefault(rp => rp.RewardPackageId == rewardPackageId);

            var reward = _rewardService.UpdateRewardPackage(rewardPackageToUpdate, updateRewardOptions);

            if (reward == null)
            {
                return Result<bool>.Failed(StatusCode.NotFound, "Sorry, we couldn't find this page. But don't worry," +
                                                                " you can find plenty of other things in our homepage");
            }

            var rows = 0;

            try
            {
                rows = _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return Result<bool>.Failed(StatusCode.InternalServerError, ex.Message);
            }

            return rows <= 0
                ? Result<bool>.Failed(StatusCode.InternalServerError, "Reward Package Could Not Be Updated ")
                : Result<bool>.Succeed(true);
        }

        public Result<bool> DeleteRewardPackage(int? userId, int? projectId, int? rewardPackageId)
        {
            if (userId == null || projectId == null || rewardPackageId == null)
            {
                return Result<bool>.Failed(StatusCode.BadRequest, "Null Options");
            }

            if (Helpers.UserOwnsProject(_context, userId, projectId) == false)
            {
                return Result<bool>.Failed(StatusCode.BadRequest, "Can Not Access A Project You Don't Own");
            }

            var project = GetProjectById(projectId);

            try
            {
                var rewardToDelete = project.Data.RewardPackages
                    .FirstOrDefault(rp => rp.RewardPackageId == rewardPackageId);

                return rewardToDelete == null
                    ? Result<bool>.Failed(StatusCode.NotFound, "Reward Package Could Not Be Deleted")
                    : Result<bool>.Succeed(_rewardService.DeleteRewardPackage(rewardToDelete));
            }
            catch (Exception ex)
            {
                return Result<bool>.Failed(StatusCode.InternalServerError, ex.Message);
            }
        }

        public IList<Media> GetProjectPhotos(int? projectId)
        {
            if (projectId == null)
            {
                return null;
            }

            var project = GetProjectById(projectId);

            return project.Data.Medias.Where(p => p.MediaType == MediaType.Photo).ToList();
        }

        public IList<Media> GetProjectVideos(int? projectId)
        {
            if (projectId == null)
            {
                return null;
            }

            var project = GetProjectById(projectId);

            return project.Data.Medias.Where(p => p.MediaType == MediaType.Video).ToList();
        }
        
        public Result<bool> AddMedia(IEnumerable<CreateMediaOptions> createMediaOptions, int? userId, int? projectId)
        {
            if (createMediaOptions == null
                || projectId == null
                || userId == null)
            {
                return Result<bool>.Failed(StatusCode.BadRequest, "Options Not Valid");
            }

            if (Helpers.UserOwnsProject(_context, userId, projectId) == false)
            {
                return Result<bool>.Failed(StatusCode.BadRequest, "Can Not Access A Project You Don't Own");
            }

            var project = GetProjectById(projectId);

            foreach (var option in createMediaOptions)
            {
                var mediaResult = _mediaService.CreateMedia(option);
                
                if (mediaResult.Success)
                {
                    project.Data.Medias.Add(mediaResult.Data);
                }
                else
                {
                    return Result<bool>.Failed(mediaResult.ErrorCode, mediaResult.ErrorText);
                }
            }
            

            var rows = 0;

            try
            {
                rows = _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return Result<bool>.Failed(StatusCode.InternalServerError, ex.Message);
            }

            return rows <= 0
                ? Result<bool>.Failed(StatusCode.InternalServerError, "Media Could Not Be Created")
                : Result<bool>.Succeed(true);
        }

        public Result<bool> DeleteMedia(int? userId, int? projectId, int? mediaId)
        {
            if (userId == null || projectId == null || mediaId == null)
            {
                return Result<bool>.Failed(StatusCode.BadRequest, "Null Options");
            }

            var project = GetProjectById(projectId);

            if (Helpers.UserOwnsProject(_context, userId, projectId) == false)
            {
                return Result<bool>.Failed(StatusCode.BadRequest, "Can Not Access A Project You Don't Own");
            }

            try
            {
                var mediaToDelete = project.Data.Medias
                    .FirstOrDefault(m => m.MediaId == mediaId);

                return mediaToDelete == null
                    ? Result<bool>.Failed(StatusCode.NotFound, "Media Could Not Be Deleted")
                    : Result<bool>.Succeed(_mediaService.DeleteMedia(mediaToDelete));
            }
            catch (Exception ex)
            {
                return Result<bool>.Failed(StatusCode.InternalServerError, ex.Message);
            }
        }

        public Result<bool> AddPost(CreatePostOptions createPostOptions, int? userId, int? projectId)
        {
            if (createPostOptions == null
                || projectId == null
                || userId == null)
            {
                return Result<bool>.Failed(StatusCode.BadRequest, "Options Not Valid");
            }

            if (Helpers.UserOwnsProject(_context, userId, projectId) == false)
            {
                return Result<bool>.Failed(StatusCode.BadRequest, "Can Not Access A Project You Don't Own");
            }

            var project = GetProjectById(projectId);

            var post = _postService.CreatePost(createPostOptions);

            if (post != null)
            {
                project.Data.Posts.Add(post.Data);
            }

            var rows = 0;

            try
            {
                rows = _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return Result<bool>.Failed(StatusCode.InternalServerError, ex.Message);
            }

            return rows <= 0
                ? Result<bool>.Failed(StatusCode.InternalServerError, "Post Could Not Be Created")
                : Result<bool>.Succeed(true);
        }


        public Result<bool> UpdatePost(int? postId, int? userId, int? projectId, UpdatePostOptions updatePostOptions)
        {
            if (updatePostOptions == null || postId == null
                                          || projectId == null
                                          || userId == null)
            {
                return Result<bool>.Failed(StatusCode.BadRequest, "Options Not Valid");
            }

            if (Helpers.UserOwnsProject(_context, userId, projectId) == false)
            {
                return Result<bool>.Failed(StatusCode.BadRequest, "Can Not Access A Project You Don't Own");
            }

            var project = GetProjectById(projectId);

            var post = project.Data.Posts.SingleOrDefault(p => p.PostId == postId);

            if (post != null)
            {
                _postService.UpdatePost(post, updatePostOptions);
            }

            var rows = 0;

            try
            {
                rows = _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return Result<bool>.Failed(StatusCode.InternalServerError, ex.Message);
            }

            return rows <= 0
                ? Result<bool>.Failed(StatusCode.InternalServerError, "Post Could Not Be Updated")
                : Result<bool>.Succeed(true);
        }

        public Result<bool> DeletePost(int? postId, int? userId, int? projectId)
        {
            if (postId == null
                || projectId == null
                || userId == null)
            {
                return Result<bool>.Failed(StatusCode.BadRequest, "Null Options");
            }

            if (Helpers.UserOwnsProject(_context, userId, projectId) == false)
            {
                return Result<bool>.Failed(StatusCode.BadRequest, "Can Not Access A Project You Don't Own");
            }

            var project = GetProjectById(projectId);

            try
            {
                var postToDelete = project.Data.Posts.SingleOrDefault(p => p.PostId == postId);

                return postToDelete == null
                    ? Result<bool>.Failed(StatusCode.NotFound, "Post Could Not Be Deleted")
                    : Result<bool>.Succeed(_postService.DeletePost(postToDelete));
            }
            catch (Exception ex)
            {
                return Result<bool>.Failed(StatusCode.InternalServerError, ex.Message);
            }
        }

        public IList<Post> GetProjectPosts(int? projectId)
        {
            if (projectId == null)
            {
                return null;
            }

            var project = GetProjectById(projectId);

            return project.Data.Posts.OrderByDescending(p => p.CreatedAt).ToList();
        }
    }
}