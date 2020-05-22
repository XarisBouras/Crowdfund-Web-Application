using Crowdfund.Models;
using Crowdfund.Services.Options.UserOptions;
using Crowdfund.Services.SearchOptions;
using Crowdfund.Services.UpdateOptions;
using System.Linq;

namespace Crowdfund.Services.Interfaces
{
    public interface IUserService
    {
        User CreateUser(CreateUserOptions createUserOptions);
        User GetUserById(int? id);
        IQueryable<User> SearchUser(SearchUserOptions options);
        User UpdateUser(UpdateUserOptions options);
    }
}