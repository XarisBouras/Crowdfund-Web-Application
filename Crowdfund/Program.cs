using System;
using System.Linq;
using Crowdfund.Data;
using Crowdfund.Models;
using Microsoft.EntityFrameworkCore;

namespace Crowdfund
{
    class Program
    {
        static void Main(string[] args)
        {
            var dbCtx = new DataContext();
            //=========Project Creator=======================================================
            
            /*var user = new User
            {
                Email = "test@test.com",
                FirstName = "TestUser",
                LastName = "TestUserLastName"
            };
            
            dbCtx.Add(user);

            var project = new Project()
            {
                Title = "ArtProject",
                Category = Category.Art,
                DueTo = new DateTime(2020, 9, 30),
                Goal = 10000,
                Description = "lorem ipsum..."
            };

            var post1 = new Post
            {
                Text = "Post1"
            };

            var post2 = new Post
            {
                Text = "Post2"
            };

            project.Posts.Add(post1);
            project.Posts.Add(post2);

            var media = new Media
            {
                MediaType = MediaType.Photo,
                MediaUrl = "http://photourl.test/photo.jpg"
            };
            project.Medias.Add(media);

            var rewardPkg1 = new RewardPackage
            {
                Quantity = 15,
                MinAmount = 10,
                Description = "stylo"
            };
            
            var rewardPkg2 = new RewardPackage
            {
                MinAmount = 20,
                Description = "koupa"
            };

            project.RewardPackages.Add(rewardPkg1);
            project.RewardPackages.Add(rewardPkg2);

            dbCtx.Add(project);

            var userProject = new UserProjectReward
            {
                IsOwner = true,
                User = user,
                Project = project
            };

            user.UserProjectReward.Add(userProject);

            dbCtx.SaveChanges();*/
            
            //============================================================================
            //==========Project Backer=======================================================
            
            var user2 = new User
            {
                FirstName = "TestUser2",
                LastName = "TestUser2LastName",
                Email = "test2@test.com"
            };

            dbCtx.Add(user2);
            dbCtx.SaveChanges();

            var loggedInUser = dbCtx.Set<User>().SingleOrDefault(u => u.UserId == 2);
            Console.WriteLine(loggedInUser.UserId);

            var project = dbCtx.Set<UserProjectReward>()
                .Include(p => p.Project).ThenInclude(p => p.RewardPackages)
                .FirstOrDefault(u => u.UserId == 1 && u.ProjectId == 1 && u.IsOwner == true)?.Project;
            
            var  projectReward = project?.RewardPackages.FirstOrDefault(rp => rp.RewardPackageId == 1);

            var user2ProjectBacking = new UserProjectReward
            {
                IsOwner = false,
                User = loggedInUser,
                Project = project,
                RewardPackage = projectReward,
                Amount = 10
            };
            
            loggedInUser.UserProjectReward.Add(user2ProjectBacking);
            
            dbCtx.SaveChanges();
            //============================================================================
            
            //var eligiblePackages = project.RewardPackages.Where(rp => rp.MinAmount >= 15);

            
            Console.WriteLine("OK");
            dbCtx.Dispose();
        }
    }
}
