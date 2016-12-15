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

namespace Aristotle.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext context;

        public StudentController(UserManager<ApplicationUser> userManager, ApplicationDbContext ctx)
        {
            _userManager = userManager;
            context = ctx;
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        public async Task<IActionResult> Index()
        {
            var user = await GetCurrentUserAsync();
            List<Class> ClassList = await context.Class.ToListAsync();
            foreach (Class c in ClassList)
            {
                List<ClassMember> cm = await context.ClassMember.Where(d => d.ClassId == c.ClassId).ToListAsync();
                c.ClassMember = cm;
            }
            var model = new ProfileView(context, user);
            model.Class = ClassList;
            model.ApplicationUser = user;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Add([FromRoute]int id)
        {
            var user = await GetCurrentUserAsync();
            List<Student> StudentList = await context.Student.Where(s => s.ApplicationUserId == user.Id).ToListAsync();
            var model = new AddStudentViewModel(context, user);
            model.ClassId = id;
            model.Student = StudentList;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Add(AddStudentViewModel model, [FromRoute]int id)
        {
            var user = await GetCurrentUserAsync();
            var newStudent = new Student { FirstName = model.FirstName, LastName = model.LastName, Grade = model.Grade, ApplicationUserId = user.Id };

            if (ModelState.IsValid && newStudent.ApplicationUserId != null)
            {
                context.Add(newStudent);
                await context.SaveChangesAsync();

                Student student = await context.Student.Where(s => s.FirstName == newStudent.FirstName && s.LastName == newStudent.LastName && s.Grade == newStudent.Grade && s.ApplicationUserId == newStudent.ApplicationUserId).SingleOrDefaultAsync();

                ClassMember classMember = new ClassMember { StudentId = student.StudentId, ClassId = id};
                context.Add(classMember);
                await context.SaveChangesAsync();

                return RedirectToAction("Index", new RouteValueDictionary(
                     new { controller = "Profile", action = "Index" }));
            }

            return View(model);
        }


    }
}
