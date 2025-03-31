using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NewsSite.Models;

namespace NewsSite.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<Article> Articles { get; set; }

        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Article>()
                .HasMany(x => x.AppUsers)
                .WithMany(y => y.Articles)
                .UsingEntity(d => d.ToTable("ArticleAuthor"));
            builder.Entity<Article>()
                .HasMany(o => o.Categories)
                .WithMany(u => u.Articles)
                .UsingEntity(b => b.ToTable("ArticleCategory"));
        }
    }
}
