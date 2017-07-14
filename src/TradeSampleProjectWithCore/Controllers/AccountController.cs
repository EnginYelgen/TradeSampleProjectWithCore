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
                        return RedirectToAction("Index", "Home");
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
            return RedirectToAction("Login", "Account");
        }
    }
}
