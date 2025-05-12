using Humanizer.Localisation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public async Task<IActionResult> Index( string searchString, string searchValue)
        {
            if (_context.Articles == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Article' is null.");
            }

            var articles = from m in _context.Articles.Include(m => m.Category).AsQueryable()
                           select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                articles = articles.Where(s => s.Title!.ToUpper().Contains(searchString.ToUpper()));
            }
            if (!String.IsNullOrEmpty(searchValue))
            {
                articles = articles.Where(s => s.Category.CategoryName!.ToUpper().Contains(searchValue.ToUpper()));
            }

            await articles.ToListAsync();
            articles.Reverse();
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
