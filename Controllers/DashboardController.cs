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
    public class DashboardController : Controller
    {
        private TrackerContext _context;
        public DashboardController(TrackerContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("dashboard")]
        public IActionResult dashboard(){
            if(HttpContext.Session.GetInt32("Permission") == null){
                return RedirectToAction("Index", "Home");
            }
            ViewBag.permission = HttpContext.Session.GetInt32("Permission");
            ViewBag.firstName = HttpContext.Session.GetString("FirstName");
            return View("dashboard");
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult logout(){
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}