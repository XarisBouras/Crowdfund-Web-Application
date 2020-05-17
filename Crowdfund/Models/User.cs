using System;
using System.Collections.Generic;

namespace Crowdfund.Models
{
    public class User
    {
        public int UserId { get; set; }
        
        public string Email { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public string Address { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public IList<UserProjectReward> UserProjectReward { get; set; }

        public User()
        {
            UserProjectReward = new List<UserProjectReward>();
        }
    }
}