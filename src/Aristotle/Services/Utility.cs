using Aristotle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Aristotle.Data;

namespace Aristotle.Services
{
    public class Utility
    {
        private ApplicationDbContext context;

        public Utility(ApplicationDbContext ctx)
        {
            context = ctx;
        }

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

        internal static List<Student> FindTop5Students(List<Student> AllStudentList, List<ClassMember> ClassMemberList, List<Attendance> AttendanceList, DateTime today)
        {
            List<Student> Top5 = new List<Student>();

            foreach (ClassMember cm in ClassMemberList)
            {
                Student StudentInfo = AllStudentList.Where(s => s.StudentId == cm.StudentId).SingleOrDefault();
                double TotalAmountOfSchoolDays = AttendanceList.Where(a => a.ClassMemberId == cm.ClassMemberId && a.Date <= today).Count();
                double DaysAttendedByClassMember = AttendanceList.Where(a => a.ClassMemberId == cm.ClassMemberId && a.Date <= today && a.CurrentlyAbsent == false).Count();
                double DaysMissedByClassMember = AttendanceList.Where(a => a.ClassMemberId == cm.ClassMemberId && a.Date <= today && a.CurrentlyAbsent).Count();
                double numerator = DaysAttendedByClassMember;
                double denominator = TotalAmountOfSchoolDays;
                double AttendanceRating = (DaysAttendedByClassMember / TotalAmountOfSchoolDays) * 100;

                //If Top 5 is not yet populated
                if (Top5.Count < 5)
                {
                    Top5.Add(StudentInfo);
                }
                else
                {
                    //Will loop through all students currently in the top 5 list
                    foreach(Student student in Top5)
                    {
                        ClassMember ComparisonClassMember = ClassMemberList.Where(ccm => ccm.StudentId == student.StudentId).SingleOrDefault();
                        double ComparisonNumerator = AttendanceList.Where(a => a.ClassMemberId == ComparisonClassMember.ClassMemberId && a.Date <= today && a.CurrentlyAbsent == false).Count();
                        double ComparisonDenominator = AttendanceList.Where(a => a.ClassMemberId == ComparisonClassMember.ClassMemberId && a.Date <= today).Count();

                        var ComparisonStudent = new
                        {
                            ID = student.StudentId,
                            AttendanceRating = (ComparisonNumerator / ComparisonDenominator) * 100
                        };
                        if (ComparisonStudent.AttendanceRating < AttendanceRating)
                        {
                            Student StudentToRemove = Top5.Single(r => r.StudentId == ComparisonStudent.ID);
                            Top5.Remove(StudentToRemove);
                            Top5.Add(StudentInfo);
                            break;
                        }
                    }
                }

            }

            return Top5.OrderBy( s => s.LastName).ToList();
        }

        public static double FindAttendanceForStudent(int Id, List<ClassMember> ClassMembers, List<Attendance> AllAttendance, DateTime today)
        {
            ClassMember DesiredClassMember = ClassMembers.Where(cm => cm.StudentId == Id).SingleOrDefault();

            double DaysAttended = AllAttendance.Where(a => a.ClassMemberId == DesiredClassMember.ClassMemberId && a.Date <= today && a.CurrentlyAbsent == false).Count();
            double AvailableDays = AllAttendance.Where(a => a.ClassMemberId == DesiredClassMember.ClassMemberId && a.Date <= today).Count();

            return (DaysAttended / AvailableDays) * 100;
        }


    }
}
