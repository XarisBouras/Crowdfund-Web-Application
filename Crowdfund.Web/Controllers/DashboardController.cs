using System;
using System.Linq;
using Crowdfund.Core.Services.Interfaces;
using Crowdfund.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Crowdfund.Web.Controllers
{
    [Route("User/{id}/Dashboard")]
    public class DashboardController : Controller
    {
        private readonly IBackingService _backingService;

        public DashboardController(IBackingService backingService)
        {
            _backingService = backingService;
        }
        // GET
        [HttpGet]
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
    }
}