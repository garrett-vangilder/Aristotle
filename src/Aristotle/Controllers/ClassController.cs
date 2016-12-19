﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Aristotle.Models;
using Aristotle.Data;
using Aristotle.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;

namespace Aristotle.Controllers
{
    [Authorize]
    public class ClassController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext context;

        public ClassController(UserManager<ApplicationUser> userManager, ApplicationDbContext ctx)
        {
            _userManager = userManager;
            context = ctx;
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);


        public async Task<IActionResult> Index()
        {

            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Add()
        {
            var user = await GetCurrentUserAsync();
            var model = new AddClassView(context, user);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Add(AddClassView model)
        {
            var user = await GetCurrentUserAsync();
            var newClass = new Class { Title = model.Title, ApplicationUserId = user.Id, Subject = model.Subject, StartTime = model.StartTime , EndTime = model.EndTime};

            if (ModelState.IsValid && newClass.ApplicationUserId != null)
            {
                context.Add(newClass);
                await context.SaveChangesAsync();
                return RedirectToAction("Index", new RouteValueDictionary(
                     new { controller = "Profile", action = "Index"}));
            }

            return View(model);
        }



        public async Task<IActionResult> Detail(int id)
        {
            var model = new DetailClassView();
            var user = await GetCurrentUserAsync();
            var dateAndTime = DateTime.Now;
            var today = dateAndTime.Date;
            string Title = context.Class.Where(c => c.ClassId == id).SingleOrDefault().Title;
            string Subject = context.Class.Where(c => c.ClassId == id).SingleOrDefault().Subject;
            List<Attendance> AttendanceList = new List<Attendance>();
            List<Attendance> AllAttendanceEverPerClass = new List<Attendance>();
            List<Attendance> AllAttendanceEver = await context.Attendance.ToListAsync();
            List<Student> AllStudents = await context.Student.ToListAsync();

            List<ClassMember> ClassMemberList = await context.ClassMember.Where(c => c.ClassId == id).ToListAsync();

            foreach (ClassMember ClassMember in ClassMemberList)
            {
                Attendance Attendance = await context.Attendance.Where(a => a.ClassMemberId == ClassMember.ClassMemberId && a.Date == today).SingleOrDefaultAsync();
                List<Attendance> AttendancePerStudentNotCurrent = await context.Attendance.Where(a => a.ClassMemberId == ClassMember.ClassMemberId).ToListAsync();

                AttendanceList.Add(Attendance);
                AllAttendanceEverPerClass.AddRange(AttendancePerStudentNotCurrent);
            }
            model.Attendance = AttendanceList;
            model.ClassMember = ClassMemberList;
            model.AverageAttendancePercentage = 100;
            model.Student = AllStudents;
            model.Title = Title;
            model.ClassId = id;
            model.Subject = Subject;


            return View(model);
        }

        public async Task<IActionResult> Update(int id, int dayAway)
        {
            var model = new DetailClassView();
            var user = await GetCurrentUserAsync();
            var dateAndTime = DateTime.Now.AddDays(dayAway);
            var today = dateAndTime.Date;
            string Title = context.Class.Where(c => c.ClassId == id).SingleOrDefault().Title;
            string Subject = context.Class.Where(c => c.ClassId == id).SingleOrDefault().Subject;
            List<Attendance> AttendanceList = new List<Attendance>();
            List<Attendance> AllAttendanceEverPerClass = new List<Attendance>();
            List<Attendance> AllAttendanceEver = await context.Attendance.ToListAsync();
            List<Student> AllStudents = await context.Student.ToListAsync();

            List<ClassMember> ClassMemberList = await context.ClassMember.Where(c => c.ClassId == id).ToListAsync();

            foreach (ClassMember ClassMember in ClassMemberList)
            {
                Attendance Attendance = await context.Attendance.Where(a => a.ClassMemberId == ClassMember.ClassMemberId && a.Date == today).SingleOrDefaultAsync();
                List<Attendance> AttendancePerStudentNotCurrent = await context.Attendance.Where(a => a.ClassMemberId == ClassMember.ClassMemberId).ToListAsync();

                AttendanceList.Add(Attendance);
                AllAttendanceEverPerClass.AddRange(AttendancePerStudentNotCurrent);
            }
            model.Attendance = AttendanceList;
            model.ClassMember = ClassMemberList;
            model.AverageAttendancePercentage = 100;
            model.Student = AllStudents;
            model.Title = Title;
            model.ClassId = id;
            model.Subject = Subject;


            return View(model);
        }


        public IActionResult Error()
        {
            return View();
        }
    }
}
