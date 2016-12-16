using Aristotle.Data;
using Aristotle.Models;
using Aristotle.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aristotle.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext context;

        public AttendanceController(UserManager<ApplicationUser> userManager, ApplicationDbContext ctx)
        {
            _userManager = userManager;
            context = ctx;
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Update(ProfileView model)
        {
            foreach (Attendance a in model.Attendance)
            {
                Attendance OriginalAttendance = await context.Attendance.Where(o => o.AttendanceId == a.AttendanceId).SingleAsync();

                if (ModelState.IsValid && a.CurrentlyAbsent != OriginalAttendance.CurrentlyAbsent)
                {
                    OriginalAttendance.CurrentlyAbsent = a.CurrentlyAbsent;
                    context.Entry(OriginalAttendance).State = EntityState.Modified;
                    context.Update(OriginalAttendance);
                    context.SaveChanges();

                }
            }
            return RedirectToAction("Index", new RouteValueDictionary(
                 new { controller = "Profile", action = "Index" }));

        }
    }
}
