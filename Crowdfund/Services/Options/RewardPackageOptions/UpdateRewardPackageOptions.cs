using System;
using System.Collections.Generic;
using System.Text;

namespace Crowdfund.Services.Options.RewardPackageOptions
{
    public class UpdateRewardPackageOptions
    {
        public int? UserId { get; set; }
        public int? ProjectId { get; set; }
        public int? RewardPackageId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal? MinAmount { get; set; }
        public int? Quantity { get; set; }
    }
}
