using System.Collections.Generic;

namespace Crowdfund.Core.Services.Options.ProjectOptions
{
    public class SearchProjectOptions
    {
        public string SearchString { get; set; }

        public IList<int> CategoryIds { get; set; } 
    }
}
