using Crowdfund.Core.Models;
using Crowdfund.Core.Services.Options.UserOptions;
using System.Linq;

namespace Crowdfund.Core.Services.Interfaces
{
    public interface IUserService
    {
        User CreateUser(CreateUserOptions createUserOptions);
        User GetUserById(int? id);
        IQueryable<User> SearchUser(SearchUserOptions options);
        User UpdateUser(UpdateUserOptions options);
    }
}