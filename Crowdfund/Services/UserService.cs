using System.Linq;
using Crowdfund.Data;
using Crowdfund.Models;
using Crowdfund.Services.Interfaces;
using Crowdfund.Services.Options.UserOptions;

namespace Crowdfund.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;

        public UserService(DataContext context)
        {
            _context = context;
        }

        public User CreateUser(CreateUserOptions createUserOptions)
        {
            var user = new User
            {
                Email = createUserOptions.Email,
                Address = createUserOptions.Address,
                FirstName = createUserOptions.FirstName,
                LastName = createUserOptions.LastName
            };
            _context.Set<User>().Add(user);
            return _context.SaveChanges() > 0 ? user : null;
        }

        public User GetUserById(int id)
        {
            return _context.Set<User>().SingleOrDefault(u => u.UserId == id);
        }
    }
}