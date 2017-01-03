using Aristotle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Aristotle.Services
{
    public class Utility
    {
        public static double FindAverageAttendanceBySchool(List<Attendance> AllAttendance, DateTime today)
        {
            List<Attendance> CurrentAttendance = AllAttendance.Where(a => a.Date <= today).ToList();
            List<Attendance> AbsentStudents = CurrentAttendance.Where(a => a.CurrentlyAbsent == true).ToList();

            double baseNumber = Convert.ToDouble(CurrentAttendance.Count()) / 100;


            return Math.Ceiling((Convert.ToDouble(CurrentAttendance.Count()) - (Convert.ToDouble(AbsentStudents.Count()) / Convert.ToDouble(CurrentAttendance.Count()))));
        }
    }
}
