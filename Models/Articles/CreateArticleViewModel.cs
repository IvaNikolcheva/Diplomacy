using System.ComponentModel.DataAnnotations;

namespace NewsSite.Models.Articles
{
    public class CreateArticleViewModel
    {
        public string Title { get; set; }
        [Display(Name = "Upload Image")]
        public IFormFile ImageFile { get; set; }
        public string Content { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime PublishedDate { get; set; }
        public string UserId { get; set; }
    }
}
