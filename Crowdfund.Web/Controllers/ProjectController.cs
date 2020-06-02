using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crowdfund.Core.Data;
using Crowdfund.Core.Services;
using Crowdfund.Core.Services.Interfaces;
using Crowdfund.Core.Services.Options.BackingOptions;
using Crowdfund.Web.Models;
using Crowdfund.Web.Models.AllProjects;

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

            var projectToView = new DetailsViewModel
            {
                ProjectId = project.Data.ProjectId,
                Title = project.Data.Title,
                Description = project.Data.Description,
                Category = project.Data.Category,
                DaysToGo = (project.Data.DueTo - DateTime.Now).Days,
                Goal = project.Data.Goal,
                MainImageUrl = project.Data.MainImageUrl,
                Medias = project.Data.Medias,
                Posts = project.Data.Posts,
                RewardPackages = project.Data.RewardPackages,
                IsFirstImage = true,
                Backers = _backingService.GetProjectBackingsCount(id).Data,
                BackingsAmount = _backingService.GetProjectBackingsAmount(id).Data,
                Progress = (int)Math.Round((_backingService.GetProjectBackingsAmount(id).Data / project.Data.Goal) * 100),
                InterestingProjects = _projectService.GetAllProjects().Data.Where(p => p.ProjectId != id).Take(3)
            };


            return View(projectToView);

        }

        [HttpPost]
        public IActionResult Back([FromBody] CreateBackingOptions options)
        {
            var backResult = _backingService.CreateBacking(Globals.UserId, options.ProjectId,
                options.RewardPackageId, options.Amount);
            if(!backResult.Success)
            {
                return StatusCode((int)backResult.ErrorCode,
                    backResult.ErrorText);
            }

            return Json(backResult.Data);
        }

        [HttpGet]
        public IActionResult All()
        {
            var allProject = _projectService.GetAllProjects();

            if (!allProject.Success)
            {
                return StatusCode((int)allProject.ErrorCode,
                    allProject.ErrorText);
            }

            var projectsToView = allProject.Data.Select(p => new AllProjectsViewModel
            {
                ProjectId = p.ProjectId,
                Title = p.Title,
                Description = p.Description,
                MainImageUrl = p.MainImageUrl,
                DueTo = (p.DueTo - DateTime.Now).Days,
                Backers = _backingService.GetProjectBackingsCount(p.ProjectId).Data,
                BackingsAmount = _backingService.GetProjectBackingsAmount(p.ProjectId).Data,
                Goal = p.Goal,
                Progress = (int)Math.Round((_backingService.GetProjectBackingsAmount(p.ProjectId).Data) / (p.Goal) * 100)
            });

            return View(projectsToView);

        }

    }
}