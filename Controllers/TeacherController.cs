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
            if(HttpContext.Session.GetInt32("Permission") < 5){
                return RedirectToAction("dashboard", "Dashboard");
            }
            if(HttpContext.Session.GetInt32("Permission") == null){
                return RedirectToAction("Index", "Home");
            }
            ViewBag.permission = HttpContext.Session.GetInt32("Permission");
            return View("addTeacherPage");
        }

        [HttpPost]
        [Route("addTeacher")]
        public IActionResult addTeacher(TeacherViewModel model){
            if(HttpContext.Session.GetInt32("Permission") < 5){
                return RedirectToAction("dashboard", "Dashboard");
            }
            if(HttpContext.Session.GetInt32("Permission") == null){
                return RedirectToAction("Index", "Home");
            }
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

        [HttpGet]
        [Route("deleteTeacherPage")]
        public IActionResult deleteTeacherPage(){
            if(HttpContext.Session.GetInt32("Permission") < 9){
                return RedirectToAction("dashboard", "Dashboard");
            }
            if(HttpContext.Session.GetInt32("Permission") == null){
                return RedirectToAction("Index", "Home");
            }
            List<Teacher> allTeachers = _context.Teachers.OrderBy(l => l.LastName).ToList();
            ViewBag.allTeachers = allTeachers;
            return View("deleteTeacherPage");
        }

        [HttpPost]
        [Route("deleteTeacher")]
        public IActionResult deleteTeacher(int teacher){
            if(HttpContext.Session.GetInt32("Permission") < 9){
                return RedirectToAction("dashboard", "Dashboard");
            }
            if(HttpContext.Session.GetInt32("Permission") == null){
                return RedirectToAction("Index", "Home");
            }
            List<Kid> allKids = _context.Kids.Include(t => t.Teacher).ToList();
            int count = 0;
            foreach (var kid in allKids){
                if(kid.TeacherId == teacher){
                    count += 1;
                }
            }
            if(count > 0){
                List<Teacher> allTeachers = _context.Teachers.OrderBy(l => l.LastName).ToList();
                ViewBag.allTeachers = allTeachers;
                Teacher thisTeach = _context.Teachers.SingleOrDefault(t => t.id == teacher);
                ViewBag.teacherError = thisTeach.Prefix + " " + thisTeach.LastName + " cannot be removed at this time because there are still children listed as being in his/her class. If " + thisTeach.Prefix + " " + thisTeach.FirstName + " is no longer on staff, please go through and update the children to reflect the new teacher before removing this teacher.";
                return View("deleteTeacherPage");
            }
            Teacher deleteTeach = _context.Teachers.SingleOrDefault(t => t.id == teacher);
            _context.Teachers.Remove(deleteTeach);
            _context.SaveChanges();
            return RedirectToAction("addTeacherPage");
        }

        [HttpGet]
        [Route("editTeacherPage")]
        public IActionResult editTeacherPage(){
            if(HttpContext.Session.GetInt32("Permission") < 9){
                return RedirectToAction("dashboard", "Dashboard");
            }
            if(HttpContext.Session.GetInt32("Permission") == null){
                return RedirectToAction("Index", "Home");
            }
            ViewBag.teachers = _context.Teachers;
            return View("editTeacher");
        }

        [HttpGet]
        [Route("editTeacherPage2")]
        public IActionResult editTeacherPage2(int teacherId){
            if(HttpContext.Session.GetInt32("Permission") < 9){
                return RedirectToAction("dashboard", "Dashboard");
            }
            if(HttpContext.Session.GetInt32("Permission") == null){
                return RedirectToAction("Index", "Home");
            }
            ViewBag.teacher = _context.Teachers.SingleOrDefault(t => t.id == teacherId); 
            return View("editTeacherPage2");
        }

        [HttpPost]
        [Route("editTeacher")]
        public IActionResult editTeacher(int teacherId, TeacherViewModel model){
            if(HttpContext.Session.GetInt32("Permission") < 9){
                return RedirectToAction("dashboard", "Dashboard");
            }
            if(HttpContext.Session.GetInt32("Permission") == null){
                return RedirectToAction("Index", "Home");
            }
            Teacher teachToEdit = _context.Teachers.SingleOrDefault(t => t.id == teacherId);
            if(!ModelState.IsValid){
                return RedirectToAction("editTeacherPage2", new{id = teacherId});
            }
            teachToEdit.Prefix = model.Prefix;
            teachToEdit.FirstName = model.FirstName;
            teachToEdit.LastName = model.LastName;
            teachToEdit.GradeLevel = model.GradeLevel;
            _context.SaveChanges();
            return RedirectToAction("editTeacherPage");
        }
    }
} 