namespace NewsSite.Models.Articles
{
    public class CombinedViewModel
    {
        public IEnumerable<Article>? ListModelArticles { get; set; }
        public IEnumerable<Category>? ListModelCategories { get; set; }

        public IEnumerable<ArticleCategory>? ListModelArticleCategories { get; set; }
    }
}
