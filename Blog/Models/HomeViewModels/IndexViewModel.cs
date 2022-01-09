using Blog.Data.Models;
using PagedList.Core;

namespace Blog.Models.HomeViewModels
{
    public class IndexViewModel
    {
        public IPagedList<Blok> Bloks {get; set;}
        public string SearchString { get; set; }
        public int PageNumber { get; set; }
    }
}
