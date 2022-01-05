using Blog.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Service.Interfaces
{
    public interface IBlogService {
        public Blok GetBlok(int blokId);

        IEnumerable<Blok> GetBloks(ApplicationUser applicationUser);
        Task<Blok> Add(Blok bloks);
        Task<Blok> Update(Blok bloks);


    }
}

