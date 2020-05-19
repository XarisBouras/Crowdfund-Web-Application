using System;
using System.Linq;
using Crowdfund.Data;
using Crowdfund.Models;
using Crowdfund.Services.Interfaces;
using Crowdfund.Services.Options.UserOptions;
using Crowdfund.Services.SearchOptions;
using Crowdfund.Services.UpdateOptions;

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
            if (createUserOptions == null)
            {
                return null;
            }
            var user = new User()
            {
                FirstName = createUserOptions.FirstName,
                LastName = createUserOptions.LastName,
                Address = createUserOptions.Address
            };
            var validEmail = user.IsValidEmail(createUserOptions.Email);
            if (validEmail)
            {
                user.Email = createUserOptions.Email;
                Console.WriteLine("valid Email");
            }
            else
            {
                Console.WriteLine("Not valid Email, please try again!");
                return null;
            }
            _context.Add(user);       
            return _context.SaveChanges() > 0 ? user : null;
        }

        public User GetUserById(int id)
        {
            return _context.Set<User>().SingleOrDefault(u => u.UserId == id);
        }

        public IQueryable<User> SearchUser(SearchUserOptions options)
        {
            if (options == null)
            {
                return null;
            }

            var query = _context.Set<User>().AsQueryable();

            if (options.UserId != null)
            {
                query = query.Where(u => u.UserId == options.UserId);
            }
            if (!string.IsNullOrWhiteSpace(options.FirstName))
            {
                query = query.Where(c => c.FirstName == options.FirstName);
            }
            if (!string.IsNullOrWhiteSpace(options.LastName))
            {
                query = query.Where(c => c.LastName == options.LastName);
            }
            if (!string.IsNullOrWhiteSpace(options.Email))
            {
                query = query.Where(c => c.Email == options.Email);
            }
            if (!string.IsNullOrWhiteSpace(options.Address))
            {
                query = query.Where(c => c.Address == options.Address);
            }
            if (options.CreatedAt != null)
            {
                query = query.Where(c => c.CreatedAt == options.CreatedAt);
            }
            if (options.CreatedFrom != null)
            {
                query = query.Where(c => c.CreatedAt >= options.CreatedFrom);
            }
            if (options.CreatedTo != null)
            {
                query = query.Where(c => c.CreatedAt <= options.CreatedTo);
            }

            return query;

        }

        public User UpdateUser(UpdateUserOptions options)
        {
            if (options == null)
            {
                return null;
            }
            var user = _context.Set<User>().Where(u => u.UserId == options.UserId).SingleOrDefault();

            if (!string.IsNullOrEmpty(options.FirstName))
            {
                user.FirstName = options.FirstName;
            }
            if (!string.IsNullOrEmpty(options.Email))
            {
                var validEmail = user.IsValidEmail(options.Email);
                if (validEmail)
                {
                    user.Email = options.Email;
                    Console.WriteLine("valid Email");
                }
                else
                {
                    Console.WriteLine("Not valid Email, please try again!");
                    return null;
                }
                user.Email = options.Email;
            }
            if (!string.IsNullOrEmpty(options.LastName))
            {
                user.LastName = options.LastName;
            }
            if (!string.IsNullOrEmpty(options.Address))
            {
                user.Address = options.Address;
            }
            _context.SaveChanges();
            return (user);
        }
    }
}