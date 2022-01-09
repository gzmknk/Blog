using Blog.Authorization;
using Blog.BusinessManagers;
using Blog.BusinessManagers.Interfaces;
using Blog.Data;
using Blog.Data.Models;
using Blog.Service;
using Blog.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace Blog.Configuration
{
    public static class AppServices {
        public static void AddDefaultServices(this IServiceCollection serviceCollection , IConfiguration configuration )
        {
            serviceCollection.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")));
            serviceCollection.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            serviceCollection.AddControllersWithViews().AddRazorRuntimeCompilation();
            serviceCollection.AddRazorPages();

            serviceCollection.AddSingleton<IFileProvider>(new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));

        }
        public static void AddCustomServices(this IServiceCollection serviceCollection) {
            serviceCollection.AddScoped<IPostBusinessManager, PostBusinessManager>();
            serviceCollection.AddScoped<IAdminBusinessManager, AdminBusinessManagers>();

            serviceCollection.AddScoped<IPostService, PostService>();
        }
        public static void AddCustomAuthorization(this IServiceCollection serviceCollection)  {
            serviceCollection.AddTransient<IAuthorizationHandler, PostAuthorizationHandler>();

        }
    }
}
