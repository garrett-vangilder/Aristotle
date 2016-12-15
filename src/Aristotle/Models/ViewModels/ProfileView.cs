using System.Collections.Generic;
using System.Linq;
using Aristotle.Models;
using Aristotle.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Aristotle.ViewModels
{
 
    public class ProfileView : BaseViewModel
    {
        public ApplicationUser ApplicationUser { get; set; }
        public IEnumerable<Class> Class { get; set; }
        public IEnumerable<ClassMember> ClassMember { get; set; }
        public IEnumerable<Student> StudentList { get; set; }
        public IEnumerable<Attendance> Attendance { get; set; }

        public ProfileView(ApplicationDbContext ctx, ApplicationUser user) : base(ctx, user) { }
        public ProfileView() { }
    }
}
