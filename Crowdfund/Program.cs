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

            var userSvc = new UserService(dbCtx);
            var rewardSvc = new RewardService(dbCtx);
            var projectSvc = new ProjectService(dbCtx, userSvc, rewardSvc);
            var backingSvc = new BackingService(dbCtx, userSvc, projectSvc);

            // ===============================================================

            /*var user1 = userSvc.CreateUser(new CreateUserOptions
            {
                Email = "test_user1@test.com",
                FirstName = "TestUser1",
                LastName = "TestUse2rLastName"
            });
            Console.WriteLine(user1.UserId);
            

            var user2 = userSvc.CreateUser(new CreateUserOptions
            {
                Email = "test_user2@test.com",
                FirstName = "TestUser2",
                LastName = "TestUser2LastName"
            });
            Console.WriteLine(user2.UserId);
            
            //Create TestUser1's Project
            var project = projectSvc.CreateProject(new CreateProjectOptions
            {
                UserId = 1,
                Title = "Project 1",
                Description = "lorem ipsum....",
                CategoryId = 2,
                DueTo = new DateTime(2020, 12, 15),
                Goal = 1000
            });
            
            var rewardPkg1 = new CreateRewardPackageOptions
            {
                Title = "Reward Package 1",
                Description = "Get reward package 1",
                Quantity = 10,
                MinAmount = 10
            };

            var result = projectSvc.AddRewardPackage(project, rewardPkg1);
            
            Console.WriteLine(result);
            Console.WriteLine(project.Category);*/
            // ===========================================================
            
            // TestUser2 backs TestUsers'1 Project

            var backingSuccess = backingSvc.CreateBacking(2, 1, 1, 10);
            Console.WriteLine(backingSuccess);
            
            dbCtx.Dispose();
            // OLD === for reference ===
            //=========Project Creator=======================================================

            /*
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

            /*var user2 = new User
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
            
            dbCtx.SaveChanges();*/
            //============================================================================

            //var eligiblePackages = project.RewardPackages.Where(rp => rp.MinAmount >= 15);
        }
    }
}