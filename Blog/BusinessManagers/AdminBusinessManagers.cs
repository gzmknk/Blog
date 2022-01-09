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
        private readonly IBlokService blogService;

        public AdminBusinessManagers(
            UserManager<ApplicationUser> userManager ,
            IBlokService blogService) {
            this.userManager = userManager;
            this.blogService = blogService;
        }

        public async Task<IndexViewModel>GetAdminDashboard(ClaimsPrincipal claimsPrincipal) {
            var applicationUser = await userManager.GetUserAsync(claimsPrincipal);
            return new IndexViewModel { 
                Bloks = blogService.GetBloks(applicationUser)
            };
        }
       
    }
    
 }

