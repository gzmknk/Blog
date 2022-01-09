using Blog.BusinessManagers.Interfaces;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;


namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPostBusinessManager postBusinessManager;

        public HomeController(IPostBusinessManager postBusinessManager)
        {
            this.postBusinessManager = postBusinessManager;
        }

        public IActionResult Index(string searchString , int? page)
        {
            return View( postBusinessManager .GetIndexViewModel(searchString , page));
        }
    }
}
