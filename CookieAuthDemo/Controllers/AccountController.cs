using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CookieAuthDemo.Models;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CookieAuthDemo.Controllers
{
    public class AccountController : Controller
    {
        //private readonly UserManager<ApplicationUser> _userManager;
        //private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly TestUserStore _testUserStore;

        public AccountController(TestUserStore testUserStore)
        {
            this._testUserStore = testUserStore;
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        //public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        //{
        //    _userManager = userManager;
        //    _signInManager = signInManager;
        //}

        [HttpGet]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model, string returnUrl = null)
        {
            //if (ModelState.IsValid)
            //{
            //    var user = new ApplicationUser()
            //    {
            //        Email = model.Email,
            //        UserName = model.Email,
            //        NormalizedEmail = model.Email
            //    };

            //    var result = await _userManager.CreateAsync(user, model.Password);

            //    if (result.Succeeded)
            //    {
            //        await _signInManager.SignInAsync(user, true);

            //        //return RedirectToAction("Index", "Home");
            //        return RedirectToLocal(returnUrl);
            //    }
            //    else
            //    {
            //        result.Errors.ToList().ForEach(x => ModelState.AddModelError(string.Empty, x.Description));
            //    }
            //}


            return View();
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {

                var user = _testUserStore.FindByUsername(model.UserName);


                //var user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Sorry, Can not find the user by this username !");
                }
                else
                {
                    var result = _testUserStore.ValidateCredentials(model.UserName, model.Password);

                    //var result = await _userManager.CheckPasswordAsync(user, model.Password);
                    if (result)
                    {

                        //await _signInManager.SignInAsync(user, new AuthenticationProperties() { IsPersistent = true });

                        var prop = new AuthenticationProperties()
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTimeOffset.UtcNow.Add(TimeSpan.FromMinutes(30))
                        };

                        await HttpContext.SignInAsync(user.SubjectId, user.Username, prop);

                        //return RedirectToAction("Index", "Home");
                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "The password is error !");
                    }

                }

            }

            return View();
        }

        // GET: /<controller>/
        public IActionResult MakeLogin()
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,"djlnet"),
                new Claim(ClaimTypes.Role,"admin")
            };

            ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            return Ok();
        }

        public async Task<IActionResult> Logout()
        {
            //await _signInManager.SignOutAsync();
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
