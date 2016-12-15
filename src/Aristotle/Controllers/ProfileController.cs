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
                        Attendance attendance = new Attendance { ClassMemberId = LocalClassMember.ClassMemberId, CurrentlyPresent = true, Date = today };
                        context.Add(attendance);
                    }
                    await context.SaveChangesAsync();
             
                }
            }
            model.ClassMember = ListOfClassMembers;
            model.Class = ClassList;
            model.ApplicationUser = user;
            model.StudentList = s;
            return View(model);
        }



        public IActionResult Error()
        {
            return View();
        }
    }
}