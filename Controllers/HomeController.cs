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
            return View("Index");
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(string email, string password){
            User currentUser = _context.Users.SingleOrDefault(user => user.Email == email);
            if(currentUser != null){
                if(currentUser.Permission == 0){
                    ViewBag.name = currentUser.FirstName;
                    return RedirectToAction("newUserPage");
                }
                if(currentUser.Password == password){
                    HttpContext.Session.SetString("FirstName", currentUser.FirstName);
                    HttpContext.Session.SetString("LastName", currentUser.LastName);
                    HttpContext.Session.SetInt32("Permission", currentUser.Permission);
                    return RedirectToAction("dashboard", "Dashboard");
                }
            }
            ViewBag.LoginError = "Login Failed, Please Try Again";
            return View("Index");
        }

        [HttpPost]
        [Route("register")]
        public IActionResult register(RegisterViewModel model){
            if(!ModelState.IsValid){
                return View("Index");
            }
            List<User> exists = _context.Users.Where(x => x.Email == model.Email).ToList();
            Console.WriteLine("*********************************");
            Console.WriteLine(exists.Count);
            if(exists.Count > 0){
                ViewBag.registerError = "That email address is already in use. Please try again.";
                return View("Index");
            }
            User newUser = new User{
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Organization = model.Organization,
                Password = model.Password,
                Permission = 0
            };
            _context.Users.Add(newUser);
            _context.SaveChanges();
            return RedirectToAction("newUserPage");
        }

        [HttpGet]
        [Route("newUserPage")]
        public IActionResult newUserPage(){
            return View("newUserPage");
        }

    }
}
