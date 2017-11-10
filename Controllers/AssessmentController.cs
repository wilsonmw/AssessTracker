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
    public class AssessmentController : Controller
    {
        private TrackerContext _context;
        public AssessmentController(TrackerContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("addAssessmentPage")]
        public IActionResult addAssessmentPage(){
            if(HttpContext.Session.GetInt32("Permission") != 9){
                return RedirectToAction("dashboard", "Dashboard");
            }
            return View("addAssessmentPage");
        }

        [HttpPost]
        [Route("addAssessment")]
        public IActionResult addAssessment(AssessmentViewModel model){
            if(!ModelState.IsValid){
                ViewBag.addAssessmentError = "Please fill out all fields.";
                return View("addAssessmentPage", model);
            }
            Assessment newAssessment = new Assessment{
                Name = model.Name,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now

            };
            _context.Assessments.Add(newAssessment);
            _context.SaveChanges();
            return RedirectToAction("dashboard", "Dashboard");
        }
    }
}