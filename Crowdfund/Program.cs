using System;
using System.Linq;
using Crowdfund.Data;
using Crowdfund.Models;
using Crowdfund.Services;
using Crowdfund.Services.Options.ProjectOptions;
using Crowdfund.Services.Options.RewardPackageOptions;
using Crowdfund.Services.Options.UserOptions;
using Microsoft.EntityFrameworkCore;

namespace Crowdfund
{
    class Program
    {
        static void Main(string[] args)
        {
            var dbCtx = new DataContext();
            
            dbCtx.Dispose();
        }
    }
}