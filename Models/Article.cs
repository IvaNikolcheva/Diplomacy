using System.ComponentModel.DataAnnotations;

namespace NewsSite.Models
{
    public class Article
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public byte[] Image { get; set; }
        public string Content { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime PublishedDate { get; set; }
        public ICollection<ApplicationUser> AppUsers { get; set; }
        public ICollection<Category> Categories { get; set; }
    }
}
