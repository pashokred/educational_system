using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace educ_system.Models
{
    public partial class Subject
    {
        public Subject()
        {
            Teachers = new HashSet<Teacher>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "The field must not be empty")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z]*$", ErrorMessage = "The field must be alphabet only, first letter upcast")]
        public string Name { get; set; }
        public virtual ICollection<Teacher> Teachers { get; set; }
    }
}
