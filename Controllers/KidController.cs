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
    public class KidController : Controller
    {
        private TrackerContext _context;
        public KidController(TrackerContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("addKidPage")]
        public IActionResult addScreen(){
            if(HttpContext.Session.GetInt32("Permission") != 9){
                return RedirectToAction("dashboard", "Dashboard");
            }
            ViewBag.teachers = _context.Teachers;
            return View("addKidPage");
        }

        // [HttpPost]
        [Route("addKid")]
        public IActionResult addKid(KidViewModel model){
            if(!ModelState.IsValid){
                ViewBag.addKidError = "Please fill out all fields.";
                ViewBag.teachers = _context.Teachers;
                return View("addKidPage", model);
            }
            Teacher curTeacher = _context.Teachers.SingleOrDefault(teach => teach.LastName == model.Teacher);
            Kid newKid = new Kid{
                FirstName = model.FirstName,
                LastName = model.LastName,
                Birthdate = model.Birthdate,
                Active = true,
                TeacherId = curTeacher.id,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now

            };
            _context.Kids.Add(newKid);
            _context.SaveChanges();
            return RedirectToAction("dashboard", "Dashboard");
        }

        [HttpGet]
        [Route("viewAllKids")]
        public IActionResult viewAllKids(){
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
            return View("viewAllKids");
        }

        [HttpGet]
        [Route("viewAllKids/sortFirst")]
        public IActionResult viewAllKidsSortFirst(){
            HttpContext.Session.SetString("sort", "sortFirst");
            return RedirectToAction("viewAllKids");
        }

        [HttpGet]
        [Route("viewAllKids/sortLast")]
        public IActionResult viewAllKidsSortLast(){
            HttpContext.Session.SetString("sort", "sortLast");
            return RedirectToAction("viewAllKids");
        }

        [HttpGet]
        [Route("viewAllKids/sortBirthdate")]
        public IActionResult viewAllKidsSortBirthdate(){
            HttpContext.Session.SetString("sort", "sortBirthdate");
            return RedirectToAction("viewAllKids");
        }

        [HttpGet]
        [Route("viewAllKids/sortTeacher")]
        public IActionResult viewAllKidsSortTeacher(){
            HttpContext.Session.SetString("sort", "sortTeacher");
            return RedirectToAction("viewAllKids");
        }

        [HttpGet]
        [Route("viewAllKids/sortStatus")]
        public IActionResult viewAllKidsSortStatus(){
            HttpContext.Session.SetString("sort", "sortStatus");
            return RedirectToAction("viewAllKids");
        }


        [HttpGet]
        [Route("viewSingleKid/{id}")]
        public IActionResult viewSingleKid(int id){
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
            return View("viewSingleKid");
        }

        
    }
}