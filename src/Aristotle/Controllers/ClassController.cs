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
using Microsoft.AspNetCore.Routing;
using Aristotle.Services;

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
            //Establish user information
            var model = new DetailClassView();
            var user = await GetCurrentUserAsync();
            var dateAndTime = DateTime.Now;
            var today = dateAndTime.Date;
            string Title = context.Class.Where(c => c.ClassId == id).SingleOrDefault().Title;
            string Subject = context.Class.Where(c => c.ClassId == id).SingleOrDefault().Subject;

            //Creates needed information for LINQ Query
            List<Attendance> AttendanceList = new List<Attendance>();
            List<Attendance> AllAttendanceEverPerClass = new List<Attendance>();
            List<Attendance> AllAttendanceEver = await context.Attendance.ToListAsync();
            List<Student> AllStudents = await context.Student.OrderBy(s =>s.LastName).ToListAsync();
            List<ClassMember> ClassMemberList = await context.ClassMember.Where(c => c.ClassId == id).OrderBy(c => c.ClassMemberId).ToListAsync();

            //Creates Changeable Attendance List in Class Detail View
            foreach (ClassMember ClassMember in ClassMemberList)
            {
                Attendance Attendance = await context.Attendance.Where(a => a.ClassMemberId == ClassMember.ClassMemberId && a.Date == today).SingleOrDefaultAsync();
                List<Attendance> AttendancePerStudentNotCurrent = await context.Attendance.Where(a => a.ClassMemberId == ClassMember.ClassMemberId).ToListAsync();

                AttendanceList.Add(Attendance);
                AllAttendanceEverPerClass.AddRange(AttendancePerStudentNotCurrent);
            }

            //Creates Top 5 Attendance List
            model.Top5Attendance = Utility.FindTop5Students(AllStudents, ClassMemberList, AllAttendanceEver, today);
            model.Bottom5Attendance = Utility.FindBottom5(AllStudents, ClassMemberList, AllAttendanceEver, today); 

            //Applies Data to View-Model
            model.Attendance = AttendanceList;
            model.AllAttendance = AllAttendanceEver;
            model.ClassMember = ClassMemberList;
            model.DailyAverageAttendance = Math.Round(Utility.FindAverageAttendanceByClassForToday(AllAttendanceEver, ClassMemberList, today));
            model.ClassAverageAttendancePercentage = Math.Round(Utility.FindAverageAttendanceByClass(AllAttendanceEver, ClassMemberList, today));
            model.AverageAttendancePercentage = Math.Round(Utility.FindAverageAttendanceBySchool(AllAttendanceEver, today));
            model.Student = AllStudents;
            model.Title = Title;
            model.ClassId = id;
            model.Subject = Subject;
            model.DesiredDate = today;
            
            //Returns View Model with needed information to View
            return View(model);
        }

        public async Task<IActionResult> Update(int id, int dayAway)
        {
            var model = new DetailClassView();
            var user = await GetCurrentUserAsync();
            var dateAndTime = DateTime.Now.AddDays(dayAway);
            var DesiredDate = dateAndTime.Date;
            if (Convert.ToString(DesiredDate.DayOfWeek) == "Saturday" || Convert.ToString(DesiredDate.DayOfWeek) == "Sunday")
            {
                if (dayAway > 0)
                {
                    dayAway++;
                    return RedirectToAction("Update", new RouteValueDictionary(
                        new { controller = "Class", action = "Update", Id = id, dayAway = dayAway}));
                }
            }
            string Title = context.Class.Where(c => c.ClassId == id).SingleOrDefault().Title;
            string Subject = context.Class.Where(c => c.ClassId == id).SingleOrDefault().Subject;
            List<Attendance> AttendanceList = new List<Attendance>();
            List<Attendance> AllAttendanceEverPerClass = new List<Attendance>();
            List<Attendance> AllAttendanceEver = await context.Attendance.ToListAsync();
            List<Student> AllStudents = await context.Student.ToListAsync();
            List<ClassMember> ClassMemberList = await context.ClassMember.Where(c => c.ClassId == id).ToListAsync();

            foreach (ClassMember ClassMember in ClassMemberList)
            {
                Attendance Attendance = await context.Attendance.Where(a => a.ClassMemberId == ClassMember.ClassMemberId && a.Date == DesiredDate).SingleOrDefaultAsync();
                if (Attendance == null)
                {
                   Attendance = new Attendance { ClassMemberId = ClassMember.ClassMemberId, CurrentlyAbsent = false, Date = DesiredDate };
                }
                List<Attendance> AttendancePerStudentNotCurrent = await context.Attendance.Where(a => a.ClassMemberId == ClassMember.ClassMemberId).ToListAsync();
                AttendanceList.Add(Attendance);
                AllAttendanceEverPerClass.AddRange(AttendancePerStudentNotCurrent);
            }

            model.NewDayDifferenceFromToday = dayAway + 1;
            model.PreviousDayDifferenceFromToday = dayAway - 1;
            model.Attendance = AttendanceList;
            model.ClassMember = ClassMemberList;
            model.AverageAttendancePercentage = 100;
            model.Student = AllStudents;
            model.Title = Title;
            model.ClassId = id;
            model.Subject = Subject;
            model.DesiredDate = DesiredDate;

            if (dayAway == 0)
            {
                return RedirectToAction("Detail", new RouteValueDictionary(
                    new { controller = "Class", action = "Detail", Id = id }));
            }

            return View(model);
        }

        public async Task<ActionResult> SchoolAttendanceChart()
        {

            int id = Convert.ToInt32(RouteData.Values["id"]);      
            List<Attendance> AllAttendanceEver = await context.Attendance.ToListAsync();
            List<ClassMember> ClassMemberList = await context.ClassMember.Where(c => c.ClassId == id).OrderBy(c => c.ClassMemberId).ToListAsync();


            //Create Labels
            List<String> Last10SchoolDays = new List<String>();
            var dateAndTime = DateTime.Now;
            var today = dateAndTime.Date;
            for (var i = 0; i <= 14; i++)
            {
                if (Convert.ToString(today.AddDays(-i).DayOfWeek) != "Saturday" && Convert.ToString(today.AddDays(-i).DayOfWeek) != "Sunday")
                {
                    String DayOfWeek = Convert.ToString(today.AddDays(-i).Date.DayOfWeek);
                    Last10SchoolDays.Insert(0, $"{DayOfWeek}, {today.AddDays(-i).Month}/{today.AddDays(-i).Day}");
                }

            }
            if (Last10SchoolDays.Count() <= 0)
            {
                Last10SchoolDays.Add(Convert.ToString(today.Day));
            }

            //Create Data
            List<double> AverageAttendaceFor2Weeks = new List<double>();
            for (var i = 0; i <= 14; i++)
            {
                if (Convert.ToString(today.AddDays(-i).DayOfWeek) != "Saturday" && Convert.ToString(today.AddDays(-i).DayOfWeek) != "Sunday")
                {
                    double DataToAdd = Math.Round(Utility.FindAverageAttendanceByClassForToday(AllAttendanceEver, ClassMemberList, today.AddDays(-i)));
                    AverageAttendaceFor2Weeks.Insert(0, DataToAdd);
                }

            }



            //Adds Information to Result Object
            List<object> dataForChart = new List<object>();
            dataForChart.Insert(0, Last10SchoolDays);
            dataForChart.Insert(1, AverageAttendaceFor2Weeks);

            var result = Json(dataForChart);
            return result;

        }


        public IActionResult Error()
        {
            return View();
        }
    }
}
