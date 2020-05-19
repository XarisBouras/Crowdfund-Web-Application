using Crowdfund.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Crowdfund.Services.Options.ProjectOptions
{
    public class SearchProjectOptions
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IList<Category> Category { get; set; } 
    }
}
