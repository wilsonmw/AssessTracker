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
    public class UserController : Controller
    {
        private TrackerContext _context;
        public UserController(TrackerContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("grantAccessPage")]
        public IActionResult grantAccessPage(){
            if(HttpContext.Session.GetInt32("Permission") < 9){
                return RedirectToAction("dashboard", "Dashboard");
            }
            if(HttpContext.Session.GetInt32("Permission") == null){
                return RedirectToAction("Index", "Home");
            }
            List<User> allUsers = _context.Users.OrderBy(x => x.LastName).ToList();
            ViewBag.allUsers = allUsers;
            return View("grantAccessPage");
        }

        [HttpPost]
        [Route("getUser")]
        public IActionResult getUser(int id){
            User accessUser = _context.Users.SingleOrDefault(x => x.id == id);
            ViewBag.user = accessUser;
            return View("grantUserPerm");
        }

        [HttpPost]
        [Route("givePerm")]
        public IActionResult givePerm(int id, int perm){
            User accessUser = _context.Users.SingleOrDefault(x => x.id == id);
            accessUser.Permission = perm;
            _context.SaveChanges();
            return RedirectToAction("grantAccessPage");
        }

        [HttpGet]
        [Route("deleteUserPage/{id}")]
        public IActionResult deleteUserPage(int id){
            if(HttpContext.Session.GetInt32("Permission") < 9){
                return RedirectToAction("dashboard", "Dashboard");
            }
            if(HttpContext.Session.GetInt32("Permission") == null){
                return RedirectToAction("Index", "Home");
            }
            User accessUser = _context.Users.SingleOrDefault(x => x.id == id);
            ViewBag.user = accessUser;
            return View("deleteConfirm");
        }

        [HttpGet]
        [Route("deleteUser/{id}")]
        public IActionResult deleteUser(int id){
            if(HttpContext.Session.GetInt32("Permission") < 9){
                return RedirectToAction("dashboard", "Dashboard");
            }
            if(HttpContext.Session.GetInt32("Permission") == null){
                return RedirectToAction("Index", "Home");
            }
            User toDelete = _context.Users.SingleOrDefault(x => x.id == id);
            _context.Users.Remove(toDelete);
            _context.SaveChanges();
            return RedirectToAction("grantAccessPage");
        }
    }
}