using System.ComponentModel.DataAnnotations;

namespace NewsSite.Models.Articles
{
    public class CreateArticleViewModel
    {
        [Required]
        public string Title { get; set; }
        [Required(ErrorMessage = "The image file is mandatory")]
        [Display(Name = "Upload an Image")]
        public IFormFile ImageFile { get; set; }
        [Required]
        public string Content { get; set; }
        public string UserId { get; set; }
        public int CategoryId { get; set; }
    }
}
