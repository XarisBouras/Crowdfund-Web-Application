using Crowdfund.Models;
using Crowdfund.Services.Options.ProjectOptions;
using Crowdfund.Services.Options.RewardPackageOptions;

namespace Crowdfund.Services.Interfaces
{
    public interface IProjectService
    {
        Project CreateProject(CreateProjectOptions createProjectOptions);
        
        Project GetProjectById(int id);
        
        bool AddRewardPackage(Project project, CreateRewardPackageOptions rewardPackage);
    }
}