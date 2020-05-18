using Crowdfund.Data;
using Crowdfund.Models;
using Crowdfund.Services.Options.UserOptions;

namespace Crowdfund.Services.Interfaces
{
    public interface IUserService
    {
        User CreateUser(CreateUserOptions createUserOptions);
        User GetUserById(int id);
    }
}