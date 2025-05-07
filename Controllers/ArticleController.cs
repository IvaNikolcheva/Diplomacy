using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NewsSite.Data;
using NewsSite.Models;
using NewsSite.Models.Articles;
using System.Linq;

namespace NewsSite.Controllers
{
    public class ArticleController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        public ArticleController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }
        [Authorize(Roles = "Admin,Worker")]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var role = await _userManager.GetRolesAsync(user);

            List<Article> userArticles = new List<Article>();
            var articles = await _dbContext.Articles
                .Include(b => b.User)
                .Include(b => b.Category)
                .ToListAsync();

            if (role.Contains("Admin"))
            { 
                return View(articles);
            }
            else if(role.Contains("Worker"))
            {
                foreach (var article in articles)
                {
                    if (article.UserId == _userManager.GetUserId(User))
                    {
                        userArticles.Add(article);
                    }
                }
                return View(userArticles);
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Details(int id)
        {
            var article = _dbContext.Articles.Include(b => b.User)
                .Include(b => b.Category).FirstOrDefault(x => x.ArticleId == id);
            return View(article);
        }
        [Authorize(Roles = "Admin,Worker")]
        public ActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_dbContext.Categories, "CategoryId", "CategoryName");
            ViewData["UserId"] = new SelectList(_userManager.Users, "Id", "UserName");
            return View();
        }
        [Authorize(Roles = "Admin,Worker")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateArticleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var article = new Article
                {
                    Title = model.Title,
                    UserId = model.UserId,
                    Content = model.Content,
                    CategoryId = model.CategoryId,
                    PublishedDate = DateTime.Now,
                };
                if (model.ImageFile != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        await model.ImageFile.CopyToAsync(ms);
                        article.Image = ms.ToArray();
                    }
                }
                _dbContext.Articles.Add(article);
                await _dbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_dbContext.Categories, "Id", "CategoryName", model.CategoryId);
            ViewData["UserId"] = new SelectList(_userManager.Users, "Id", "UserName", model.UserId);
            return View(model);
        }

        [Authorize(Roles = "Admin,Worker")]
        public async Task<IActionResult> Edit(int id)
        {
            var article = _dbContext.Articles.Include(b => b.User)
                .Include(b => b.Category).FirstOrDefault(x => x.ArticleId == id);
            if (article == null)
            {
                return NotFound(0);
            }
            var model = new EditArticleViewModel()
            {
                Id=article.ArticleId,
                Title = article.Title,
                UserId = article.UserId,
                Content = article.Content,
                CategoryId = article.CategoryId,
                ExistingImage=article.Image,
                PublishedDate=article.PublishedDate
            };
            ViewData["CategoryId"] = new SelectList(_dbContext.Categories, "Id", "CategoryName", model.CategoryId);
            ViewData["UserId"] = new SelectList(_userManager.Users, "Id", "UserName", model.UserId);
            return View(model);
        }

        [Authorize(Roles = "Admin,Worker")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditArticleViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var article = _dbContext.Articles.Find(id);

                article.Title = model.Title;
                article.Content = model.Content;
                article.UserId = model.UserId;
                article.CategoryId = model.CategoryId;
                article.PublishedDate = model.PublishedDate;

                if (model.ImageFile != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        await model.ImageFile.CopyToAsync(ms);
                        article.Image = ms.ToArray();
                    }
                }

                _dbContext.Articles.Update(article);
                await _dbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        [Authorize(Roles = "Admin,Worker")]
        public ActionResult Delete(int id)
        {
            var article = _dbContext.Articles.Include(b => b.User).Include(c => c.Category)
                .FirstOrDefault(x => x.ArticleId == id);

            if (article == null) return NotFound();

            var model = new DeleteArticleViewModel
            {
                Id = article.ArticleId,
                Title = article.Title,
                Image=article.Image,
                Content=article.Content,
                PublishedDate=article.PublishedDate,
                Category=article.Category.CategoryName,
                User=article.User.FirstName + " " + article.User.FamilyName
            };
            return View(model);
        }
        [Authorize(Roles = "Admin,Worker")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, DeleteArticleViewModel model)
        {
            var article = _dbContext.Articles.Find(id);
            if (article == null) return NotFound();

            _dbContext.Articles.Remove(article);
            _dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
