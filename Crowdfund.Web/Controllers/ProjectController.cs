using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crowdfund.Core.Data;
using Crowdfund.Core.Services;
using Crowdfund.Core.Services.Interfaces;
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

            var viewModel = _projectService.GetProjectById(id);
            if (!viewModel.Success)
            {
                return StatusCode((int)viewModel.ErrorCode,
                    viewModel.ErrorText);
            }
            ViewBag.Message = HttpContext.Session.GetString("Test");
            return View(viewModel.Data);
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
                Progress = Math.Round((_backingService.GetProjectBackingsAmount(p.ProjectId).Data) / (p.Goal) * 100, 2)
            }) ;

            return View(projectsToView);

        }

    }
}