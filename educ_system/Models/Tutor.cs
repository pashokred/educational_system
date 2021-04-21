using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace educ_system.Models
{
    public partial class Tutor
    {
        public Tutor()
        {
            Groups = new HashSet<Group>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "The field must not be empty")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z]*$", ErrorMessage = "The field must be alphabet only, first letter upcast")]
        public string Name { get; set; }
        public string Info { get; set; }

        public virtual ICollection<Group> Groups { get; set; }
    }
}
