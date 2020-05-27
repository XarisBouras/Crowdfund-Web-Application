using System;
using Crowdfund.Core.Data;
using Crowdfund.Core.Services;
using Crowdfund.Core.Services.Options.ProjectOptions;
using Crowdfund.Core.Services.Options.UserOptions;

namespace Crowdfund
{
    class Program
    {
        static void Main(string[] args)
        {
            var dbCtx = new DataContext(null);
            
            var userSvc = new UserService(dbCtx);
            var rewardSvc = new RewardService(dbCtx);
            var mediaSvc = new MediaService(dbCtx);
            var postSvc = new PostService(dbCtx);
            var projectSvc = new ProjectService(dbCtx, userSvc, rewardSvc, mediaSvc, postSvc);
            var backingSvc = new BackingService(dbCtx, userSvc, projectSvc);

            // ===============================================================//

            //------------Create users----------------------------//

            /*var user1 = userSvc.CreateUser(new CreateUserOptions
            {
                Email = "julie@test.com",
                FirstName = "julie",
                LastName = "kon",
                Address = "Aris"
            });
            Console.WriteLine(user1.Data.FirstName);*/


            //var user2 = userSvc.CreateUser(new CreateUserOptions
            //{
            //    Email = "julie@test.com",
            //    FirstName = "julie",
            //    LastName = "kon",
            //     Address = "Kalamata"
            //});
            //Console.WriteLine(user2.UserId);

            //--------------Get user by id---------------------------//

            //var user = userSvc.GetUserById(3);

            //Console.WriteLine(user.FirstName);


            //--------------Search user--------------//

            //var user = userSvc.SearchUser(new Services.SearchOptions.SearchUserOptions()
            //{
            //    FirstName = "Xaris"
            //}).FirstOrDefault();

            //Console.WriteLine(user.Email);

            //----------Update user---------------------//

            //var user = userSvc.UpdateUser(new Services.UpdateOptions.UpdateUserOptions()
            //{
            //    Address = "Kalamata55",
            //    UserId = 3
            //});


            //----------------Create TestUser1's Project---------------//

            var project = projectSvc.CreateProject(1, new CreateProjectOptions
            {
                Title = "Project 2",
                MainImageUrl = "assadsddasdf",
                Description = "aeroplano",
                CategoryId = 5,
                DueTo = new DateTime(2020, 12, 15),
                Goal = 55555
            });

            
            dbCtx.Dispose();
        }
    }
}