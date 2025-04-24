using Azure.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public AccountController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
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
            var users = _userManager.Users.ToList();
            var userRoles = _dbContext.UserRoles.ToList();
            var roles = _roleManager.Roles.ToList();

            List<UsersInRoleViewModel> accounts = new List<UsersInRoleViewModel>();
            foreach (var user in users)
            {
                var model = new UsersInRoleViewModel
                {
                    UserId = user.Id,
                    FirstName = user.FirstName,
                    FatherName = user.FatherName,
                    FamilyName = user.FamilyName,
                    Email = user.Email,
                    Username=user.CustomUsername
                };
                foreach (var role in roles)
                {
                    foreach (var userRole in userRoles)
                    {
                        if (user.Id == userRole.UserId && role.Id == userRole.RoleId)
                        {
                            model.Role = role.Name;
                        }
                    }
                }
                accounts.Add(model);
            }

            return View(accounts);
        }
        public IActionResult Create()
        {
            ViewData["RolesId"] = new SelectList(_dbContext.Roles, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAccountViewModel model, string SelRole)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    CustomUsername=model.CustomUsername,
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    FatherName = model.FatherName,
                    FamilyName = model.FamilyName,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var role = await _roleManager.FindByIdAsync(SelRole);
                    await _userManager.AddToRoleAsync(user, role.Name.ToString());

                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, "Wrong email or password");
                } 
            }
            ViewData["RolesId"] = new SelectList(_dbContext.Roles, "Id", "Name", SelRole);
            return View();
        }
        public ActionResult Edit(int id) 
        {
            return View();
        }

    }
}
