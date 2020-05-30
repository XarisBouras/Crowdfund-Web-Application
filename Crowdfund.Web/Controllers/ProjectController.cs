using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crowdfund.Core.Data;
using Crowdfund.Core.Services;
using Crowdfund.Core.Services.Interfaces;
using Crowdfund.Core.Services.Options.BackingOptions;
using Crowdfund.Web.Models;
using Crowdfund.Web.Models.ProjectModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Crowdfund.Web.Controllers
{
    [Route("Project")]
    public class ProjectController : Controller
    {
        private readonly DataContext _context;

        private readonly IProjectService _projectService;
        private readonly IBackingService _backingService;

        //private readonly DataContext _context;
        /*private readonly IUserService _userService;
        private readonly IRewardService _rewardService;
        private readonly IMediaService _mediaService;
        private readonly IPostService _postService;
        private readonly IProjectService _projectService;*/

        public ProjectController(DataContext context, IProjectService projectService, IBackingService backingService)
        {
            _context = context;
            _projectService = projectService;
            _backingService = backingService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("{id}")]
        public IActionResult Details(int id)
        {
            var project = _projectService.GetProjectById(id);

            if (!project.Success)
            {
                return StatusCode((int)project.ErrorCode,
                    project.ErrorText);
            }

            var projectToView = new DetailsViewModel()
            {
                Title = project.Data.Title,
                Description = project.Data.Description,
                category = project.Data.Category,
                DueTo = (project.Data.DueTo - DateTime.Now).Days,
                Goal = project.Data.Goal,
                MainImageUrl = project.Data.MainImageUrl,
                Medias = project.Data.Medias,
                Posts = project.Data.Posts,
                RewardPackages = project.Data.RewardPackages,
                Backers = _backingService.GetProjectBackingsCount(id).Data,
                BackingsAmount = _backingService.GetProjectBackingsAmount(id).Data,
                InterestingProjects = _projectService.GetAllProjects().Where(p => p.ProjectId != id).Take(3)
            };
            

            return View(projectToView);

        }

        [HttpPost]
        public IActionResult Back([FromBody] CreateBackingOptions options)
        {
            var backResult = _backingService.CreateBacking(Globals.UserId, options.ProjectId,
                options.RewardPackageId, options.Amount);

            return Json(backResult.Data);
        }

    }
}