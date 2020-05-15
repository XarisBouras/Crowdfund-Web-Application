using System.Collections.Generic;

namespace TestProject.Models
{
    public class RewardPackage
    {
        public int RewardPackageId { get; set; }
        
        public string Description { get; set; } 
        
        public decimal MinAmount { get; set; } 
        
        public int? Quantity { get; set; }
    }
}
