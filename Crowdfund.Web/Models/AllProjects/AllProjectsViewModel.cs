using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crowdfund.Web.Models.AllProjects
{
    public class AllProjectsViewModel
    {
        public int ProjectId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int DueTo { get; set; }

        public string MainImageUrl { get; set; }

        public decimal Goal { get; set; }

        public int Backers { get; set; }

        public decimal BackingsAmount { get; set; }
        public decimal Progress { get; set; }
    }
}
