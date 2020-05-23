﻿using System;
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
        private readonly IUserService _userService;
        private readonly IRewardService _rewardService;
        private readonly IMediaService _mediaService;
        private readonly IPostService _postService;
        private readonly IProjectService projectService_;

        public ProjectController()
        {
            _context = new DataContext();
            projectService_ = new ProjectService(_context, _userService,
             _rewardService, _mediaService, _postService);
            
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("{id}")]
        public IActionResult Details(int id)
        {
            var viewModel = projectService_.GetProjectById(id);

            return View(viewModel);
        }

    }
}