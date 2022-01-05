using System;
namespace Blog.Data.Models
{
    public class Post
    {
        public int Id { get; set; }
        public Blok Blok { get; set; }
        public ApplicationUser Poser { get; set; }
        public string Content { get; set; }
        public Post Parent { get; set; }

        public DateTime CreateOn { get; set; }

    }
}
