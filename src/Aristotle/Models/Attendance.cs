using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Aristotle.Models
{
    public class Attendance
    {
        [Key]
        public int AttendanceId { get; set; }

        [Required]
        public int ClassMemberId { get; set; }

        [Required]
        public bool CurrentlyPresent { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
