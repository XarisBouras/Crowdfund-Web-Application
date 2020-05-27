using System;

namespace Crowdfund.Core.Services.Options.ProjectOptions
{
    public class CreateProjectOptions
    {
        public int UserId { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }
        
        public string MainImageUrl { get; set; }

        public DateTime? DueTo { get; set; }

        public decimal Goal { get; set; }
        
        public int CategoryId { get; set;}
    }
}