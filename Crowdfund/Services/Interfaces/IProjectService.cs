using System.Collections.Generic;
using Crowdfund.Models;
using Crowdfund.Services.Options.ProjectOptions;
using Crowdfund.Services.Options.RewardPackageOptions;
using System.Linq;
using Crowdfund.Services.CreateOptions;

namespace Crowdfund.Services.Interfaces
{
    public interface IProjectService
    {
        Project CreateProject(int? userId, CreateProjectOptions createProjectOptions);
        
        Project GetProjectById(int? id);
        
        Project GetSingleProject(int? id);

        IQueryable<Project> GetAllProjects();

        Project UpdateProject(UpdateProjectOptions updateProjectOptions);

        IQueryable<Project> SearchProjects(SearchProjectOptions searchProjectOptions);

        bool DeleteProject(int? userId, int? projectId);

        RewardPackage AddRewardPackage(int? projectId, int? userId, CreateRewardPackageOptions createRewardPackageOptions);

        RewardPackage UpdateRewardPackage(int? projectId, int? userId, int? rewardPackageId, UpdateRewardPackageOptions updateRewardPackageOptions);

        bool DeleteRewardPackage(int? userId, int? projectId, int? rewardPackageId);

        Media AddMedia(CreateMediaOptions createMediaOptions, int? userId, int? projectId);

        bool DeleteMedia(int? userId, int? projectId, int? mediaId);

        IList<Media> GetProjectPhotos(int? projectId);

        IList<Media> GetProjectVideos(int? projectId);
        
        IList<Post> GetProjectPosts(int? projectId);

        Post AddPost(CreatePostOptions createPostOptions, int? userId, int? projectId);

        bool DeletePost(int? postId, int? userId, int? projectId);

    }
}