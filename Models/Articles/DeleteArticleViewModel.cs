namespace NewsSite.Models.Articles
{
    public class DeleteArticleViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public byte[] Image { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Category { get; set; }
        public string User {  get; set; }
        public string ImageSrc
        {
            get
            {
                if (Image == null || Image.Length == 0)
                    return null;

                var base64 = Convert.ToBase64String(Image);
                return $"data:image/jpeg;base64,{base64}";
            }
        }
    }

}