
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Aristotle.Models
{
    public class Class
    {
        [Key]
        public int ClassId { get; set; }

        [Required]
        public int ApplicationUserId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        public string Subject { get; set; }

        // Foreign Key Dependencies
        public ICollection<ClassMember> ClassMember;
    }
}
