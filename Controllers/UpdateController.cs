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
    public class UpdateController : Controller
    {
        private TrackerContext _context;
        public UpdateController(TrackerContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("updateAllKids")]
        public IActionResult updateAllKids(){
            if(HttpContext.Session.GetInt32("Permission") < 5){
                return RedirectToAction("dashboard", "Dashboard");
            }
            if(HttpContext.Session.GetInt32("Permission") == null){
                return RedirectToAction("Index", "Home");
            }
            HttpContext.Session.Remove("kidId");
            if(HttpContext.Session.GetString("sort") == "sortLast"){
                ViewBag.allKids = _context.Kids.Include(kid => kid.Teacher).OrderBy(l => l.LastName);
            }
            if(HttpContext.Session.GetString("sort") == "sortFirst"){
                ViewBag.allKids = _context.Kids.Include(kid => kid.Teacher).OrderBy(l => l.FirstName);
            }
            if(HttpContext.Session.GetString("sort") == "sortBirthdate"){
                ViewBag.allKids = _context.Kids.Include(kid => kid.Teacher).OrderBy(l => l.Birthdate);
            }
            if(HttpContext.Session.GetString("sort") == "sortTeacher"){
                ViewBag.allKids = _context.Kids.Include(kid => kid.Teacher).OrderBy(l => l.Teacher.LastName);
            }
            if(HttpContext.Session.GetString("sort") == "sortStatus"){
                ViewBag.allKids = _context.Kids.Include(kid => kid.Teacher).OrderByDescending(l => l.Active).ThenBy(n => n.LastName);
            }
            ViewBag.permission = HttpContext.Session.GetInt32("Permission");
            return View("updateAllKids");
        }

        [HttpGet]
        [Route("updateAllKids/sortFirst")]
        public IActionResult updateAllKidsSortFirst(){
            HttpContext.Session.SetString("sort", "sortFirst");
            return RedirectToAction("updateAllKids");
        }

        [HttpGet]
        [Route("updateAllKids/sortLast")]
        public IActionResult updateAllKidsSortLast(){
            HttpContext.Session.SetString("sort", "sortLast");
            return RedirectToAction("updateAllKids");
        }

        [HttpGet]
        [Route("updateAllKids/sortBirthdate")]
        public IActionResult updateAllKidsSortBirthdate(){
            HttpContext.Session.SetString("sort", "sortBirthdate");
            return RedirectToAction("updateAllKids");
        }

        [HttpGet]
        [Route("updateAllKids/sortTeacher")]
        public IActionResult updateAllKidsSortTeacher(){
            HttpContext.Session.SetString("sort", "sortTeacher");
            return RedirectToAction("updateAllKids");
        }

        [HttpGet]
        [Route("updateAllKids/sortStatus")]
        public IActionResult updateAllKidsSortStatus(){
            HttpContext.Session.SetString("sort", "sortStatus");
            return RedirectToAction("updateAllKids");
        }

        [HttpGet]
        [Route("deactivateKid/{id}")]
        public IActionResult deactivateKid(int id){
            if(HttpContext.Session.GetInt32("Permission") < 5){
                return RedirectToAction("dashboard", "Dashboard");
            }
            if(HttpContext.Session.GetInt32("Permission") == null){
                return RedirectToAction("Index", "Home");
            }
            Kid kid = _context.Kids.SingleOrDefault(x => x.id == id);
            kid.Active = false;
            _context.SaveChanges();
            return RedirectToAction("updateAllKids");
        }

        [HttpGet]
        [Route("activateKid/{id}")]
        public IActionResult activateKid(int id){
            if(HttpContext.Session.GetInt32("Permission") < 5){
                return RedirectToAction("dashboard", "Dashboard");
            }
            if(HttpContext.Session.GetInt32("Permission") == null){
                return RedirectToAction("Index", "Home");
            }
            Kid kid = _context.Kids.SingleOrDefault(x => x.id == id);
            kid.Active = true;
            _context.SaveChanges();
            return RedirectToAction("updateAllKids");
        }

        [HttpGet]
        [Route("updateAssessmentPage/{id}")]
        public IActionResult updateAssessmentPage(int id){
            if(HttpContext.Session.GetInt32("Permission") < 5){
                return RedirectToAction("dashboard", "Dashboard");
            }
            if(HttpContext.Session.GetInt32("Permission") == null){
                return RedirectToAction("Index", "Home");
            }
            ViewBag.singleKid = _context.Kids.Where(kid => kid.id == id).Include(t => t.Teacher).ToArray();
            List<DateTaken> DateTakens = _context.DateTaken.Where(x => x.KidId == id).Include(a => a.Assessment).OrderByDescending(d => d.Date).ToList();
            ViewBag.assessmentsTaken = DateTakens;
            TimeSpan age = DateTime.Now - ViewBag.singleKid[0].Birthdate;
            double decimalYears = age.TotalDays/365;
            var years = Math.Truncate(decimalYears);
            double decimalMonths = (age.TotalDays % 365)/30;
            var months = Convert.ToInt32(decimalMonths);
            if(months==12){
                months=11;
            }
            ViewBag.years = years;
            ViewBag.months = months;
            ViewBag.assessments = _context.Assessments;
            HttpContext.Session.SetInt32("kidId", id);
            return View("updateAssessmentPage");
        }

        [HttpPost]
        [Route("updateAssessments")]
        public IActionResult updateAssessments(DateTakenViewModel model){
            if(HttpContext.Session.GetInt32("Permission") < 5){
                return RedirectToAction("dashboard", "Dashboard");
            }
            if(HttpContext.Session.GetInt32("Permission") == null){
                return RedirectToAction("Index", "Home");
            }
            if(!ModelState.IsValid){
                int? id = HttpContext.Session.GetInt32("kidId");
                ViewBag.singleKid = _context.Kids.Where(kiddo => kiddo.id == id).Include(t => t.Teacher).ToArray();
                List<DateTaken> DateTakens = _context.DateTaken.Where(x => x.KidId == id).Include(a => a.Assessment).OrderByDescending(d => d.Date).ToList();
                ViewBag.assessmentsTaken = DateTakens;
                TimeSpan age = DateTime.Now - ViewBag.singleKid[0].Birthdate;
                double decimalYears = age.TotalDays/365;
                var years = Math.Truncate(decimalYears);
                double decimalMonths = (age.TotalDays % 365)/30;
                var months = Convert.ToInt32(decimalMonths);
                if(months==12){
                    months=11;
                }
                ViewBag.years = years;
                ViewBag.months = months;
                ViewBag.assessments = _context.Assessments;
                return View("updateAssessmentPage");
            }
            Kid kid = _context.Kids.SingleOrDefault(x => x.id == HttpContext.Session.GetInt32("kidId"));
            Assessment assessment = _context.Assessments.SingleOrDefault(a => a.Name == model.Asst);
            DateTaken newDateTaken = new DateTaken {
                Date = model.Date,
                Score = model.Score,
                Progress = model.Progress,
                Comment = model.Comment,
                AssessmentId = assessment.id,
                KidId = kid.id
            };
            _context.DateTaken.Add(newDateTaken);
            
            _context.SaveChanges();
            return RedirectToAction("updateAssessmentPage", new {id = HttpContext.Session.GetInt32("kidId")});
        }

        [HttpGet]
        [Route("updateSingleKidPage/{id}")]
        public IActionResult updateSingleKidPage (int id){
            if(HttpContext.Session.GetInt32("Permission") < 9){
                return RedirectToAction("updateAllKids");
            }
            if(HttpContext.Session.GetInt32("Permission") == null){
                return RedirectToAction("Index", "Home");
            }
            List<Kid> kid = _context.Kids.Where(x => x.id == id).Include(k => k.Teacher).ToList();
            ViewBag.kid = kid;
            ViewBag.teachers = _context.Teachers;
            return View("updateSingleKid");
        }

        [HttpPost]
        [Route("updateSingleKid/{id}")]
        public IActionResult updateSingleKid (int id, KidViewModel model){
            if(HttpContext.Session.GetInt32("Permission") < 9){
                return RedirectToAction("dashboard", "Dashboard");
            }
            if(HttpContext.Session.GetInt32("Permission") == null){
                return RedirectToAction("Index", "Home");
            }
            if(!ModelState.IsValid){
                List<Kid> kid = _context.Kids.Where(x => x.id == id).Include(k => k.Teacher).ToList();
                ViewBag.kid = kid;
                ViewBag.teachers = _context.Teachers;
                return View("updateSingleKid");
            }
            Kid updatedKid = _context.Kids.SingleOrDefault(x => x.id == id);
            Teacher updatedTeacher = _context.Teachers.SingleOrDefault(p => p.LastName == model.Teacher);
            updatedKid.FirstName = model.FirstName;
            updatedKid.LastName = model.LastName;
            updatedKid.Birthdate = model.Birthdate;
            updatedKid.Teacher = updatedTeacher;
            _context.SaveChanges();
            return RedirectToAction("updateAllKids");
        }

        [HttpGet]
        [Route("deleteKidPage/{id}")]
        public IActionResult deleteKidPage (int id){
            if(HttpContext.Session.GetInt32("Permission") < 9){
                return RedirectToAction("dashboard", "Dashboard");
            }
            if(HttpContext.Session.GetInt32("Permission") == null){
                return RedirectToAction("Index", "Home");
            }
            Kid kid = _context.Kids.SingleOrDefault(x => x.id == id);
            ViewBag.kid = kid;
            return View("deleteKid");
        }

        [HttpGet]
        [Route("deleteKid/{id}")]
        public IActionResult deleteKid (int id){
            if(HttpContext.Session.GetInt32("Permission") < 9){
                return RedirectToAction("dashboard", "Dashboard");
            }
            if(HttpContext.Session.GetInt32("Permission") == null){
                return RedirectToAction("Index", "Home");
            }
            Kid kid = _context.Kids.SingleOrDefault(x => x.id == id);
            _context.Kids.Remove(kid);
            _context.SaveChanges();
            return RedirectToAction("updateAllKids");
        }
        
    }
}