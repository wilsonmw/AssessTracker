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
            ViewBag.permission = HttpContext.Session.GetInt32("Permission");
            ViewBag.firstName = HttpContext.Session.GetString("FirstName");
            return View("dashboard");
        }
    }
}