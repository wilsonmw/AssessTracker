using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using AssessTracker.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace AssessTracker.Controllers
{
    public class HomeController : Controller
    {
 
        private TrackerContext _context;
        public HomeController(TrackerContext context)
        {
            _context = context;
        }
        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View("index");
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(string email, string password){
            User currentUser = _context.Users.SingleOrDefault(user => user.Email == email);
            if(currentUser != null){
                if(currentUser.Password == password){
                    HttpContext.Session.SetString("FirstName", currentUser.FirstName);
                    HttpContext.Session.SetString("LastName", currentUser.LastName);
                    HttpContext.Session.SetInt32("Permission", currentUser.Permission);
                    return RedirectToAction("dashboard", "Dashboard");
                }
            }
            ViewBag.LoginError = "Login Failed, Please Try Again";
            return View("index");
        }

    }
}
