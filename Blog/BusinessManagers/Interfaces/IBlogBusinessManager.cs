using Blog.Data.Models;
using Blog.Models.BlogViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Blog.BusinessManagers.Interfaces
{
    public interface IBlogBusinessManager
    {
        Task<Blok> CreateBlog(CreateViewModel createBlogViewModel, ClaimsPrincipal claimsPrincipal);

        Task<ActionResult<EditViewModel>> UpdateBlok(EditViewModel editViewModel, ClaimsPrincipal claimsPrincipal);
        Task<ActionResult<EditViewModel>> GetEditViewModel(int? id, ClaimsPrincipal claimsPrincipal);
     }
}

