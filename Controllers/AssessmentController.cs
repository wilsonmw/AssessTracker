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
            if(HttpContext.Session.GetInt32("Permission") < 5){
                return RedirectToAction("dashboard", "Dashboard");
            }
            if(HttpContext.Session.GetInt32("Permission") == null){
                return RedirectToAction("Index", "Home");
            }
            return View("addAssessmentPage");
        }

        [HttpPost]
        [Route("addAssessment")]
        public IActionResult addAssessment(AssessmentViewModel model){
            if(HttpContext.Session.GetInt32("Permission") < 5){
                return RedirectToAction("dashboard", "Dashboard");
            }
            if(HttpContext.Session.GetInt32("Permission") == null){
                return RedirectToAction("Index", "Home");
            }
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

        [HttpGet]
        [Route("deleteAsstPage/{id}")]
        public IActionResult deleteAsstPage(int id){
            if(HttpContext.Session.GetInt32("Permission") < 9){
                return RedirectToAction("updateAllKids");
            }
            if(HttpContext.Session.GetInt32("Permission") == null){
                return RedirectToAction("Index", "Home");
            }
            List<DateTaken> curAsst = _context.DateTaken.Where(x => x.id == id).Include(k => k.Kid).Include(a => a.Assessment).ToList();
            ViewBag.assessment = curAsst[0];
            return View("deleteAsstConfirm");
        }

        [HttpGet]
        [Route("deleteAsst/{id}")]
        public IActionResult deleteAsst(int id){
            if(HttpContext.Session.GetInt32("Permission") < 9){
                return RedirectToAction("updateAllKids");
            }
            if(HttpContext.Session.GetInt32("Permission") == null){
                return RedirectToAction("Index", "Home");
            }
            DateTaken deleteThisAsst = _context.DateTaken.SingleOrDefault(x => x.id == id);
            _context.DateTaken.Remove(deleteThisAsst);
            _context.SaveChanges();
            return RedirectToAction("updateAssessmentPage", "Update", new {id = HttpContext.Session.GetInt32("kidId")});
        }

        [HttpGet]
        [Route("viewDueDecaPage")]
        public IActionResult viewDueDecaPage (){
            if(HttpContext.Session.GetInt32("Permission") == null){
                return RedirectToAction("Index", "Home");
            }
            return View("viewDueDecaPage");
        }

        [HttpPost]
        [Route("viewDueDeca")]
        public IActionResult viewDueDeca (int month, int year){
            if(HttpContext.Session.GetInt32("Permission") == null){
                return RedirectToAction("Index", "Home");
            }
            DateTime currentTime = DateTime.Now;
            if(year < currentTime.Year){
                year = currentTime.Year;
            }
            DateTime target = new DateTime(year, month, 15);
            List<Kid> allKids = _context.Kids.OrderBy(n => n.LastName).ToList();
            List<DateTaken> kidsDueDates = new List<DateTaken>();
            foreach(var kid in allKids){
                List<DateTaken> kidsDates = _context.DateTaken.Where(x => x.KidId == kid.id).Include(k => k.Kid).Include(a => a.Assessment).Where(p => p.Assessment.Name == "DECA").OrderByDescending(d => d.Date).ToList();
                if(kidsDates.Count > 0){
                    TimeSpan sinceRecentDeca = target - kidsDates[0].Date;
                    if(sinceRecentDeca.TotalDays > 170 && kid.Active == true){
                        kidsDueDates.Add(kidsDates[0]);
                    }
                }
            }
            ViewBag.kidsDue = kidsDueDates;
            List<Kid> allKidsNoDeca = _context.Kids.OrderBy(n => n.LastName).Include(d => d.DateTaken).ThenInclude(a => a.Assessment).ToList();
            List<Kid> noDecaKids = new List<Kid>();
            foreach(var kid in allKidsNoDeca){
                int count = 0;
                foreach(var asst in kid.DateTaken){
                    if(asst.Assessment.Name == "DECA"){
                        count += 1;
                    }
                }
                if(count == 0 && kid.Active == true){
                    noDecaKids.Add(kid);
                }
            }
            ViewBag.noDecaKids = noDecaKids;
            ViewBag.year = year;
            if(month == 1){
                ViewBag.month = "January";
            }
            if(month == 2){
                ViewBag.month = "February";
            }
            if(month == 3){
                ViewBag.month = "March";
            }
            if(month == 4){
                ViewBag.month = "April";
            }
            if(month == 5){
                ViewBag.month = "May";
            }
            if(month == 6){
                ViewBag.month = "June";
            }
            if(month == 7){
                ViewBag.month = "July";
            }
            if(month == 8){
                ViewBag.month = "August";
            }
            if(month == 9){
                ViewBag.month = "September";
            }
            if(month == 10){
                ViewBag.month = "October";
            }
            if(month == 11){
                ViewBag.month = "November";
            }
            if(month == 12){
                ViewBag.month = "December";
            }
            return View("kidsDuePage");
        }
    }
}