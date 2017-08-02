using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TradeSampleProjectWithCore.Models;
using Microsoft.AspNetCore.Mvc.Filters;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TradeSampleProjectWithCore.BaseClasses
{
    public class BaseController : Controller
    {
        public readonly TradeSampleContext DbContext;

        public string LogInMailAddress
        {
            get { return ViewData["LogInMailAddress"].ToString(); }
            set { ViewData["LogInMailAddress"] = value; }
        }
        public string LogInUserName
        {
            get { return ViewData["LogInUserName"].ToString(); }
            set { ViewData["LogInUserName"] = value; }
        }
        public string LogInUserId
        {
            get { return ViewData["LogInUserId"].ToString(); }
            set { ViewData["LogInUserId"] = value; }
        }
        public int ShoppingItemCount
        {
            get { return Convert.ToInt32(ViewData["ShoppingItemCount"]); }
            set { ViewData["ShoppingItemCount"] = value; }
        }

        public BaseController(TradeSampleContext context)
        {
            this.DbContext = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            ReadCookie();

            if (!string.IsNullOrEmpty(LogInUserId))
            {
                ShoppingItemCount = new DataService.Order(this.DbContext).GetCartItemCount(Convert.ToInt32(LogInUserId));
            }
        }

        private void ReadCookie()
        {
            LogInMailAddress = LogInUserName = LogInUserId = string.Empty;
            ShoppingItemCount = 0;

            ClaimsPrincipal loggedInUser = HttpContext.User;
            List<Claim> listClaim = loggedInUser.Claims.ToList();

            if (listClaim.Count > 0)
            {
                LogInMailAddress = listClaim[0].Value; // mail address
                LogInUserName = listClaim[2].Value; // user name
                LogInUserId = listClaim[3].Value; // user id
            }
        }
    }
}
