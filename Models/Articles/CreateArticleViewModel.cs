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
        public DateTime PublishedDate { get; set; } = DateTime.Now;
        public string UserId { get; set; }
        public List<CategoryCheckboxItem> Categories { get; set; }
    }
    public class CategoryCheckboxItem
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool IsSelected { get; set; }
    }
}
