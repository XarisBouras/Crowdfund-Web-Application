using Crowdfund.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Crowdfund.Services.Options.ProjectOptions
{
    public class SearchProjectOptions
    {
        public string SearchString { get; set; }

        public IList<int> CategoryIds { get; set; } 
    }
}
