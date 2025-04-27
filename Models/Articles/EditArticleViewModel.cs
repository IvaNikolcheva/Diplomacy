using System.ComponentModel.DataAnnotations;

namespace NewsSite.Models.Articles
{
    public class EditArticleViewModel
    {
        [Required]
        public string Title { get; set; }
        [Required(ErrorMessage = "Images are mandatory")]
        [Display(Name = "Add an Image")]
        public IFormFile ImageFile { get; set; }
        public byte[] Image { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}
