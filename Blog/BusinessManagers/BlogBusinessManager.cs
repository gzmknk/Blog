using Blog.Authorization;
using Blog.BusinessManagers.Interfaces;
using Blog.Data.Models;
using Blog.Models.BlogViewModels;
using Blog.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Blog.BusinessManagers
{
    public class BlogBusinessManager : IBlogBusinessManager {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IBlogService blogService;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IAuthorizationService authorizationService;

        public BlogBusinessManager(
            UserManager<ApplicationUser> userManager,
            IBlogService blogService,
            IWebHostEnvironment webHostEnvironment,
            IAuthorizationService authorizationService) {
            this.userManager = userManager;
            this.blogService = blogService;
            this.webHostEnvironment = webHostEnvironment;
            this.authorizationService = authorizationService;
        }
        public async Task<Blok> CreateBlog(CreateViewModel createViewModel, ClaimsPrincipal claimsPrincipal) {
            Blok blok = createViewModel.Blok;

            blok.Creator = await userManager.GetUserAsync(claimsPrincipal);
            blok.CreatedOn = DateTime.Now;
            blok.UpDateOn = DateTime.Now;


            blok = await blogService.Add(blok);

            string webRootPath = webHostEnvironment.WebRootPath;
            string pathToImage = $@"{webRootPath}\UserFiles\Bloks\{blok.Id}\HeaderImage.jpg";

            EnsureFoleder(pathToImage);

            using (var fileStream = new FileStream(pathToImage, FileMode.Create)) {
                await createViewModel.BlokHeaderImage.CopyToAsync(fileStream);
            }

            return await blogService.Add(blok);
           

        }

        public async Task<ActionResult<EditViewModel>> UpdateBlok(EditViewModel editViewModel, ClaimsPrincipal claimsPrincipal) {
            var blok = blogService.GetBlok(editViewModel.Blok.Id);

            if (blok is null)
                return new NotFoundResult();

            var authorizationResult = await authorizationService.AuthorizeAsync(claimsPrincipal, blok, Operations.Update);

            if (!authorizationResult.Succeeded) return DetermineActionResult(claimsPrincipal);

            blok.Published = editViewModel.Blok.Published;
            blok.Title = editViewModel.Blok.Title;
            blok.Content = editViewModel.Blok.Content;
            blok.UpDateOn = DateTime.Now;

            if (editViewModel.BlokHeaderImage != null)
            {
                string webRootPath = webHostEnvironment.WebRootPath;
                string pathToImage = $@"{webRootPath}\ UserFiles\Bloks\{blok.Id}\HeaderImage.jpg";


                EnsureFoleder(pathToImage);

                using (var fileStream = new FileStream(pathToImage, FileMode.Create)) {
                    await editViewModel.BlokHeaderImage.CopyToAsync(fileStream);
                }

            }
            return new EditViewModel
            {
                Blok = await blogService.Update(blok)
            };

        }



        public async Task<ActionResult<EditViewModel>> GetEditViewModel(int? id, ClaimsPrincipal claimsPrincipal) {
            if (id is null)
                return new BadRequestResult();

            var blokId = id.Value;
            var blok = blogService.GetBlok(blokId);

            if (blok is null)
                return new NotFoundResult();



            var authorizationResult = await authorizationService.AuthorizeAsync(claimsPrincipal, blok, Operations.Update);

            if (!authorizationResult.Succeeded) return DetermineActionResult(claimsPrincipal);

            return new EditViewModel {
                Blok = blok
            };
        }

        private ActionResult DetermineActionResult(ClaimsPrincipal claimsPrincipal) {
            if (claimsPrincipal.Identity.IsAuthenticated)
                return new ForbidResult();
            else 
                return new ChallengeResult();
       
        
        }
         
        private void EnsureFoleder (string path) {

            string directoryName = Path.GetDirectoryName(path);
            if(directoryName.Length > 0) {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
        }
    }
}
