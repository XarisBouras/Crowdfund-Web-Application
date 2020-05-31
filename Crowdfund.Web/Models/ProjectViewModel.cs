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

        public decimal Goal { get; set; }
        
        public int Backers { get; set; }

        public decimal BackingsAmount { get; set; }
        
    }
}