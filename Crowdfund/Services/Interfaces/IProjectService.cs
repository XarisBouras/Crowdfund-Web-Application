using Crowdfund.Models;
using Crowdfund.Services.Options.MediaOptions;
using Crowdfund.Services.Options.ProjectOptions;
using Crowdfund.Services.Options.RewardPackageOptions;
using System.Linq;

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
        
    }
}