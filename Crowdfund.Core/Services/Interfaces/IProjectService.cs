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