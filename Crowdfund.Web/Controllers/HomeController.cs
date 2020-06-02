﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Crowdfund.Core.Services.Interfaces;
using Crowdfund.Core.Services.Options.UserOptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Crowdfund.Web.Models;
using Microsoft.AspNetCore.Http;
using Crowdfund.Web.Models.Trendings;

namespace Crowdfund.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;
        private readonly IBackingService _backingService;

        public HomeController(ILogger<HomeController> logger, IUserService userService, IBackingService backingService)
        {
            _logger = logger;
            _userService = userService;
            _backingService = backingService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            HttpContext.Session.SetString("Test", "Session Test!");
            Globals.UserId = HttpContext.Session.GetInt32("UserId");
            var trendingProjects = _backingService.TrendingProjects();

            if (!trendingProjects.Success)
            {
                return StatusCode((int)trendingProjects.ErrorCode,
                    trendingProjects.ErrorText);
            }
            var trendingProjectsToView = trendingProjects.Data.Select(p => new TrendingsViewModel
            {
                ProjectId = p.ProjectId,
                Title = p.Title,
                Description = p.Description,
                MainImageUrl = p.MainImageUrl,
                DueTo = (p.DueTo - DateTime.Now).Days,
                Backers = _backingService.GetProjectBackingsCount(p.ProjectId).Data,
                BackingsAmount = _backingService.GetProjectBackingsAmount(p.ProjectId).Data,
                Goal = p.Goal,
                Progress =(int) Math.Round((_backingService.GetProjectBackingsAmount(p.ProjectId).Data) / (p.Goal) * 100)
            });

            return View(trendingProjectsToView);
            //return View();
        }
        
        [HttpPost]
        public IActionResult Index(CreateUserOptions userOptions) {
            var result = _userService.LoginUser(userOptions);
            
            if (!result.Success) {
                return StatusCode((int)result.ErrorCode,
                    result.ErrorText);
            }
            HttpContext.Session.SetInt32("UserId", result.Data);
            
            return Redirect("/");
        }

        public IActionResult Privacy()
        {
            ViewBag.Message = HttpContext.Session.GetString("Test");
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet]
        public IActionResult Tredings()
        {
            var trendingProjects = _backingService.TrendingProjects();

            if (!trendingProjects.Success)
            {
                return StatusCode((int)trendingProjects.ErrorCode,
                    trendingProjects.ErrorText);
            }
            var trendingProjectsToView = trendingProjects.Data.Select(p => new TrendingsViewModel
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
            });

            return View(trendingProjectsToView);

            }
        
    }
}
