using Blog.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Service.Interfaces
{
    public interface IPostService {
        public Post GetPost(int postId);
        public IEnumerable<Post> GetPosts(string searchString);
        IEnumerable<Post> GetPosts(ApplicationUser applicationUser);
        Task<Post> Add(Post posts);
        Task<Post> Update(Post posts);
        object GetPosts(int ıd);
        Task<Post> Update(object post);
    }
}

