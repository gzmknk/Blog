using Blog.BusinessManagers.Interfaces;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;


namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBlogBusinessManager blogBusinessManager;

        public HomeController(IBlogBusinessManager blogBusinessManager)
        {
            this.blogBusinessManager = blogBusinessManager;
        }

        public IActionResult Index(string searchString , int? page)
        {
            return View( blogBusinessManager .GetIndexViewModel(searchString , page));
        }
    }
}
