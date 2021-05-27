using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using educ_system.Validation;

#nullable disable

namespace educ_system.Models
{
    public partial class Teacher
    {
        public Teacher()
        {
            Courses = new HashSet<Course>();
        }

        public int Id { get; set; }
        public int SubjectId { get; set; }
        [Required(ErrorMessage = "The field must not be empty")]
        [RegularExpression(@"^[A-Z][a-zA-Z]{3,}(?: [A-Z][a-zA-Z]*){0,2}$", ErrorMessage = "The field must be alphabet only, first letter upcast, ")]
        [TeacherNameUnique]
        public string Name { get; set; }
        public string Info { get; set; }

        public virtual Subject Subject { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
    }
}
