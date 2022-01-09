﻿using Blog.Authorization;
using Blog.BusinessManagers.Interfaces;
using Blog.Data.Models;
using Blog.Models.HomeViewModels;
using Blog.Models.PostViewModels;
using Blog.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PagedList.Core;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Blog.BusinessManagers
{
    public class PostBusinessManager : IPostBusinessManager
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IPostService postService;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IAuthorizationService authorizationService;

        public PostBusinessManager(
            UserManager<ApplicationUser> userManager,
            IPostService postService,
            IWebHostEnvironment webHostEnvironment,
            IAuthorizationService authorizationService)
        {
            this.userManager = userManager;
            this.postService = postService;
            this.webHostEnvironment = webHostEnvironment;
            this.authorizationService = authorizationService;
        }
        public IndexViewModel GetIndexViewModel(string searchString, int? page)
        {
            int pageSize = 20;
            int pageNumber = page ?? 1;
            var posts = postService.GetPosts(searchString ?? string.Empty)
                .Where(post => post.Published);

            return new IndexViewModel
            {
                Posts = new StaticPagedList<Post>(posts.Skip((pageNumber - 1) * pageSize).Take(pageSize), pageNumber, pageSize, posts.Count()),
                SearchString = searchString,
                PageNumber = pageNumber
            };
        }
        public async Task<ActionResult<PostViewModel>> GetPostViewModel (int? id, ClaimsPrincipal claimsPrincipal) {

            if (id is null)
                return new BadRequestResult();

            var postId = id.Value;
            var post = postService.GetPost(postId);

            if (post is null)
                return new NotFoundResult();

            if (!post.Published) {
                var authorizationResult = await authorizationService.AuthorizeAsync(claimsPrincipal, post, Operations.Read);

                if (!authorizationResult.Succeeded) return DetermineActionResult(claimsPrincipal);
            }
            return new PostViewModel{
                Post = post
            };
        }


        public async Task<Post> CreatePost(CreateViewModel createViewModel, ClaimsPrincipal claimsPrincipal)
        {
            Post post = createViewModel.Post;

            post.Creator = await userManager.GetUserAsync(claimsPrincipal);
            post.CreatedOn = DateTime.Now;
            post.UpDateOn = DateTime.Now;


            post = await postService.Add(post);

            string webRootPath = webHostEnvironment.WebRootPath;
            string pathToImage = $@"{webRootPath}\UserFiles\Posts\{post.Id}\HeaderImage.jpg";

            EnsureFoleder(pathToImage);

            using (var fileStream = new FileStream(pathToImage, FileMode.Create))
            {
                await createViewModel.HeaderImage.CopyToAsync(fileStream);
            }

            return await postService.Add(post);


        }

        public async Task<ActionResult<EditViewModel>> UpdatePost(EditViewModel editViewModel, ClaimsPrincipal claimsPrincipal)
        {
            var post = postService.GetPost(editViewModel.Post.Id);

            if (post is null)
                return new NotFoundResult();

            var authorizationResult = await authorizationService.AuthorizeAsync(claimsPrincipal, post, Operations.Update);

            if (!authorizationResult.Succeeded) return DetermineActionResult(claimsPrincipal);

            post.Published = editViewModel.Post.Published;
            post.Title = editViewModel.Post.Title;
            post.Content = editViewModel.Post.Content;
            post.UpDateOn = DateTime.Now;

            if (editViewModel.HeaderImage != null)
            {
                string webRootPath = webHostEnvironment.WebRootPath;
                string pathToImage = $@"{webRootPath}\ UserFiles\Posts\{post.Id}\HeaderImage.jpg";


                EnsureFoleder(pathToImage);

                using (var fileStream = new FileStream(pathToImage, FileMode.Create))
                {
                    await editViewModel.HeaderImage.CopyToAsync(fileStream);
                }

            }
            return new EditViewModel
            {
                Post = await postService.Update(post)
            };

        }



        public async Task<ActionResult<EditViewModel>> GetEditViewModel(int? id, ClaimsPrincipal claimsPrincipal)
        {
            if (id is null)
                return new BadRequestResult();

            var postId = id.Value;
            var post = postService.GetPost(postId);

            if (post is null)
                return new NotFoundResult();



            var authorizationResult = await authorizationService.AuthorizeAsync(claimsPrincipal, post, Operations.Update);

            if (!authorizationResult.Succeeded) return DetermineActionResult(claimsPrincipal);

            return new EditViewModel
            {
                Post = post
            };
        }

        private ActionResult DetermineActionResult(ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal.Identity.IsAuthenticated)
                return new ForbidResult();
            else
                return new ChallengeResult();


        }

        private void EnsureFoleder(string path)
        {

            string directoryName = Path.GetDirectoryName(path);
            if (directoryName.Length > 0)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
        }
    }
}
