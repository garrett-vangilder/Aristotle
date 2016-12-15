using System.Collections.Generic;
using System.Linq;
using Aristotle.Models;
using Aristotle.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace Aristotle.ViewModels
{

    public class AddStudentViewModel : BaseViewModel
    {
        public IEnumerable<Student> Student { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Grade { get; set; }
        public int ClassId { get; set; }


        public AddStudentViewModel(ApplicationDbContext ctx, ApplicationUser user) : base(ctx, user) { }
        public AddStudentViewModel() { }
    }
}
