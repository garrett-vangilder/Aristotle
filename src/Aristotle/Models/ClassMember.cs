using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Aristotle.Models
{
    public class ClassMember
    {
        [Key]
        public int ClassMemberId { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public int ClassId { get; set; }


        // Foreign Key Dependencies
        public ICollection<Attendance> Attendace;
    }
}
