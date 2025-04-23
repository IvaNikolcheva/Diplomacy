using Azure.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsSite.Data;
using NewsSite.Models;
using NewsSite.Models.Account;

namespace NewsSite.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Wrong email or password");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    Email = model.Email,
                    FirstName = model.FirstName,
                    FatherName = model.FatherName,
                    FamilyName = model.FamilyName,
                    UserName = model.Email,
                    CustomUsername = model.CustomUsername,
                    EmailConfirmed = true
                };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Member");
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, "Wrong email or password");
                }
            }
            return View();
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Index()
        {
            var usersWithRoles=(from user in _dbContext.Users
                                select new
                                {
                                    UserId = user.Id,
                                    Email = user.Email,
                                    FirstName = user.FirstName,
                                    FatherName = user.FatherName,
                                    FamilyName = user.FamilyName,
                                    UserName = user.CustomUsername,
                                    RoleNames = (from userRole in user.Roles
                                                 join role
                                                 in _dbContext.Roles
                                                 on userRole.RoleId
                                                 equals role.Id
                                                 select role.Name).ToList()
                                }).ToList().Select(p => new UsersInRoleViewModel()
                                {
                                    UserId = p.UserId,
                                    Username = p.Username,
                                    Email = p.Email,
                                    Role = string.Join(",", p.RoleNames)
                                });
            return View(usersWithRoles);
            /*List<ApplicationUser> applicationUsers = new List<ApplicationUser>();
            var users=_userManager.Users.ToList();
            foreach (var user in users)
            {
                applicationUsers.Add(user);
            }
            return View(applicationUsers);*/
        }

        public ActionResult Edit(int id) 
        {
            
            return View();
        }
    }
}
