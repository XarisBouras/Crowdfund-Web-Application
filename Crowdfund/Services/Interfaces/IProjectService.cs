using Crowdfund.Models;
using Crowdfund.Services.Options.RewardPackageOptions;
using Crowdfund.Services.Options.ProjectOptions;
using System.Linq;
using Crowdfund.Services.Options.MediaOptions;
using Crowdfund.Services.CreateOptions;
using Crowdfund.Services.UpdateOptions;
using System.Collections;
using System.Collections.Generic;

namespace Crowdfund.Services.Interfaces
{
    public interface IProjectService
    {
        Project CreateProject(CreateProjectOptions createProjectOptions);

        Project GetProjectById(int? id);

        Project UpdateProject(UpdateProjectOptions updateProjectOptions);

        IQueryable<Project> SearchProject(SearchProjectOptions searchProjectOptions);

        bool DeleteProject(int? userId, int? projectId);

        RewardPackage AddRewardPackage(CreateRewardPackageOptions createRewardPackageOptions);

        RewardPackage UpdateRewardPackage(UpdateRewardPackageOptions updateRewardPackageOptions);

        bool DeleteRewardPackage(int? userId, int? projectId, int? rewardPackageId);

        Media AddMedia(CreateMediaOptions createMediaOptions, int? userId, int? projectId);

        bool DeleteMedia(int? userId, int? projectId, int? mediaId);
        Post AddPost(CreatePostOptions createPostOptions, int? userId, int? projectId);
        Post UpdatePost(UpdatePostOptions updateOptions, int? userId, int? projectId, int? postId);
        IList<Post> GetAllPosts(int? userId, int? projectId);
        bool DeletePost(int? userId, int? projectId, int? postId);
    }
}
