using Humanizer.Localisation;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NewsSite.Models;
using System.Reflection.Emit;

namespace NewsSite.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options){ }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ArticleCategory> ArticleCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Article>()
                .HasOne(b => b.User)
                .WithMany(a => a.Articles)
                .HasForeignKey(b => b.UserId);

            modelBuilder.Entity<ArticleCategory>()
                .HasKey(bt => new { bt.ArticleId, bt.CategoryId });

            modelBuilder.Entity<ArticleCategory>()
                .HasOne(bt => bt.Article)
                .WithMany(b => b.ArticleCategories)
                .HasForeignKey(bt => bt.ArticleId);

            modelBuilder.Entity<ArticleCategory>()
                .HasOne(bt => bt.Category)
                .WithMany(t => t.ArticleCategories)
                .HasForeignKey(bt => bt.CategoryId);
        }
    }
}
