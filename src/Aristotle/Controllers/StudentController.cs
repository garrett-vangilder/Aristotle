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

        public async Task<IActionResult> Add()
        {
            var user = await GetCurrentUserAsync();
            List<Student> StudentList = await context.Student.Where(s => s.ApplicationUserId == user.Id).ToListAsync();
            var model = new AddStudentViewModel(context, user);
            model.Student = StudentList;

            return View(model);
        }
    }
}
