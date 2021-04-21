using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace educ_system.Models
{
    public partial class Student
    {
        public Student()
        {
            Marks = new HashSet<Mark>();
            StudentGroups = new HashSet<StudentGroup>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "The field must not be empty")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z]\s*$", ErrorMessage = "The field must be alphabet only, first letter upcast")]
        public string Name { get; set; }
        public virtual ICollection<Mark> Marks { get; set; }
        public virtual ICollection<StudentGroup> StudentGroups { get; set; }
    }
}
