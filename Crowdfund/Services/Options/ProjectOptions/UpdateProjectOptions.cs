﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Crowdfund.Services.Options.ProjectOptions
{
    public class UpdateProjectOptions
    {
        public int? UserId { get; set; }
        public int? ProjectId { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime? DueTo { get; set; }

        public decimal? Goal { get; set; }
    }
}
