using System.ComponentModel.DataAnnotations;

namespace NewsSite.Models
{
    public class Article
    {
        [Key]
        public int ArticleId { get; set; }
        public string Title { get; set; }
        public byte[] Image { get; set; }
        public string Content { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime PublishedDate { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User{ get; set; }
        public ICollection<ArticleCategory> ArticleCategories { get; set; }
    }
}
