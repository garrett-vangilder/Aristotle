
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Aristotle.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public int Grade { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }


        // Foreign Key Dependencies
        public ICollection<Student> Students;
    }
}
