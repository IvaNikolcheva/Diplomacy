using System.ComponentModel.DataAnnotations;

namespace NewsSite.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public ICollection<ArticleCategory> ArticleCategories { get; set; }
    }
}
