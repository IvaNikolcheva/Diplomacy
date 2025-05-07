using System.ComponentModel.DataAnnotations;

namespace NewsSite.Models.Articles
{
    public class EditArticleViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required(ErrorMessage = "Images are mandatory")]
        [Display(Name = "Add an Image")]
        public IFormFile ImageFile { get; set; }
        public byte[] ExistingImage { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime PublishedDate { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}
