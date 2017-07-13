using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using TradeSampleProjectWithCore.Models;
using Microsoft.AspNetCore.Authorization;


// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TradeSampleProjectWithCore.Controllers
{
    public class AccountController : Controller
    {
        private readonly TradeSampleContext appContext;

        public AccountController(TradeSampleContext context)
        {
            this.appContext = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Register model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };
                //var result = await _securityManager.CreateAsync(user, model.Password);
                //if (result.Succeeded)
                //{
                //    await _loginManager.SignInAsync(user, isPersistent: false);
                //    return RedirectToAction(nameof(HomeController.Index), "Home");
                //}
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = "/")
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Login(Login model, string returnUrl = null)
        public IActionResult Login(Login model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                string hashedPass = Common.Security.GetHashed(model.Password);

                if (appContext.Users.Any(x => x.MailAddress == model.Email))
                {
                    string userPass = appContext.Users.Where(x => x.MailAddress == model.Email).Single().Password;

                    if (userPass == hashedPass)
                        return RedirectToReturnUrl("Home");
                    else
                        ViewData["Message"] = "Şifre yanlış";
                }
                else
                    ViewData["Message"] = "E-posta adresi bulunamadı";
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            //await _loginManager.SignOutAsync();

            return RedirectToAction("Login", nameof(AccountController));
        }

        private IActionResult RedirectToReturnUrl(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Login", nameof(AccountController));
            }
        }
    }
}
