using System;
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
using Aristotle.Services;

namespace Aristotle.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext context;

        public ProfileController(UserManager<ApplicationUser> userManager, ApplicationDbContext ctx)
        {
            _userManager = userManager;
            context = ctx;
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);


        public async Task<IActionResult> Index()
        {
            var user = await GetCurrentUserAsync();
            var model = new ProfileView(context, user);
            var dateAndTime = DateTime.Now;
            var today = dateAndTime.Date;

            List<Class> ClassList = await context.Class.Where(c => c.ApplicationUserId == user.Id).ToListAsync();
            List<Student> s = new List<Student>();
            List<ClassMember> ListOfClassMembers = new List<ClassMember>();
            List<Attendance> ListOfAttendance = new List<Attendance>();

            foreach (Class c in ClassList) {
                List<ClassMember> cm = await context.ClassMember.Where(d => d.ClassId == c.ClassId).ToListAsync();
                foreach (ClassMember LocalClassMember in cm)
                {

                    Student student = await context.Student.Where(LocalStudent => LocalStudent.StudentId == LocalClassMember.StudentId).SingleOrDefaultAsync();
                    s.Add(student);
                    ListOfClassMembers.Add(LocalClassMember);

                    Attendance a = await context.Attendance.Where(b => b.ClassMemberId == LocalClassMember.ClassMemberId && b.Date == today).SingleOrDefaultAsync();

                    if (a == null)
                    {
                        Attendance attendance = new Attendance { ClassMemberId = LocalClassMember.ClassMemberId, CurrentlyAbsent = false, Date = today };
                        context.Add(attendance);
                        await context.SaveChangesAsync();
                        ListOfAttendance.Add(attendance);
                    }
                    else
                    {
                        ListOfAttendance.Add(a);
                    }


                }
            }

            model.Attendance = ListOfAttendance;
            model.ClassMember = ListOfClassMembers;
            model.Class = ClassList;
            model.ApplicationUser = user;
            model.StudentList = s;
            return View(model);
        }

        public async Task<ActionResult> SchoolAttendanceChart()
        {
            var user = await GetCurrentUserAsync();
            var dateAndTime = DateTime.Now;
            var today = dateAndTime.Date;

            List<Class> ClassList = await context.Class.Where(c => c.ApplicationUserId == user.Id).ToListAsync();
            List<Attendance> AllAttendanceEver = await context.Attendance.ToListAsync();

            List<string> ClassTitles = new List<string>();
            List<double> TodaysAttendance = new List<double>();
            List<double> AverageAttendance = new List<double>();

            foreach(Class classInfo in ClassList)
            {
                List<ClassMember> ClassMemberList = await context.ClassMember.Where(c => c.ClassId == classInfo.ClassId).OrderBy(c => c.ClassMemberId).ToListAsync();
                double AverageAttendanceForToday = Math.Round(Utility.FindAverageAttendanceByClassForToday(AllAttendanceEver, ClassMemberList, today));
                double AverageAttendanceForAllDays = Math.Round(Utility.FindAverageAttendanceByClass(AllAttendanceEver, ClassMemberList, today));

                ClassTitles.Add(classInfo.Title);
                TodaysAttendance.Add(AverageAttendanceForToday);
                AverageAttendance.Add(AverageAttendanceForAllDays);
            }


            List<object> dataForChart = new List<object>();
            dataForChart.Insert(0, ClassTitles);
            dataForChart.Insert(1, TodaysAttendance);
            dataForChart.Insert(2, AverageAttendance);

            var result = Json(dataForChart);
            return result;
        }


        public IActionResult Error()
        {
            return View();
        }
    }
}