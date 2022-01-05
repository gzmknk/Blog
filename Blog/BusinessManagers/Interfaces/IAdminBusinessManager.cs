using Blog.Models.AdminViewModel;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Blog.BusinessManagers.Interfaces
{
    public interface IAdminBusinessManager {
        Task<IndexViewModel> GetAdminDashboard(ClaimsPrincipal claimsPrincipal);
    }
}
