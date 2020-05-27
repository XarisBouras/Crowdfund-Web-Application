using Crowdfund.Core.Models;
using Crowdfund.Core.Services.Options.MediaOptions;
using Crowdfund.Core.Services.Options.PostOptions;
using Crowdfund.Core.Services.Options.ProjectOptions;
using Crowdfund.Core.Services.Options.RewardPackageOptions;
using System.Collections.Generic;
using System.Linq;

namespace Crowdfund.Core.Services.Interfaces
{
    public interface IProjectService
    {
        Result<Project> CreateProject(int? userId, CreateProjectOptions createProjectOptions);
        
        Result<Project> GetProjectById(int? id);
        
        Project GetSingleProject(int? id);

        IQueryable<Project> GetAllProjects();

        Result<Project> UpdateProject(UpdateProjectOptions updateProjectOptions);

        IQueryable<Project> SearchProjects(SearchProjectOptions searchProjectOptions);

        Result<bool> DeleteProject(int? userId, int? projectId);

        Result<bool> AddRewardPackage(int? projectId, int? userId, CreateRewardPackageOptions createRewardPackageOptions);

        Result<RewardPackage> UpdateRewardPackage(int? projectId, int? userId, int? rewardPackageId, UpdateRewardPackageOptions updateRewardPackageOptions);

        bool DeleteRewardPackage(int? userId, int? projectId, int? rewardPackageId);

        Result<Media> AddMedia(CreateMediaOptions createMediaOptions, int? userId, int? projectId);

        bool DeleteMedia(int? userId, int? projectId, int? mediaId);

        IList<Media> GetProjectPhotos(int? projectId);

        IList<Media> GetProjectVideos(int? projectId);
        
        IList<Post> GetProjectPosts(int? projectId);

        Result<Post> AddPost(CreatePostOptions createPostOptions, int? userId, int? projectId);

        Result<Post> UpdatePost(int? postId, int? userId, int? projectId, UpdatePostOptions updatePostOptions);

        bool DeletePost(int? postId, int? userId, int? projectId);

    }
}