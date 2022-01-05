using Blog.Data.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Blog.Models.BlogViewModels
{
    public class CreateViewModel
    {
        [Required,Display(Name ="Header Image")]
        public IFormFile BlokHeaderImage { get; set; }
        public Blok Blok { get; set; }
       
    }
}
 