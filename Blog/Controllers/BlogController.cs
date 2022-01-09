using Blog.BusinessManagers.Interfaces;
using Blog.Models.BlogViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Blog.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogBusinessManager blogBusinessManager;
        public BlogController(IBlogBusinessManager blogBusinessManager) {
            this.blogBusinessManager = blogBusinessManager;
        }

        public IActionResult Index() {
            return View();
        }


        public IActionResult Create() {
            return View(new CreateViewModel());
        }

        public async Task<IActionResult> Edit(int? id) {
            var actionResult = await blogBusinessManager.GetEditViewModel(id, User);

            if (actionResult.Result is null)
                return View(actionResult.Value);

            return actionResult.Result;
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateViewModel createViewModel) {
           await blogBusinessManager.CreateBlok(createViewModel, User);
            return RedirectToAction("Create");
        }

        [HttpPost]
        public async Task<IActionResult> Update(EditViewModel editViewModel)
        {
          var actionResult =  await blogBusinessManager.UpdateBlok(editViewModel, User);

            if (actionResult.Result is null)
                return RedirectToAction("Edit", new { editViewModel.Blok.Id });
          
            
            return actionResult.Result;
        }
    }
}
