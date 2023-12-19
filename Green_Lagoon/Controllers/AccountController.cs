using Green_Lagoon.Application.Common.Interface;
using Green_Lagoon.Application.Common.Utility;
using Green_Lagoon.Domain.Entities;
using Green_Lagoon.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Green_Lagoon.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        [HttpGet]
        public IActionResult Login( string returnUrl=null)
        {
            returnUrl ??= Url.Content("~/");
            LoginViewModel loginViewModel = new()
            {
                RedirectUrl = returnUrl,
            };
            return View(loginViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewM)
        {
            if(ModelState.IsValid)
            {
                var result = await _signInManager.
                     PasswordSignInAsync(loginViewM.Email, loginViewM.Password, loginViewM.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(loginViewM.Email);
                    if(await _userManager.IsInRoleAsync(user,SD.Role_Admin))
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(loginViewM.RedirectUrl))
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            return LocalRedirect(loginViewM.RedirectUrl);
                        }
                    }
                   

                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attept.");
                }
            }

            return View(loginViewM);
        }
        public IActionResult Register(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if (!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).Wait();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).Wait();
            }
            RegisterViewModel viewModel = new()
            {
                Roles = _roleManager.Roles.Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                { 
                    Text = x.Name,
                    Value = x.Name
                
                }),
                RedirectUrl= returnUrl
                
            };
           
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewM)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new()
                {
                    Name = registerViewM.Name,
                    Email = registerViewM.Email,
                    PhoneNumber = registerViewM.PhoneNumber,
                    NormalizedEmail = registerViewM.Email.ToUpper(),
                    EmailConfirmed = true,
                    UserName = registerViewM.Email,
                    CreatedAt = DateTime.Now
                };
                var result = await _userManager.CreateAsync(user, registerViewM.Password);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(registerViewM.Role))
                    {
                        await _userManager.AddToRoleAsync(user, registerViewM.Role);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, SD.Role_Customer);
                    }
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    if (string.IsNullOrEmpty(registerViewM.RedirectUrl))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return LocalRedirect(registerViewM.RedirectUrl);
                    }

                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
           
             registerViewM.Roles =

                 _roleManager.Roles.Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Text = x.Name,
                    Value = x.Name

                });
            

            return View(registerViewM);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }
        public IActionResult AccessDenied()
        {

            return View();
        }
    }
}
