using System.ComponentModel.DataAnnotations;

namespace NewsSite.Models.Articles
{
    public class CreateArticleViewModel
    {
        [Required]
        public string Title { get; set; }
        [Required(ErrorMessage = "Images are mandatory")]
        [Display(Name = "Add an Image")]
        public IFormFile ImageFile { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}
