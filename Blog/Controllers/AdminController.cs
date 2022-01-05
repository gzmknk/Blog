﻿using Blog.BusinessManagers.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Blog.Controllers{
    [Authorize]
    public class AdminController : Controller{
        private readonly IAdminBusinessManager adminBusinessManager;

        public AdminController (IAdminBusinessManager adminBusinessManager) {
            this.adminBusinessManager = adminBusinessManager;
        }
        public async Task<IActionResult>Index()  {
            return View(await adminBusinessManager.GetAdminDashboard(User));
        }
    }
}
