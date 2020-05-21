using System;
using System.Collections.Generic;
using System.Text;

namespace Crowdfund.Services.Options.RewardPackageOptions
{
    public class UpdateRewardPackageOptions
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal? MinAmount { get; set; }
        public int? Quantity { get; set; }
    }
}
