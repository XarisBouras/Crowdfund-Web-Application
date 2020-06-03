using System;

namespace Crowdfund.Web.Models
{
    public class ProjectViewModel
    {
        public int ProjectId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int DaysToGo { get; set; }

        public string MainImageUrl { get; set; }

        public int Goal { get; set; }
        
        public int Backers { get; set; }

        public int BackingsAmount { get; set; }

        public int Progress { get; set; }
        
    }
}