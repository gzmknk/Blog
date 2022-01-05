using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Blog.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace Blog.Authorization
{
    public class BlogAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Blok> {
        private readonly UserManager<ApplicationUser> userManager;

        public BlogAuthorizationHandler(UserManager<ApplicationUser> userManager) {
            this.userManager = userManager;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, Blok resource) {
            var applicationUser = await userManager.GetUserAsync(context.User);
           if((requirement.Name == Operations.Update.Name || requirement.Name == Operations.Delete.Name) && applicationUser == resource.Creator){
                context.Succeed(requirement);

            }
        }
    }
}
  