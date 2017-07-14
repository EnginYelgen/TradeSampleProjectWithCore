using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TradeSampleProjectWithCore.Models;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TradeSampleProjectWithCore.Controllers
{
    public class RegisterController : Controller
    {
        private readonly TradeSampleContext appContext;

        public RegisterController(TradeSampleContext context)
        {
            this.appContext = context;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Save(Register model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (model.Birthday > DateTime.Now.AddYears(-12))
            {
                ModelState.AddModelError("Birthday", "12 yaşından küçükler üye olamaz");
            }

            if (ModelState.IsValid)
            {
                if (appContext.Users.Any(x => x.MailAddress == model.Email))
                {
                    ViewData["Message"] = "Bu mail adresi kullanılıyor";
                }
                else
                {
                    string hashedPass = Common.Security.GetHashed(model.Password);

                    appContext.Users.Add(new Models.User
                    {
                        Name = model.Name,
                        Surname = model.Surname,
                        MailAddress = model.Email,
                        Password = hashedPass,
                        Birthday = model.Birthday,
                        Telephone = model.Telephone,
                        InUse = true,
                        UpdateDate = DateTime.Now
                    });

                    try
                    {
                        appContext.SaveChanges();

                        return RedirectToAction("Login", "Account");
                    }
                    catch (Exception ex)
                    {
                        ViewData["Message"] = "İşlem sırasında hata oluştu";
                    }
                }
            }

            return View("Index");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Cancel()
        {
            return RedirectToAction("Login", "Account");
        }
    }
}
