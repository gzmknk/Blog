using Blog.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Service.Interfaces
{
    public interface IBlokService {
        public Blok GetBlok(int blokId);
        public IEnumerable<Blok> GetBloks(string searchString);
        IEnumerable<Blok> GetBloks(ApplicationUser applicationUser);
        Task<Blok> Add(Blok bloks);
        Task<Blok> Update(Blok bloks);


    }
}

