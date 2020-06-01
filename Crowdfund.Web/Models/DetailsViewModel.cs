using Crowdfund.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crowdfund.Web.Models.ProjectModel
{
    public class DetailsViewModel
    {
        public int ProjectId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int DaysToGo { get; set; }

        public string MainImageUrl { get; set; }

        public decimal Goal { get; set; }

        public Category Category { get; set; }

        public int Backers { get; set; }

        public decimal BackingsAmount { get; set; }
        public IList<RewardPackage> RewardPackages { get; set; }

        public IList<Post> Posts { get; set; }

        public IList<Media> Medias { get; set; }

        public IEnumerable<Project> InterestingProjects { get; set; }

        public DetailsViewModel()
        {
            RewardPackages = new List<RewardPackage>();
            Posts = new List<Post>();
            Medias = new List<Media>();           
        }
    }
}
