using Blog.Data.Models;
using System.Collections.Generic;

namespace Blog.Models.AdminViewModel
{
    public class IndexViewModel
    {
        public IEnumerable<Blok> Bloks { get; set; }
    }
}
