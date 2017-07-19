using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using TradeSampleProjectWithCore.Models;
using Microsoft.AspNetCore.Authorization;

namespace TradeSampleProjectWithCore.Controllers
{
    public class AccountController : Controller
    {
        private readonly TradeSampleContext _context;
        //private readonly UserManager<User> _userManager;
        //private readonly SignInManager<User> _signInManager;

        //public AccountController(
        //    TradeSampleContext context,
        //    UserManager<User> userManager,
        //    SignInManager<User> signInManager)
        //{
        //    this._context = context;
        //    this._userManager = userManager;
        //    this._signInManager = signInManager;
        //}
        public AccountController(TradeSampleContext context)
        {
            this._context = context;
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Register()
        {
            return RedirectToAction("Index", "Register");
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
        public async Task<IActionResult> Login(Login model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                string hashedPass = Common.Security.GetHashed(model.Password);

                if (_context.Users.Any(x => x.MailAddress == model.Email))
                {
                    User loginUser = _context.Users.Where(x => x.MailAddress == model.Email).Single();

                    if (loginUser.Password == hashedPass)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                        ViewData["Message"] = "Şifre yanlış";
                }
                else
                    ViewData["Message"] = "E-posta adresi bulunamadı";

                //Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

                //if (result.Succeeded)
                //{
                //    return RedirectToAction("Index", "Home");
                //}
                //else
                //{
                //    ViewData["Message"] = "Email veya şifre yanlış";
                //}
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            //await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
