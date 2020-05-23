namespace Crowdfund.Core.Services.Options.RewardPackageOptions
{
    public class CreateRewardPackageOptions
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public decimal? MinAmount { get; set; }

        public int? Quantity { get; set; }
    }
}