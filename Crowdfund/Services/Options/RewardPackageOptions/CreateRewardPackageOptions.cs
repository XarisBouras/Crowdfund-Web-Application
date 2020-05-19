namespace Crowdfund.Services.Options.RewardPackageOptions
{
    public class CreateRewardPackageOptions
    {
        public int? UserId { get; set; }
        public int? ProjectId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public decimal? MinAmount { get; set; }

        public int? Quantity { get; set; }
    }
}