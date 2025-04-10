using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsSite.Data;
using NewsSite.Models;
using System;
using System.Diagnostics;

namespace NewsSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context,ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }
        [HttpGet]
        public IActionResult ChangeThemeRf()
        {
            var theme = Request.Cookies["UserPreference"] ?? "light";
            ViewBag.Theme = theme;
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult ChangeTheme(string theme)
        {
            Response.Cookies.Append("UserPreference", theme, new CookieOptions
            {
                Expires = DateTime.UtcNow.AddYears(1),
                HttpOnly = true,
                IsEssential = true
            });
            return RedirectToAction("ChangeThemeRf");
        }
        public IActionResult Index(string chosenOne)
        {
            var articles = _context.Articles.ToList();
            List<Article> chosenOnes = new List<Article>();
            string title;

            if (chosenOne == null) 
            {
                return View(articles);
            }
            if (chosenOne != null)
            {
                foreach (var article in articles)
                {
                    var articleCategories = _context.ArticleCategories.Where(n => n.ArticleId == article.ArticleId).ToList();
                    foreach (var articleCategory in articleCategories)
                    {
                        switch (chosenOne)
                        {
                            case "Bulgaria":
                            {
                                    title = "У нас";
                                    if (article.ArticleId == articleCategory.ArticleId && articleCategory.Category.CategoryName == chosenOne)
                                    {
                                        chosenOnes.Add(article);
                                    }
                                    ViewData["PageTitle"] = title;
                                    break;
                            }
                            case "World":
                                {
                                    title = "По светът";
                                    if (article.ArticleId == articleCategory.ArticleId && articleCategory.Category.CategoryName == chosenOne)
                                    {
                                        chosenOnes.Add(article);
                                    }
                                    ViewData["PageTitle"] = title;
                                }
                                break;
                            case "Politics":
                                {
                                    title = "Политика";
                                    if (article.ArticleId == articleCategory.ArticleId && articleCategory.Category.CategoryName == chosenOne)
                                    {
                                        chosenOnes.Add(article);
                                    }
                                    ViewData["PageTitle"] = title;
                                }
                                break;
                            case "Economy":
                                {
                                    title = "Икономика";
                                    if (article.ArticleId == articleCategory.ArticleId && articleCategory.Category.CategoryName == chosenOne)
                                    {
                                        chosenOnes.Add(article);
                                    }
                                    ViewData["PageTitle"] = title;
                                }
                                break;
                            case "Sports":
                                {
                                    title = "Спорт";
                                    if (article.ArticleId == articleCategory.ArticleId && articleCategory.Category.CategoryName == chosenOne)
                                    {
                                        chosenOnes.Add(article);
                                    }
                                    ViewData["PageTitle"] = title;
                                }
                                break;
                            default:
                                {
                                    title = "Новините днес";
                                    ViewData["PageTitle"] = title;
                                    return View(articles);
                                    
                                }
                        }
                    }
                    
                }
                return View(chosenOnes);
            }


            return View(articles);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
