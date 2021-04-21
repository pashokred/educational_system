using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable enable

namespace educ_system.Models
{
    public partial class Type
    {
        public Type()
        {
            Courses = new HashSet<Course>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "The field must not be empty")]
        [RegularExpression(@"^[a-zA-Z]+.*$", ErrorMessage = "First alphabet")]
        public string Name { get; set; }
        public string? Info { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }
}
