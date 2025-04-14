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
        public async Task<IActionResult> Index()
        {
            var articles = await _dbContext.Articles.Include(s => s.ArticleCategories).ThenInclude(sp => sp.Category).ToListAsync();
            return View(articles);
            //https://stackoverflow.com/questions/74841868/many-to-many-crud-operations-in-asp-net-core <3
        }
        public ActionResult Details(int id)
        {
            return View();
        }
        public ActionResult Create()
        {
            /*IEnumerable<ApplicationUser> SuppliesList = _dbContext.Users.Include(s => s.FirstName);
            ViewBag.Supplies = SuppliesList;
            return View();
            just checking something out*/
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
