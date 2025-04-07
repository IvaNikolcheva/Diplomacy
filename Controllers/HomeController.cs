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

        public IActionResult Index(string chosenOne)
        {
            var articles = _context.Articles.ToList();
            List<Article> chosenOnes = new List<Article>();

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
                            case "България":
                            {
                                    if (article.ArticleId == articleCategory.ArticleId && articleCategory.Category.CategoryName == chosenOne)
                                    {
                                        chosenOnes.Add(article);
                                    }
                                break;
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
        public IActionResult Bulgaria()
        {
            var articles = _context.Articles;
            return View(articles);
        }
        public IActionResult World()
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
