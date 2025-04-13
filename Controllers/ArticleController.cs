using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewsSite.Data;
using NewsSite.Models;
using NewsSite.Models.Articles;

namespace NewsSite.Controllers
{
    [Authorize(Roles ="Admin,Worker")]
    public class ArticleController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public ArticleController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ActionResult Index()
        {
            var articles = _dbContext.Articles.Include(s => s.ArticleCategories).ToList();
            foreach(var article in articles)
            {
                if (article.ArticleCategories.Any())
                {
                    foreach (var category in article.ArticleCategories)
                    {
                        return View(articles);
                    }
                }
            }
            return View();
            //https://www.youtube.com/watch?v=6YIRKBsRWVI
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
            var users = _dbContext.Users;
            List<string> usersList = new List<string>();
            foreach(var user in users)
            {
                usersList.Add(user.FirstName.ToString() + " " + user.FamilyName.ToString());
            }
            ViewData["UserId"] = new SelectList(usersList);
            return View();
        }

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
            ViewData["UserId"] = new SelectList(_dbContext.Users, "UserId", "FirstName");
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
