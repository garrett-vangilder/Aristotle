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
            double numerator = Convert.ToDouble(CurrentAttendance.Count()) - Convert.ToDouble(AbsentStudents.Count());
            double denominator = Convert.ToDouble(CurrentAttendance.Count());

            return (numerator / denominator) * 100;
        }

        public static double FindAverageAttendanceByClass(List<Attendance> AllAttendance, List<ClassMember> ClassMemberList, DateTime today)
        {
            List<Attendance> ClassMemberAttendance = new List<Attendance>();
            foreach (ClassMember ClassMember in ClassMemberList)
            {
                foreach (Attendance attendance in AllAttendance)
                {
                    if (attendance.ClassMemberId == ClassMember.ClassMemberId)
                    {
                        ClassMemberAttendance.Add(attendance);
                    }
                }
            }
            List<Attendance> CurrentAttendance = ClassMemberAttendance.Where(a => a.Date <= today).ToList();
            List<Attendance> AbsentStudents = CurrentAttendance.Where(a => a.CurrentlyAbsent == true).ToList();
            double numerator = Convert.ToDouble(CurrentAttendance.Count()) - Convert.ToDouble(AbsentStudents.Count());
            double denominator = Convert.ToDouble(CurrentAttendance.Count());

            return (numerator / denominator) * 100;
        }

        public static double FindAverageAttendanceByClassForToday(List<Attendance> AllAttendance, List<ClassMember> ClassMemberList, DateTime today)
        {
            List<Attendance> ClassMemberAttendance = new List<Attendance>();
            foreach (ClassMember ClassMember in ClassMemberList)
            {
                foreach (Attendance attendance in AllAttendance)
                {
                    if (attendance.ClassMemberId == ClassMember.ClassMemberId)
                    {
                        ClassMemberAttendance.Add(attendance);
                    }
                }
            }
            List<Attendance> CurrentAttendance = ClassMemberAttendance.Where(a => a.Date == today).ToList();
            List<Attendance> AbsentStudents = CurrentAttendance.Where(a => a.CurrentlyAbsent == true).ToList();
            double numerator = Convert.ToDouble(CurrentAttendance.Count()) - Convert.ToDouble(AbsentStudents.Count());
            double denominator = Convert.ToDouble(CurrentAttendance.Count());

            return (numerator / denominator) * 100;
        }
    }
}
