using System.ComponentModel.DataAnnotations;

namespace NewsSite.Models.Articles
{
    public class CreateArticleViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required(ErrorMessage = "Снимката е задължителна")]
        [Display(Name = "Добави снимка")]
        public IFormFile ImageFile { get; set; }
        [Required]
        public string Content { get; set; }
        public string UserId { get; set; }
        public int CategoryId { get; set; }
    }
}
