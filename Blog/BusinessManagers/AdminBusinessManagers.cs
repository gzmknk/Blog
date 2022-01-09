using Blog.BusinessManagers.Interfaces;
using Blog.Data.Models;
using Blog.Models.AdminViewModel;
using Blog.Service.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Blog.BusinessManagers{
    
    public class AdminBusinessManagers : IAdminBusinessManager {
        private UserManager<ApplicationUser> userManager;
        private readonly IPostService postService;

        public AdminBusinessManagers(
            UserManager<ApplicationUser> userManager ,
            IPostService blogService) {
            this.userManager = userManager;
            this.postService = blogService;
        }

        public async Task<IndexViewModel>GetAdminDashboard(ClaimsPrincipal claimsPrincipal) {
            var applicationUser = await userManager.GetUserAsync(claimsPrincipal);
            return new IndexViewModel { 
                Posts = postService.GetPosts(applicationUser)
            };
        }
       
    }
    
 }

