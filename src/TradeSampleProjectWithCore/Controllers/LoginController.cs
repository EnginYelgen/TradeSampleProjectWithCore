using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TradeSampleProjectWithCore.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TradeSampleProjectWithCore.Controllers
{
    public class LoginController : Controller
    {
        private readonly TradeSampleContext _context;

        public LoginController(TradeSampleContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
    }
}
