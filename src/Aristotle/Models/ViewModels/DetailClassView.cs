using Aristotle.Data;
using Aristotle.Models;
using Aristotle.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aristotle.ViewModels
{
    public class DetailClassView : BaseViewModel
    {
        public string Title { get; set; }
        public string Subject { get; set; }
        public int ClassId { get; set; }
        public List<Attendance> Attendance { get; set; }
        public List<Student> Student { get; set; }
        public List<ClassMember> ClassMember { get; set; }
        public List<int> AverageAttendanceByStudent { get; set; }
        public int OverallAttendancePercentage { get; set; }
        public int CurrentAttendancePercentage { get; set; }
        public int AverageAttendancePercentage { get; set; }
        public DateTime DesiredDate { get; set; }


        public DetailClassView(ApplicationDbContext ctx, ApplicationUser user) : base(ctx, user) { }
        public DetailClassView() { }
    }
}
