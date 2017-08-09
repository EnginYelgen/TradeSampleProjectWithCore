using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using TradeSampleProjectWithCore.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using TradeSampleProjectWithCore.Common;

namespace TradeSampleProjectWithCore.Controllers
{
    public class AccountController : BaseClasses.BaseController
    {
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

        public AccountController(TradeSampleContext context) : base(context) { }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Register()
        {
            return RedirectToAction("Index", "Register");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Unauthorized(string returnUrl = "/")
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Forbidden(string returnUrl = "/")
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = "/")
        {
            ViewData["ReturnUrl"] = returnUrl;

            ClaimsPrincipal loggedInUser = HttpContext.User;
            //var loggedInUserName = loggedInUser.Identity.Name;
            List<Claim> listClaim = loggedInUser.Claims.ToList();

            if (listClaim.Count > 0)
            {
                if (this.DbContext.Users.Any(x => x.MailAddress == listClaim[0].Value))
                {
                    User loginUser = this.DbContext.Users.Where(x => x.MailAddress == listClaim[0].Value).Single();

                    if (loginUser.Password == listClaim[1].Value)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(ViewModelLogin model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                string hashedPass = Common.Security.GetHashed(model.Password);

                if (this.DbContext.Users.Any(x => x.MailAddress == model.Email))
                {
                    User loginUser = this.DbContext.Users.Where(x => x.MailAddress == model.Email).Single();

                    if (loginUser.Password == hashedPass)
                    {
                        List<Claim> claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Authentication, model.Email),
                            new Claim(ClaimTypes.Authentication, hashedPass),
                            new Claim(ClaimTypes.Authentication, loginUser.Name + " " + loginUser.Surname),
                            new Claim(ClaimTypes.Authentication, loginUser.Id.ToString())
                        };

                        ClaimsIdentity userIdentity = new ClaimsIdentity(claims, "login");

                        ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                        await HttpContext.Authentication.SignInAsync("CookieAuthentication", principal);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewData["Message"] = "Şifre yanlış";

                        //return RedirectToAction("Unauthorized", "Account");
                    }
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

            await HttpContext.Authentication.SignOutAsync("CookieAuthentication");

            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Detail(string message, string messageKey)
        {
            if (!string.IsNullOrEmpty(messageKey))
            {
                ViewData[messageKey] = message;
            }

            SelectList list = new DataService.DropdownList(this.DbContext).GetCountryList(hasEmptyItem: true);
            ViewBag.CountryList = list;
            list = new DataService.DropdownList(this.DbContext).GetCityList(0, hasEmptyItem: true);
            ViewBag.CityList = list;

            return View(new DataService.Account(this.DbContext).GetAccountDetail(this.LogInUserId));
        }

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ChangePassword(ViewModelChangePassword model, string returnUrl = null)
        //{
        //    ViewData["ReturnUrl"] = returnUrl;

        //    if (ModelState.IsValid)
        //    {
        //        User loginUser = this.DbContext.Users.Where(x => x.Id == this.LogInUserId).Single();

        //        string hashedPassOld = Common.Security.GetHashed(model.CurrentPassword);

        //        if (loginUser.Password != hashedPassOld)
        //        {
        //            ViewData["Message"] = "Şifre yanlış";
        //        }
        //        else
        //        {
        //            string hashedPass = Common.Security.GetHashed(model.NewPassword);

        //            loginUser.Password = hashedPass;

        //            this.DbContext.SaveChanges();

        //            await HttpContext.Authentication.SignOutAsync("CookieAuthentication");

        //            return RedirectToAction("Login");
        //        }
        //    }

        //    //return View(model);
        //    //return View("Detail", model);
        //    return RedirectToAction("Detail", new { hasModelChangePassword = true });
        //}

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ViewModelChangePassword model)
        {
            string errMess = string.Empty;
            string errMessKey = "Message_ViewModelChangePassword";

            if (ModelState.IsValid)
            {
                User loginUser = this.DbContext.Users.Where(x => x.Id == this.LogInUserId).Single();

                string hashedPassOld = Common.Security.GetHashed(model.CurrentPassword);

                if (loginUser.Password != hashedPassOld)
                {
                    errMess = "Şifre yanlış";
                }
                else
                {
                    string hashedPass = Common.Security.GetHashed(model.NewPassword);

                    loginUser.Password = hashedPass;

                    this.DbContext.SaveChanges();

                    await HttpContext.Authentication.SignOutAsync("CookieAuthentication");

                    //return Json("OK");

                    return RedirectToAction("Login");
                }
            }

            //return View(model);
            //return View("Detail", model);
            //return RedirectToAction("Detail", new { hasModelChangePassword = true });
            //return Json("ERROR");
            return RedirectToAction("Detail", new { message = errMess, messageKey = errMessKey });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult SaveAddress(ViewModelAddress model)
        {
            string errMess = string.Empty;
            string errMessKey = "Message_ViewModelAddress";

            if (ModelState.IsValid)
            {
                new DataService.Account(this.DbContext).SaveAddress(new Address
                {
                    AddressName = model.AddressName,
                    CityId = model.CityId,
                    CountryId = model.CountryId,
                    Description = model.Description,
                    InUse = model.InUse,
                    No = model.Number,
                    Street = model.Street,
                    PostCode = model.PostCode,
                    UpdateDate = DateTime.Now,
                    UpdateUserId = this.LogInUserId,
                    UserId = this.LogInUserId
                },
                out errMess);
            }

            return RedirectToAction("Detail", new { message = errMess, messageKey = errMessKey });
        }

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public IActionResult ChangeActiveAddress(IEnumerable<ViewModelAddress> modelList)
        //{
        //    string errMess = string.Empty;
        //    string errMessKey = "Message_ViewModelAddressList";

        //    if (ModelState.IsValid)
        //    {
        //        new DataService.Account(this.DbContext).SetActiveAddress(
        //            modelList.Where(x => x.InUse).Single().AddressId,
        //            this.LogInUserId,
        //            out errMess);
        //    }

        //    return RedirectToAction("Detail", new { message = errMess, messageKey = errMessKey });
        //}

        [HttpPost]
        public JsonResult ChangeActiveAddress(List<DataPair> data)
        {
            string errMess = string.Empty;
            //string errMessKey = "Message_ViewModelAddressList";

            if (ModelState.IsValid)
            {
                new DataService.Account(this.DbContext).SetActiveAddress(
                    Convert.ToInt32(data.Where(x => Convert.ToBoolean(x.Value)).Single().Key),
                    this.LogInUserId,
                    out errMess);
            }

            return Json(new { msgCode = string.IsNullOrEmpty(errMess) ? "OK" : "ERROR", msgDetail = string.IsNullOrEmpty(errMess) ? "İşlem başarıyla gerçekleşti." : errMess });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteAddress(int? addressId)
        {
            string errMess = string.Empty;
            string errMessKey = "Message_ViewModelAddressList";

            if (addressId > 0)
            {
                new DataService.Account(this.DbContext).DeleteAddress(
                    Convert.ToInt32(addressId),
                    this.LogInUserId,
                    out errMess);
            }

            return RedirectToAction("Detail", new { message = errMess, messageKey = errMessKey });
        }

        public SelectList GetCity(int countryId)
        {
            return new DataService.DropdownList(this.DbContext).GetCityList(countryId, hasEmptyItem: true);
        }
    }
}
