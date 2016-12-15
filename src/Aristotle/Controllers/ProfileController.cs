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
            List<Class> ClassList = await context.Class.Where(c => c.ApplicationUserId == user.Id).ToListAsync();
            var model = new ProfileView(context, user);
            List<Student> s = new List<Student>();
            List<ClassMember> ListOfClassMembers = new List<ClassMember>();

            foreach (Class c in ClassList) {
                List<ClassMember> cm = await context.ClassMember.Where(d => d.ClassId == c.ClassId).ToListAsync();
                foreach (ClassMember LocalClassMember in cm)
                {
                        Student student = await context.Student.Where(a => a.StudentId == LocalClassMember.StudentId).SingleOrDefaultAsync();
                    s.Add(student);
                    ListOfClassMembers.Add(LocalClassMember);
                }
            }
            model.ClassMember = ListOfClassMembers;
            model.Class = ClassList;
            model.ApplicationUser = user;
            model.StudentList = s;
            return View(model);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }


        public IActionResult Error()
        {
            return View();
        }
    }
}