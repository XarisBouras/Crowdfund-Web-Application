/*using Crowdfund.Core.Services.Interfaces;
using Crowdfund.Core.Services.Options.UserOptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Crowdfund.Web.Controllers
{
    [Route("User")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        // GET
        [HttpPost]
        public IActionResult Login([FromBody] CreateUserOptions userOptions)
        {
            var result = _userService.CreateUser(userOptions).Data;
            HttpContext.Session.SetInt32("UserId", result.UserId);
            return Redirect("/");
        }
    }
}*/