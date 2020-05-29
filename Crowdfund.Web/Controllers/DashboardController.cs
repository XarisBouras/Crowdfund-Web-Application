using System;
using System.Linq;
using Crowdfund.Core.Models;
using Crowdfund.Core.Services.Interfaces;
using Crowdfund.Core.Services.Options.ProjectOptions;
using Crowdfund.Web.Models.Dashboard;
using Microsoft.AspNetCore.Mvc;

namespace Crowdfund.Web.Controllers
{
    [Route("Dashboard/User")]
    public class DashboardController : Controller
    {
        private readonly IBackingService _backingService;
        private readonly IProjectService _projectService;

        public DashboardController(IBackingService backingService, IProjectService projectService)
        {
            _backingService = backingService;
            _projectService = projectService;
        }
        // GET
        [HttpGet("{id}")]
        public IActionResult Index(int id)
        {
            var projects = _backingService.GetUserProjects(id);
            
            if (!projects.Success)
            {
                return StatusCode((int)projects.ErrorCode,
                    projects.ErrorText);
            }

            var projectsToView = projects.Data.Select(p => new DashboardViewModel
            {
                ProjectId = p.ProjectId,
                Title = p.Title,
                Description = p.Description,
                MainImageUrl = p.MainImageUrl,
                DueTo = (p.DueTo - DateTime.Now).Days,
                Backers = _backingService.GetProjectBackingsCount(p.ProjectId).Data,
                BackingsAmount = _backingService.GetProjectBackingsAmount(p.ProjectId).Data,
                Goal = p.Goal
            });
            
            return View(projectsToView);
        }

        [HttpGet]
        public IActionResult CreateProject()
        {
            ViewBag.Categories = (Category []) Enum.GetValues(typeof(Category));
            return View();
        }


        [HttpPost]
        public IActionResult CreateProject(CreateProjectOptions options)
        {
            var result = _projectService.CreateProject(Globals.UserId, options);

            if (!result.Success)
            {
                return StatusCode((int)result.ErrorCode,
                    result.ErrorText);
            }

            return RedirectToAction("Index", new {id = Globals.UserId});

            //return Ok(result.Data);

        }
    }
}