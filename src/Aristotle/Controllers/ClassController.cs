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
