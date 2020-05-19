using System;

namespace Crowdfund.Services.SearchOptions
{
    public class SearchUserOptions
    {
        public int? UserId { get; set; }
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }

    }
}
