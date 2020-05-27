using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crowdfund.Core.Data;
using Crowdfund.Core.Services;
using Crowdfund.Core.Services.Interfaces;
using Crowdfund.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Crowdfund.Web.Controllers
{
    [Route("Project")]
    public class ProjectController : Controller
    {
        private readonly DataContext _context;

        private readonly IProjectService _projectService;
        //private readonly DataContext _context;
        /*private readonly IUserService _userService;
        private readonly IRewardService _rewardService;
        private readonly IMediaService _mediaService;
        private readonly IPostService _postService;
        private readonly IProjectService _projectService;*/

        public ProjectController(DataContext context, IProjectService projectService)
        {
            _context = context;
            _projectService = projectService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("{id}")]
        public IActionResult Details(int id)
        {

            var viewModel = _projectService.GetProjectById(id);
            if (!viewModel.Success)
            {
                return StatusCode((int)viewModel.ErrorCode,
                    viewModel.ErrorText);
            }

            return View(viewModel.Data);
        }

    }
}