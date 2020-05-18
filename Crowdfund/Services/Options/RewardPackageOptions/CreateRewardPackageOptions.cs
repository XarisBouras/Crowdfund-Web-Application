namespace Crowdfund.Services.Options.RewardPackageOptions
{
    public class CreateRewardPackageOptions
    {
        public string Description { get; set; }

        public decimal MinAmount { get; set; }

        public int? Quantity { get; set; }
    }
}