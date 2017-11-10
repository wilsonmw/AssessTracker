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
    public class TeacherController : Controller
    {
        private TrackerContext _context;
        public TeacherController(TrackerContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("addTeacherPage")]
        public IActionResult addTeacherPage(){
            if(HttpContext.Session.GetInt32("Permission") != 9){
                return RedirectToAction("dashboard", "Dashboard");
            }
            return View("addTeacherPage");
        }

        [HttpPost]
        [Route("addTeacher")]
        public IActionResult addTeacher(TeacherViewModel model){
            if(!ModelState.IsValid){
                ViewBag.addTeacherError = "Please fill out all fields.";
                return View("addKidPage", model);
            }
            Teacher newTeacher = new Teacher{
                Prefix = model.Prefix,
                FirstName = model.FirstName,
                LastName = model.LastName,
                GradeLevel = model.GradeLevel,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now

            };
            _context.Teachers.Add(newTeacher);
            _context.SaveChanges();
            return RedirectToAction("dashboard", "Dashboard");
        }
    }
} 