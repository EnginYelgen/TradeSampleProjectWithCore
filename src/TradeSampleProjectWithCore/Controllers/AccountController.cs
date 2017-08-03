﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using TradeSampleProjectWithCore.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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
        public IActionResult Detail(string returnUrl = "/")
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(new DataService.Account(this.DbContext).GetAccountDetail(this.LogInUserId));
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ViewModelAccountDetail model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                User loginUser = this.DbContext.Users.Where(x => x.Id == this.LogInUserId).Single();

                string hashedPassOld = Common.Security.GetHashed(model.ChangePassword.CurrentPassword);

                if (loginUser.Password != hashedPassOld)
                {
                    ViewData["Message"] = "Şifre yanlış";
                }
                else
                {
                    string hashedPass = Common.Security.GetHashed(model.ChangePassword.NewPassword);

                    loginUser.Password = hashedPass;

                    this.DbContext.SaveChanges();

                    await HttpContext.Authentication.SignOutAsync("CookieAuthentication");

                    return RedirectToAction("Login");
                }
            }

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult SaveAddress(ViewModelAccountDetail model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                this.DbContext.Addresses.Add(new Address
                {
                    CityId = model.NewAddress.CityId,
                    CountryId = model.NewAddress.CityId,
                    Description = model.NewAddress.Description,
                    InUse = model.NewAddress.InUse,
                    No = model.NewAddress.Number,
                    Street = model.NewAddress.Street,
                    PostCode = model.NewAddress.PostCode,
                    UpdateDate = DateTime.Now,
                    UpdateUserId = this.LogInUserId,
                    UserId = this.LogInUserId
                });

                this.DbContext.SaveChanges();
            }

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeActiveAddress(ViewModelAccountDetail model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {

            }

            return View(model);
        }
    }
}
